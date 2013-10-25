using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.UserInterface;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey
{
    public class DeviceSettings : ISettings
    {
        public int AdapterOrdinal { get; set; }

        //public string AdapterName
        //{
        //    get
        //    {
        //        using (Factory factory = new Factory())
        //        using (Adapter adapter = factory.GetAdapter(AdapterOrdinal))
        //            return adapter.Description.Description;
        //    }
        //}

        public DeviceCreationFlags CreationFlags { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }
        public float Dpi { get; set; }

        public Format Format { get; set; }

        public Size ScreenSize { get { return new Size(ScreenWidth, ScreenHeight); } }

        public SampleDescription SampleDescription { get; set; }

        public bool IsStereo { get; set; }

        public bool IsWindowed { get; set; }

        public float AspectRatio
        {
            get { return ScreenWidth / (float)ScreenHeight; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Adapter[{0}]: \n", AdapterOrdinal); //AdapterName);
            sb.AppendFormat("\tResolution: {0}x{1} Format: {2} M{3}Q{4}\n", ScreenWidth, ScreenHeight, Format, SampleDescription.Count, SampleDescription.Quality);
            sb.AppendFormat("\tRendering is {0}\n", IsWindowed ? "Windowed" : "Full Screen");
            sb.AppendFormat("\tStereo is {0}", IsStereo ? "Enabled" : "Disabled");

            return sb.ToString();
        }
    }
}