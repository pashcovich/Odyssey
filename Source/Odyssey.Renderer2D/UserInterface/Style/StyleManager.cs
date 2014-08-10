#region Using Directives

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics;
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

            var device = services.GetService<IDirect2DService>().Direct2DDevice;
            fontLoader = new NativeFontLoader(services);
            fontCollection = ToDispose(new FontCollection(device.DirectWriteFactory, fontLoader, fontLoader.Key));
            content.AssetsLoaded -= InitializeFontCollection;
        }

        [Pure]
        public bool ContainsTheme(string themeName)
        {
            return content.Contains(themeName);
        }

        void CheckTheme(string themeName)
        {
            if (!ContainsTheme(themeName))
                throw new ArgumentException(string.Format("Theme [{0}] not found.", themeName));
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
            CheckTheme(themeName);
            return content.Load<Theme>(themeName);
        }

        public ControlStyle GetControlStyle(string themeName, string controlClass)
        {
            CheckTheme(themeName);

            var theme = content.Load<Theme>(themeName);

            var style = theme.GetResource<ControlStyle>(controlClass);
            return style;
        }

        public TextDescription GetTextStyle(string themeName, string controlClass)
        {
            CheckTheme(themeName);

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
    }
}