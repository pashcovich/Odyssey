using Odyssey.Engine;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using Odyssey.Utilities.Text;
using SharpDX;
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml wrapper class for the abstract BaseControl class.
    /// </summary>
    public abstract class XmlUIElement
    {
        protected XmlUIElement()
        {
            Id = string.Empty;
            PositionString = string.Empty;
        }

        protected XmlUIElement(UIElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null, "control");

            Id = element.Name;
            Position = element.Position;
            Size = new Size2F(element.Width, element.Height);
            PositionString = (element.Position != Vector2.Zero) ? Text.EncodeVector2(element.Position) : string.Empty;
            Width = element.Width;
            Height = element.Height;

            IsEnabled = element.IsEnabled;
            IsVisible = element.IsVisible;
       }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public bool IsEnabled { get; set; }
        [XmlAttribute]
        public bool IsVisible { get; set; }

        [XmlAttribute("Position")]
        public string PositionString { get; set; }

        [XmlIgnore]
        internal Vector2 Position { get; private set; }
        [XmlAttribute]
        internal float Width { get; private set; }
        [XmlAttribute]
        internal float Height { get; private set; }

        [XmlIgnore]
        internal Size2F Size { get; private set; }

    }
}