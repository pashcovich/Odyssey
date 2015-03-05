#region Using Directives

using Odyssey.Core;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public class ContentPresenter : UIElement
    {
        /// <summary>
        ///     The key to the Content dependency property.
        /// </summary>
        public static readonly PropertyKey<UIElement> ContentPropertyKey = new PropertyKey<UIElement>("ContentKey", typeof (ContentPresenter),
            DefaultValueMetadata.Static<UIElement>(null), ObjectInvalidationMetadata.New<UIElement>(ContentInvalidationCallback));

        public UIElement Content
        {
            get { return DependencyProperties.Get(ContentPropertyKey); }
            set { DependencyProperties.Set(ContentPropertyKey, value); }
        }


        public override void Render()
        {
            if (Content != null)
                Content.Render();
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            Content.Measure(availableSizeWithoutMargins);
            return availableSizeWithoutMargins;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            Content.Arrange(availableSizeWithoutMargins);
            return availableSizeWithoutMargins;
        }

        private static void ContentInvalidationCallback(object propertyOwner, PropertyKey<UIElement> propertyKey, UIElement oldContent)
        {
            var presenter = (ContentPresenter) propertyOwner;

            if (oldContent == presenter.Content)
                return;

            if (oldContent != null)
                SetParent(oldContent, null);

            if (presenter.Content != null)
                SetParent(presenter.Content, presenter);

            if (!presenter.DesignMode)
                presenter.InvalidateMeasure();
        }

    }
}