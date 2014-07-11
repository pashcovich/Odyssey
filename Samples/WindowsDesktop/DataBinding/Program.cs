#region Using Directives

using SharpDX;
using System;

#endregion Using Directives

namespace DataBinding
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Configuration.EnableObjectTracking = true;

            using (var app = new DataBindingApplication())
            {
                app.Run();
            }
        }
    }
}