#region Using Directives

using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using System;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey
{
    public class DesktopApplication : Application
    {
        protected DesktopApplication()
            : base(typeof(DesktopApplicationPlatform))
        {
        }

        public static IEnumerable<KeyValuePair<Type, Type>> RequiredServices
        {
            get
            {
                return new[]
                {
                    new KeyValuePair<Type, Type>(typeof (IUserInterfaceState), typeof (DesktopUserInterfaceManager)),
                    new KeyValuePair<Type, Type>(typeof (IStyleService), typeof (StyleManager)),
                };
            }
        }
    }
}