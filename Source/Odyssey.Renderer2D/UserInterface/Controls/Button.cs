#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
//
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
//
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion License

#region Using Directives

using Odyssey.Interaction;
using Odyssey.UserInterface.Data;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public class Button : ButtonBase
    {
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            base.OnPointerEnter(e);
            string animationName = ControlStatus.Highlighted.ToString();
            var animation = Animator[animationName];
            animation.Speed = 1.0f;
            Animator.Play(animationName);
        }

        protected override void OnPointerExited(PointerEventArgs e)
        {
            base.OnPointerExited(e);
            string animationName = ControlStatus.Highlighted.ToString();
            var animation = Animator[animationName];
            animation.Speed = -1.0f;
            Animator.Play(animationName);
        }

        protected override void CreateDefaultTemplate()
        {
            string typeName = GetType().Name;
            Template = new DataTemplate
            {
                Key = string.Format("{0}.Template", typeName),
                DataType = GetType(),
                VisualTree = new Border()
                {
                    Content = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center }
                }
            };
        }
    }
}