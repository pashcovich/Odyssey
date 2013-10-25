using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Rendering2D;
using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public abstract class Control : UIElement, IControl
    {
        private ControlDescription description;
        private Size size;
        private TextDescription textDescription;

        protected Control(string id, string descriptionClass)
            : base(id)
        {
            ApplyControlDescription(StyleManager.GetControlDescription(descriptionClass));
            ShapeMap = new ShapeMap(Description);
        }

        protected IShape[] ActiveStyle { get; set; }
        protected ShapeMap ShapeMap { get; set; }

        #region Properties

        public Color4 Background { get; set; }
        public Color4 BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the height and width of the client area of the control.
        /// </summary>
        /// <value>A <see cref = "Engine.Drawing.Size" /> that represents the dimensions of the
        /// client area of the control.</value>
        public Size ClientSize { get; set; }

        public Size ContentAreaSize { get; set; }

        /// <summary>
        /// Gets or sets the <see cref = "Description" /> class to use for this control.
        /// </summary>
        /// <value>The name of the <see cref = "TextStyle" /> class to use. Classes are defined in a
        /// <c><b>[Theme Name]</b> TextStyles.ots</c> file.</value>
        public string ControlDescriptionClass
        {
            get
            {
                return description.Name;
            }
            set
            {
                if (description.Name != value)
                {
                    Description = StyleManager.GetControlDescription(value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref = "Description" /> to use for this control.
        /// </summary>
        /// <value>The <see cref = "Description" /> object that contains information on how to style
        /// the text appearance of this control.</value>
        public ControlDescription Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    ApplyControlDescription(value);
                    OnControlStyleChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets the height and width of the control.
        /// </summary>
        /// <value>The <see cref = "Engine.Geometry.Size">Size</see> that represents the height and
        /// width of the control in pixels.</value>
        public Size Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size == value) return;
                size = value;
                ContentAreaSize = new Size(Size.Width - (Description.BorderSize.Horizontal + Description.Padding.Horizontal),
                                 Size.Height - (Description.BorderSize.Vertical + Description.Padding.Vertical));

                ClientSize = new Size(Size.Width - Description.BorderSize.Horizontal,
                                      Size.Height - Description.BorderSize.Vertical);

                Width = size.Width;
                Height = size.Height;

                if (DesignMode) return;
                OnSizeChanged(EventArgs.Empty);
            }
        }
        /// <summary>
        /// Gets or sets the <see cref = "TextDescription" /> to use for this control.
        /// </summary>
        /// <value>The <see cref = "TextDescription" /> object that contains information on how to
        /// format the text of this control.</value>
        public TextDescription TextDescription
        {
            get
            {
                return textDescription;
            }
            set
            {
                if (textDescription != value)
                {
                    textDescription = value;
                    OnTextDescriptionChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref = "TextStyle" /> class to use for this control.
        /// </summary>
        /// <value>The name of the <see cref = "TextStyle" /> class to use. Classes are defined in a
        /// <c><b>[Theme Name]</b> TextStyles.ots</c> file.</value>
        /// <remarks>
        /// If you <b>set</b> the class, it will cause that class to be recovered from the static
        /// cache in memory. If there's no <see cref = "TextStyle" /> object present in memory, the
        /// <b>Default</b> style will be used instead.
        /// </remarks>
        public string TextDescriptionClass
        {
            get
            {
                return textDescription.Name;
            }
            set
            {
                if (textDescription.Name != value)
                {
                    textDescription = StyleManager.GetTextDescription(value);
                    OnTextDescriptionChanged(EventArgs.Empty);
                }
            }
        }

        #endregion Properties

        #region Protected methods

        protected void ApplyControlDescription(ControlDescription newDescription)
        {
            description = newDescription;

            if (!description.Size.IsEmpty)
                Size = description.Size;

            ContentAreaSize = new Size(Size.Width - (Description.BorderSize.Horizontal + Description.Padding.Horizontal),
                                  Size.Height - (Description.BorderSize.Vertical + Description.Padding.Vertical));

            ClientSize = new Size(Size.Width - Description.BorderSize.Horizontal,
                                  Size.Height - Description.BorderSize.Vertical);

            TopLeftPosition = new Vector2(description.Padding.Left + description.BorderSize.Left,
                                          description.Padding.Top + description.BorderSize.Top);

            textDescription = StyleManager.GetTextDescription(description.TextStyleClass);
        }

        #endregion Protected methods

        public abstract void Initialize(IDirectXProvider directX);

        public abstract void Render(IDirectXTarget target);
    }
}