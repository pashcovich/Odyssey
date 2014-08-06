using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public class AssetsLoadedEventArgs : EventArgs
    {
        public string AssetListPath { get; private set; }

        public AssetsLoadedEventArgs(string assetListPath)
        {
            AssetListPath = assetListPath;

            
        }
    }
}
