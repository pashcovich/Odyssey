#region Using directives

using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Xml;
using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

#endregion Using directives

namespace Odyssey.UserInterface.Style
{
    public static class StyleManager
    {
        private const int DefaultFontSize = 20;

        private const string DefaultNamespace = "http://www.avengersutd.com";
        private const string StyleDeclarationsTag = "StyleDeclarations";

        private static Dictionary<string, ControlDescription> controlStyles;
        private static Dictionary<string, TextDescription> textStyles;

        #region Properties

        public static int ControlDescriptionCount
        {
            get { return controlStyles.Count; }
        }
        public static IEnumerable<ControlDescription> ControlDescriptions
        {
            get
            {
                return controlStyles != null ? controlStyles.Values : null;
            }
        }
        public static int DefaultFontSizeScaled
        {
            // not yet fully implemented
            get { return DefaultFontSize; }
        }
        public static int TextDescriptionCount
        {
            get { return textStyles.Count; }
        }
        public static IEnumerable<TextDescription> TextDescriptions
        {
            get
            {
                return textStyles != null ? textStyles.Values : null;
            }
        }
        #endregion Properties

        public static void AddControlStyle(ControlDescription newStyle)
        {
            controlStyles.Add(newStyle.Name, newStyle);
        }

        public static void AddTextStyle(TextDescription newTextStyle)
        {
            textStyles.Add(newTextStyle.Name, newTextStyle);
        }

        public static bool ContainsControlStyle(string id)
        {
            return controlStyles.ContainsKey(id);
        }

        public static bool ContainsTextStyle(string id)
        {
            return textStyles.ContainsKey(id);
        }

        public static ControlDescription GetControlDescription(string id)
        {
            Contract.Requires<InvalidOperationException>(ControlDescriptionCount > 0, "No Control Descriptions loaded.");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(id));
            if (id == null || !controlStyles.ContainsKey(id))
                return controlStyles[ControlDescription.Error];
            else
                return controlStyles[id];
        }

        public static TextDescription GetTextDescription(string id)
        {
            Contract.Requires<InvalidOperationException>(ControlDescriptionCount > 0, "No Text Descriptions loaded.");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(id));
            if (id == null || !textStyles.ContainsKey(id))
            {
                return textStyles[TextDescription.Error];
            }
            else
                return textStyles[id];
        }

        public static void LoadControlDescription(string filename)
        {
            XmlControlDescription[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlControlDescription[]));
            MemoryStream stream = new MemoryStream(NativeFile.ReadAllBytes(filename));

            xmlcStyle = (XmlControlDescription[])xmlSerializer.Deserialize(stream);

            if (controlStyles == null)
                controlStyles = new Dictionary<string, ControlDescription>();

            for (int i = 0; i < xmlcStyle.Length; i++)
            {
                controlStyles.Add(xmlcStyle[i].Name, xmlcStyle[i].ToControlDescription());
            }
        }

        public static void LoadTextDescription(string filename)
        {
            XmlTextDescription[] xmlcStyle;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlTextDescription[]));
            MemoryStream stream = new MemoryStream(NativeFile.ReadAllBytes(filename));

            xmlcStyle = (XmlTextDescription[])xmlSerializer.Deserialize(stream);

            if (textStyles == null)
                textStyles = new Dictionary<string, TextDescription>();

            foreach (XmlTextDescription t in xmlcStyle)
            {
                textStyles.Add(t.Name, t.ToTextDescription());
            }
        }

        public static void Remove(ControlDescription style)
        {
            controlStyles.Remove(style.Name);
        }

        //public static void SaveControlStyles(Overlay Overlay)
        //{
        //    List<XmlControlStyle> xmlControlStyles = new List<XmlControlStyle>();

        // foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(Overlay)) {
        // xmlControlStyles.Add(new XmlControlStyle(ctl.ControlStyle)); }

        // XmlSerializer xmlSerializer = new XmlSerializer(typeof (List<XmlControlStyle>));
        // XmlWriterSettings xmlSettings = new XmlWriterSettings(); xmlSettings.Indent = true;
        // XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("Style",
        // DefaultNamespace) ;

        // using (XmlWriter xmlWriter = XmlWriter.Create("styles.xml", xmlSettings)) {
        // xmlWriter.WriteStartDocument(); xmlWriter.WriteComment( string.Format( "This is an
        // Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you
        // don't know what you are doing. Visit the Odyssey UI website at
        // http: //www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the
        // Odyssey UI!", DateTime.Now.ToString("f"),
        // Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
        // xmlSerializer.Serialize(xmlWriter, xmlControlStyles, ns); xmlWriter.WriteEndDocument();
        // xmlWriter.Flush(); }
        //}

        //public static void SaveTextStyles(TextDescription style)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlTextDescription[]));
        //    XmlWriterSettings xmlSettings = new XmlWriterSettings();
        //    xmlSettings.Indent = true;
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("Style", DefaultNamespace);

        // using (XmlWriter xmlWriter = XmlWriter.Create("textstyles.xml", xmlSettings)) {
        // xmlWriter.WriteStartDocument(); xmlWriter.WriteComment( string.Format( "This is an
        // Odyssey UI Style Declaration file, generated on {0}.\nPlease do not modify it if you
        // don't know what you are doing. Visit the Odyssey UI website at
        // http: //www.avengersutd.com/wiki/OdysseyUI for more information.\nThanks for using the
        // Odyssey UI!", DateTime.Now.ToString("f"))); xmlSerializer.Serialize(xmlWriter, new
        // XmlTextDescription[] {new XmlTextDescription(style)}, ns); xmlWriter.WriteEndDocument();
        // xmlWriter.Flush(); }
        //}
    }
}