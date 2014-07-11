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

using System.Windows;
using System.Windows.Controls;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.View
{
    public class BrowserBehavior
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof (string), typeof (BrowserBehavior), new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof (WebBrowser))]
        public static string GetHtml(WebBrowser browser)
        {
            return (string) browser.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser browser, string value)
        {
            browser.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var browser = dependencyObject as WebBrowser;
            if (browser != null)
            {
                browser.NavigateToString(e.NewValue as string);
            }
        }
    }
}