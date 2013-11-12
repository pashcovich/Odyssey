using Odyssey.Collections;
using Odyssey.Engine;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using SharpDX.Toolkit.Graphics;
using System;
using System.IO;
using System.Linq;
using Resource = SharpDX.Direct3D11.Resource;

namespace Odyssey.Content
{
    [SupportedType(typeof(SharpDX.Direct3D11.Texture2D))]
    public class TextureReader : IResourceReader, IDeviceDependentComponent
    {
        private SharpDX.Direct3D11.Device device;

        /*
        internal static void UpdateFileList()
        {
            System.Collections.Generic.List<string> files = new System.Collections.Generic.List<string>();

            DirectoryInfo dir = new DirectoryInfo(Global.Resources);
            Uri resourcePath = new Uri(Path.GetFullPath(Global.Resources));
            foreach (FileInfo f in dir.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                Uri absolutePath = new Uri(f.FullName);
                files.Add(resourcePath.MakeRelativeUri(absolutePath).ToString());
            }
            Odyssey.Utils.Xml.Data.Serialize<System.Collections.Generic.List<string>>(files, "Resources\\resources.xml");
        }

        internal static void PerformIntegrityCheck()
        {
            //UpdateFileList();
            string[] files;
            try
            {
                files = Data.DeserializeCollection<string>(Global.Resources + "resources.xml");
            }
            catch (FileNotFoundException ex)
            {
                CriticalEvent.MissingFile.LogError(new TraceData(typeof(EffectManager), MethodBase.GetCurrentMethod(),
                            new System.Collections.Generic.Dictionary<string, string> { { "filename", ex.FileName } }), ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            bool missing = false;
            foreach (string filename in files)
            {
                if (!File.Exists(Global.Resources + filename))
                {
                    CriticalEvent.MissingFile.LogError(new TraceData(typeof(ResourceManager), MethodBase.GetCurrentMethod(),
                        new Dictionary<string,string>{{"filename", filename}}));
                    missing = true;
                }
            }
            if (missing)
                SystemState.Close((int)EventCode.CriticalFault);
        }*/


        public void Initialize(InitializeDirectXEventArgs e)
        {
            device = e.DirectX.Direct3D.Device;
        }

        public object ReadContent(string resourceName, Stream stream)
        {
            Image image = Image.Load(stream); 
            Texture2DDescription desc = new Texture2DDescription()
            {
                Width = image.Description.Width,
                Height = image.Description.Height,
                ArraySize = image.Description.ArraySize,
                BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                Usage = SharpDX.Direct3D11.ResourceUsage.Default, 
                CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                Format = image.Description.Format,
                MipLevels = 1,
                OptionFlags = image.Description.ArraySize == 6 ? ResourceOptionFlags.TextureCube : ResourceOptionFlags.None,
                SampleDescription = new SampleDescription(1, 0),
            };
            SharpDX.Direct3D11.Texture2D texture2D = new SharpDX.Direct3D11.Texture2D(device, desc, image.ToDataBox());
            return texture2D;
        }
    }
}