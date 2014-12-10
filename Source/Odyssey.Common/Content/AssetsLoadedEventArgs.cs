#region Using Directives

using System;

#endregion

namespace Odyssey.Content
{
    public class AssetsLoadedEventArgs : EventArgs
    {
        public AssetsLoadedEventArgs(string assetListPath)
        {
            AssetListPath = assetListPath;
        }

        public string AssetListPath { get; private set; }
    }
}