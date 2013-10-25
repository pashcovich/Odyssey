using Odyssey.Collections;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Meshes;
using Odyssey.Utils.Logging;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public class ContentManager
    {
        List<IResourceResolver> resolvers;
        List<IResourceReader> readers;
        Cache<string, CacheNode<object>> cachedAssets;
        SortedDictionary<string, object> managedAssets;

        public List<IResourceReader> Readers { get { return readers; } }
        public List<IResourceResolver> Resolvers { get { return resolvers; } }

        public ContentManager()
        {
            cachedAssets = new Cache<string, CacheNode<object>>(20 * 1024 * 1024);
            managedAssets = new SortedDictionary<string, object>();
            resolvers = new List<IResourceResolver>();
            readers = new List<IResourceReader>();
        }

        public bool Contains(string assetName)
        {
            return cachedAssets.ContainsKey(assetName) || managedAssets.ContainsKey(assetName);
        }

        public void Store<T>(string fileName, string assetName)
        {
            Contract.Requires(!Contains(assetName));

            Stream stream = FindStream(fileName);
            if (stream == null)
                throw new Exception();

            object asset = LoadAsset<T>(fileName, stream);
            Store(assetName, asset);
        }

        public T Get<T>(string assetName)
        {
            object asset = null;

            if (TryGetAsset(assetName, out asset))
                return (T)asset;

            Stream stream = FindStream(assetName);
            if (stream == null)
                throw new KeyNotFoundException(string.Format("Asset [{0}] not found in the system.", assetName));

            asset = LoadAsset<T>(assetName, stream);
            Store(assetName, asset);

            return (T)asset;
            
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
                StoreUnmanaged(asset, assetName);
            else
                StoreManaged(assetName, asset);
        }

        void StoreManaged(string assetName, object asset)
        {
            Contract.Requires<InvalidOperationException>(!managedAssets.ContainsKey(assetName));
            managedAssets.Add(assetName, asset);
        }

        void StoreUnmanaged(object asset, string assetName)
        {
            Contract.Requires<InvalidOperationException>(!cachedAssets.ContainsKey(assetName));
            int size=FindSize(asset);
            cachedAssets.Add(assetName, new CacheNode<object>(size, asset));
            LogEvent.Io.Info(string.Format("Loaded {0} into cache.\n{1:f2} kB used; {2:f2} kB free.", assetName, 
                size/1024f, cachedAssets.FreeSpace/1024f));
        }


        int FindSize(object asset)
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

        static bool IsUnmanagedAsset(Type type)
        {
            var types = type.GetTypeInfo().ImplementedInterfaces;
            return types.Contains(typeof(IDisposable));
            //return type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDisposable));
        }


        object LoadAsset<T>(string filename, Stream stream)
        {
            object result = null;
            IResourceReader resourceReader = null;

            foreach (IResourceReader rReader in readers)
            {
                var attributes = CustomAttributeExtensions.GetCustomAttributes<SupportedTypeAttribute>(rReader.GetType().GetTypeInfo());
                if (attributes.Any(a => a.SupportedType == typeof(T)))
                    resourceReader = rReader;
            }
            if (resourceReader == null)
                throw new NotSupportedException(string.Format("Type {0} is not supported.", typeof(T).Name));

            result = resourceReader.ReadContent(filename, stream);
            stream.Dispose();
            return result;
        }

        private Stream FindStream(string assetName)
        {
            Stream stream = null;
            List<IResourceResolver> lResolvers;
            lock (resolvers)
            {
                lResolvers = new List<IResourceResolver>(resolvers);
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

        public void Dispose()
        {
            foreach (IResourceReader reader in readers)
            {
                IDisposable disposable = reader as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            if (!cachedAssets.IsEmpty)
                foreach (var node in cachedAssets)
                {
                    ((IDisposable)node.Object).Dispose();
                }
        }
    }
}
