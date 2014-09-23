using System.IO;
using System.Runtime.CompilerServices;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.PostProcessing;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Texture2D = Odyssey.Graphics.Texture2D;

namespace Odyssey.Organization.Commands
{
    public class TakeScreenshotCommand  : RenderToTextureCommandBase
    {
        private readonly string path;

        public TakeScreenshotCommand(IServiceRegistry services, string path, Texture2DDescription textureDescription)
            : base(services, textureDescription, OutputRule.Output)
        {
            this.path = path;
        }

        public override void Render()
        {
            foreach (var texture in Inputs)
            {
                var newT = Texture2D.New(DeviceService.DirectXDevice,new Texture2DDescription()
                        {
                            CpuAccessFlags = CpuAccessFlags.None,
                            Format = Format.R8G8B8A8_UNorm,
                            Height = texture.Height,
                            Usage = ResourceUsage.Default, 
                            Width = texture.Width,
                            ArraySize = 1,
                            SampleDescription = new SampleDescription(1, 0),
                            BindFlags = BindFlags.None,
                            MipLevels = 1,
                            OptionFlags = texture.Description.OptionFlags
                        });

                newT.Initialize();
                DeviceService.DirectXDevice.Copy(texture, 0, newT, 0, texture.Description.Format);
                newT.Save(path, ImageFileType.Png);
            }
        }

    }
}
