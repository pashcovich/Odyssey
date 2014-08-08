#region Using Directives

using System.Diagnostics.Contracts;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
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

        public ControlStyle GetControlStyle(string themeName, string controlClass)
        {
            CheckTheme(themeName);

            var theme = content.Load<Theme>(themeName);

            var style = theme.GetStyle(controlClass);
            return style;
        }

        public Gradient GetGradient(string themeName, string resourceName)
        {
            CheckTheme(themeName);

            return (Gradient)content.Load<Theme>(themeName).GetResource(resourceName);
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
    }
}