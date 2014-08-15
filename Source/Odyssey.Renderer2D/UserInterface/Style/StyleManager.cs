#region Using Directives

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SharpDX.DirectWrite;
using Font = Odyssey.Content.Font;
using Path = System.IO.Path;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public class StyleManager : Component, IStyleService
    {
        private readonly IAssetProvider content;
        private readonly Dictionary<string, IResource> resources;
        private FontCollection fontCollection;
        private NativeFontLoader fontLoader;
        private readonly IServiceRegistry services;
        public FontCollection FontCollection { get { return fontCollection; } }

        public StyleManager(IServiceRegistry services)
        {
            this.services = services;
            content = services.GetService<IAssetProvider>();

            content.AddMapping("Theme", typeof (Theme));
            content.AddMapping("TextDefinitions", typeof (TextDescription));
            content.AddMapping("Font", typeof(Font));

            content.AssetsLoaded += InitializeFontCollection;
            resources = new Dictionary<string, IResource>();
        }

        void InitializeFontCollection(object sender, AssetsLoadedEventArgs e)
        {
            var assetListName = Path.GetFileNameWithoutExtension(e.AssetListPath);
            if (!string.Equals(assetListName, "fonts"))
                return;

            var d2DService = services.GetService<IDirect2DService>();
            d2DService.DeviceDisposing += (s, args) => Unload();

            fontLoader = new NativeFontLoader(services);
            fontCollection = new FontCollection(d2DService.Direct2DDevice, fontLoader, fontLoader.Key);
            content.AssetsLoaded -= InitializeFontCollection;
        }

        [Pure]
        public bool ContainsResource(string resourceName)
        {
            return resources.ContainsKey(resourceName);
        }

        public void AddResource(IResource resource)
        {
            Contract.Requires<ArgumentException>(!ContainsResource(resource.Name), "A resource with the same name is already in the collection");

            resources.Add(resource.Name, resource); 
            var disposableResource = resource as IDisposable;
            if (disposableResource != null)
                ToDispose(disposableResource);
        }

        public Theme GetTheme(string themeName)
        {
            return content.Load<Theme>(themeName);
        }

        public TextDescription GetTextStyle(string themeName, string controlClass)
        {
            var theme = content.Load<TextDescription[]>(themeName);

            var description = theme.FirstOrDefault(t => t.Name == controlClass);

            if (description == default(TextDescription))
            {
                LogEvent.UserInterface.Warning("[{0}] Text Style not found.", controlClass);
                return theme.First(c => c.Name == ControlStyle.Error);
            }
            else return description;
        }

        public static T[] LoadDefinitions<T>(Stream stream)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T[]));
            T[] definitions = (T[]) xmlSerializer.Deserialize(stream);
            return definitions;
        }

        public TResource GetResource<TResource>(string resourceName) where TResource : class, IResource
        {
            if (!ContainsResource(resourceName)) 
                throw new ArgumentException(string.Format("Resource '{0}' not found", resourceName));

            var resource = resources[resourceName];
            TResource resultResource = resource as TResource;
            if (resultResource == null)
                throw new ArgumentException(string.Format("Resource '{0}' of type '{1}' cannot be cast to '{2}'",
                    resourceName, resource, typeof(TResource).Name));
            return resultResource;
        }

        public IEnumerable<IResource> Resources
        {
            get { throw new NotImplementedException(); }
        }

        void Unload()
        {
            // FontCollection and FontLoader have to be unregistered and disposed before
            // the DirectWriteFactory is disposed
            var dwFactory = services.GetService<IDirect2DService>().Direct2DDevice.DirectWriteFactory;
            dwFactory.UnregisterFontCollectionLoader(fontLoader);
            dwFactory.UnregisterFontFileLoader(fontLoader);
            SharpDX.Utilities.Dispose(ref fontCollection);
            SharpDX.Utilities.Dispose(ref fontLoader);
        }

    }
}