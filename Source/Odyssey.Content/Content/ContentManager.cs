using Odyssey.Utilities.Collections;
using Odyssey.Utilities.Logging;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using SharpDX.Collections;
using SharpDX;
using SharpDX.IO;
using SharpYaml.Serialization;

namespace Odyssey.Content
{
    public class ContentManager : IAssetProvider
    {
        Application application;
        readonly Dictionary<string, string> assetReferences;
        readonly Cache<string, CacheNode<object>> cachedAssets;
        readonly SortedDictionary<string, object> managedAssets;

        public IServiceRegistry Services { get {return application.Services;}}

        public ObservableCollection<IResourceReader> Readers { get; private set; }
        public ObservableCollection<IResourceResolver> Resolvers { get; private set; }

        public ContentManager()
        {
            cachedAssets = new Cache<string, CacheNode<object>>(40 * 1024 * 1024);
            managedAssets = new SortedDictionary<string, object>();
            Resolvers = new ObservableCollection<IResourceResolver>();
            Readers = new ObservableCollection<IResourceReader>();
            assetReferences = new Dictionary<string, string>();

            // Init ContentManager
            Resolvers.Add(new FileSystemResourceResolver(Global.Assets));
            Readers.Add(new TextureReader());
            Readers.Add(new EffectReader());
            Readers.Add(new OmdReader());
            Readers.Add(new ControlDefinitionsReader());
            Readers.Add(new TextDefinitionsReader());

        }

        [Pure]
        public bool Contains(string assetName)
        {
            return cachedAssets.ContainsKey(assetName) || managedAssets.ContainsKey(assetName);
        }

        public void Store<T>(string fileName, string assetName)
        {
            Stream stream = FindStream(fileName);
            if (stream == null)
                throw new Exception();

            object asset = LoadAsset<T>(fileName, stream);
            Store(assetName, asset);
            assetReferences.Add(assetName, fileName);
        }

        internal void Store(AssetIdentifier assetIdentifier)
        {
            Stream stream = FindStream(assetIdentifier.Path);
            if (stream == null)
                throw new Exception();

            Type type = ContentMapper.Map(assetIdentifier.Type);
            object asset = LoadAsset(assetIdentifier.Path, stream, type);
            Store(assetIdentifier.Name, asset);
            assetReferences.Add(assetIdentifier.Name, assetIdentifier.Path);
        }

        public T Get<T>(string assetName)
        {
            object asset;

            if (TryGetAsset(assetName, out asset))
                return (T)asset;

            Stream stream = FindStream(assetName);
            if (stream == null)
                throw new KeyNotFoundException(string.Format("Asset [{0}] not found in the system.", assetName));

            asset = LoadAsset<T>(assetName, stream);
            Store(assetName, asset);

            return (T)asset;
        }

        public IEnumerable<T> SelectAssets<T>()
        {
            return (from cacheNode in cachedAssets.GetValues().OfType<CacheNode<T>>()
                    select cacheNode.Object);
        }

        public bool TryGetAsset(string assetName, out object asset)
        {
            asset = null;

            if (cachedAssets.ContainsKey(assetName)) 
                asset = cachedAssets[assetName].Object;
            else if (managedAssets.ContainsKey(assetName)) 
                asset = managedAssets[assetName];

            return asset != null;
        }

        public void Store<T>(string assetName, T asset)
        {
            if (IsUnmanagedAsset(asset.GetType()))
                StoreUnmanaged(assetName, asset);
            else
                StoreManaged(assetName, asset);
        }

        void StoreManaged(string assetName, object asset)
        {
            Contract.Requires<InvalidOperationException>(!managedAssets.ContainsKey(assetName));
            managedAssets.Add(assetName, asset);
        }

        void StoreUnmanaged(string assetName, object asset)
        {
            Contract.Requires<InvalidOperationException>(Contains(assetName));
            int size=FindSize(asset);
            cachedAssets.Add(assetName, new CacheNode<object>(size, asset));
            LogEvent.Io.Info(string.Format("Loaded {0} into cache.\n{1:f2} kB used; {2:f2} kB free.", assetName, 
                size/1024f, cachedAssets.FreeSpace/1024f));
        }


        static int FindSize(object asset)
        {
            Contract.Requires<InvalidOperationException>(IsUnmanagedAsset(asset.GetType()), "FindSize called on managed asset.");

            Resource resource = asset as Resource;
            if (resource != null)
            {
                switch (resource.Dimension)
                {
                    case ResourceDimension.Texture2D:
                        Texture2DDescription t2Desc = ((Texture2D)resource).Description;
                        int format = (int)SharpDX.DXGI.FormatHelper.SizeOfInBytes(t2Desc.Format);
                        return format * t2Desc.Width * t2Desc.Height * t2Desc.ArraySize;
                }
            }

            IByteSize sizableAsset = asset as IByteSize;
            if (sizableAsset != null)
                return sizableAsset.ByteSize;

            throw new NotImplementedException("Loading a " + asset.GetType().Name + " is not implemented.");
        }

        [Pure]
        static bool IsUnmanagedAsset(Type type)
        {
            var types = type.GetTypeInfo().ImplementedInterfaces;
            return types.Contains(typeof(IDisposable));
        }


        object LoadAsset<T>(string filename, Stream stream)
        {
            return LoadAsset(filename, stream, typeof (T));
        }

        object LoadAsset(string filename, Stream stream, Type assetType)
        {
            IResourceReader resourceReader = null;

            foreach (IResourceReader rReader in from rReader in Readers 
                let attributes = rReader.GetType().GetTypeInfo().GetCustomAttributes<SupportedTypeAttribute>() 
                where attributes.Any(a => a.SupportedType == assetType) select rReader)
            {
                resourceReader = rReader;
            }
            if (resourceReader == null)
                throw new NotSupportedException(string.Format("Type {0} is not supported.", assetType.Name));

            object result = resourceReader.ReadContent(this, filename, stream);
            stream.Dispose();
            return result;
        }

        private Stream FindStream(string assetName)
        {
            Stream stream = null;
            List<IResourceResolver> lResolvers;
            lock (Resolvers)
            {
                lResolvers = new List<IResourceResolver>(Resolvers);
            }

            if (lResolvers.Count == 0)
                throw new InvalidOperationException("No resolver registered to this content manager");

            foreach (IResourceResolver rResolver in lResolvers)
            {
                stream = rResolver.Resolve(assetName);
                if (stream != null)
                    break;
            }
            return stream;
        }

        public void Unload()
        {
            foreach (IDisposable disposable in Readers.OfType<IDisposable>())
            {
                disposable.Dispose();
            }

            if (!cachedAssets.IsEmpty)
                foreach (var node in cachedAssets)
                {
                    ((IDisposable)node.Object).Dispose();
                }

            SharpDX.Toolkit.Graphics.GraphicsAdapter.Dispose();
        }

        public void InitializeApplication(Application application)
        {
            this.application = application;
            Services.AddService(typeof(IAssetProvider), this);

#if DEBUG
            Services.AddService(typeof(SharpDX.Toolkit.Graphics.IGraphicsDeviceService), new ToolkitDeviceProvider());
            Readers.Add(new ModelReader());
#endif
        }

        public void LoadAssetList(string assetListFile)
        {
            var serializer = new SharpYaml.Serialization.Serializer();
            serializer.Settings.RegisterTagMapping("Asset", typeof(AssetIdentifier));
            serializer.Settings.RegisterTagMapping("Assets", typeof(AssetIdentifier[]));
            AssetIdentifier[] assetList;
            using (var nativeStream = new NativeFileStream(assetListFile, NativeFileMode.Open, NativeFileAccess.Read))
                assetList = serializer.Deserialize<AssetIdentifier[]>(nativeStream);

            foreach (AssetIdentifier assetIdentifier in assetList)
            {
                Store(assetIdentifier);
            }
        }
    }
}
