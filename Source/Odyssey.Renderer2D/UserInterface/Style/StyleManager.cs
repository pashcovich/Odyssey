#region Using Directives

using Odyssey.Content;
using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public class StyleManager : IStyleService
    {
        private readonly IAssetProvider content;

        public StyleManager(IServiceRegistry services)
        {
            content = services.GetService<IAssetProvider>();

            content.AddMapping("ControlDefinitions", typeof (ControlDescription));
            content.AddMapping("TextDefinitions", typeof (TextDescription));
        }

        public ControlDescription GetControlDescription(string themeName, string controlClass)
        {
            if (!content.Contains(themeName))
                throw new InvalidOperationException(string.Format("[{0}] theme not found.", themeName));

            var theme = content.Get<ControlDescription[]>(themeName);

            var description = theme.FirstOrDefault(c => c.Name == controlClass);

            if (description == null)
            {
                LogEvent.UserInterface.Warning("[{0}] description not found.", controlClass);
                return theme.First(c => c.Name == ControlDescription.Error);
            }
            else return description;
        }

        public TextDescription GetTextDescription(string themeName, string controlClass)
        {
            if (!content.Contains(themeName))
                throw new InvalidOperationException(string.Format("[{0}] theme not found.", themeName));

            var theme = content.Get<TextDescription[]>(themeName);

            var description = theme.FirstOrDefault(t => t.Name == controlClass);

            if (description == default(TextDescription))
            {
                LogEvent.UserInterface.Warning("[{0}] description not found.", controlClass);
                return theme.First(c => c.Name == ControlDescription.Error);
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