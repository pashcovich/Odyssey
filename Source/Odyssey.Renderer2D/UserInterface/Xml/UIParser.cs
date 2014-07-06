using Odyssey.UserInterface.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Allows Xml serialization and deserialization of interfaces built with the Odyssey UI
    /// </summary>
    public static class UIParser
    {
        private static readonly Dictionary<Type, Type> registeredWrappers;

        static UIParser()
        {
            registeredWrappers = new Dictionary<Type, Type>
                                     {
                                         //{typeof (Button), typeof (XmlButton)},
                                         {typeof (PanelBase), typeof (XmlPanel)},
                                         //{typeof (DropDownList), typeof (XmlDropDownList)}
                                     };

            //registeredWrappers.Add(typeof (CheckBox), typeof (XmlCheckBox));
            //registeredWrappers.Add(typeof (GroupBox), typeof (XmlGroupBox));
            //registeredWrappers.Add(typeof (Label), typeof (XmlLabel));
            //registeredWrappers.Add(typeof (OptionGroup), typeof (XmlOptionGroup));
            //registeredWrappers.Add(typeof (TextBox), typeof (XmlTextBox));
            //registeredWrappers.Add(typeof (TrackBar), typeof (XmlTrackBar));
            //registeredWrappers.Add(typeof (Window), typeof (XmlWindow));
        }

        public static Type GetControlTypeForWrapper(Type wrapperClass)
        {
            return (from pair in registeredWrappers where pair.Value == wrapperClass select pair.Key).FirstOrDefault();
        }

        public static Type GetWrapperTypeForControl(Type controlClass)
        {
            try { return registeredWrappers[controlClass]; }
            catch (KeyNotFoundException)
            {
                return typeof(XmlUnknownControl);
            }
        }

        public static void RegisterXmlWrapper(Type controlType, Type wrapperType)
        {
            if (!registeredWrappers.ContainsKey(controlType))
                registeredWrappers.Add(controlType, wrapperType);
        }

        //public static void SerializeOverlay(Overlay Overlay, string outputFilename)
        //{
        //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
        //    xmlWriterSettings.Indent = true;

        // XmlAttributes xmlAttributes = new XmlAttributes(); xmlAttributes.XmlArray = new
        // XmlArrayAttribute("Controls"); foreach (Type type in registeredWrappers.Values) {
        // xmlAttributes.XmlArrayItems.Add(new XmlArrayItemAttribute(type)); } XmlAttributeOverrides
        // xmlAttributeOverrides = new XmlAttributeOverrides(); xmlAttributeOverrides.Add(typeof
        // (XmlContainerControl), "XmlControlList", xmlAttributes);

        // XmlSerializer xmlSerializer = new XmlSerializer(typeof (XmlOverlay),
        // xmlAttributeOverrides); XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        // ns.Add("Style", "http://www.avengersutd.com");

        // XmlOverlay xmlOverlay = new XmlOverlay(); xmlOverlay.FromControl(Overlay);

        // using (XmlWriter xmlWriter = XmlWriter.Create(outputFilename, xmlWriterSettings)) {
        // xmlWriter.WriteStartDocument(); xmlWriter.WriteComment( string.Format( "This is an
        // Odyssey User Interface Declaration file, generated on {0}.\nPlease do not modify it if
        // you don't know what you are doing. Visit the Odyssey UI website at
        // http: //www.avengersutd.com/wiki/UserInterfaceManager for more information.\nThanks for using the
        // Odyssey UI!", DateTime.Now.ToString("f"),
        // Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
        // xmlSerializer.Serialize(xmlWriter, xmlOverlay, ns); xmlWriter.WriteEndDocument();
        // xmlWriter.Flush(); }
        //}
    }
}