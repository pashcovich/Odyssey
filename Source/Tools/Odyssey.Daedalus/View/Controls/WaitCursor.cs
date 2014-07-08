#region Using Directives

using System;
using System.Windows.Input;

#endregion Using Directives

namespace Odyssey.Tools.ShaderGenerator.View.Controls
{
    public class WaitCursor : IDisposable
    {
        private readonly Cursor previousCursor;

        public WaitCursor()
        {
            previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = previousCursor;
        }

        #endregion IDisposable Members
    }
}