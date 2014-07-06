using Odyssey.Engine;
using Odyssey.Graphics.Rendering2D.Effects;
using Odyssey.UserInterface.Controls;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D
{
    public class StereoRenderer : Renderer
    {
        Bitmap1 uiSurface;
        Effect<StereoEffect> stereoEffect;
        Effect effectGraph;

        protected override void OnInitialize(InitializeDirectXEventArgs e)
        {
            IDirect2DProvider direct2D = e.DirectX.Direct2D;
            BitmapProperties1 bitmapProperties = new BitmapProperties1()
            {
                DpiX = e.Settings.Dpi,
                DpiY = e.Settings.Dpi,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                BitmapOptions = BitmapOptions.Target, 
            };
            uiSurface = ToDispose(new Bitmap1(direct2D.Context, 
                new Size2(e.Settings.ScreenWidth, e.Settings.ScreenHeight), bitmapProperties));

            Overlay = ToDispose(Overlay.FromDescription(
            new OverlayDescription(width: e.Settings.ScreenWidth,
                height: e.Settings.ScreenHeight)));
            Overlay.BeginDesign();

            Panel panel = new Panel() { Position = new SharpDX.Vector2(512, 192), Width = 320, Height = 92 };
            Label label = new Label() { Position = new SharpDX.Vector2(8, 8), Text = "stereo" };

            panel.Add(label);
            Overlay.Add(panel);

            Overlay.EndDesign(e.DirectX);

            effectGraph = CreateEffectGraph(direct2D);
            direct2D.Context.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
        }

        Effect CreateEffectGraph(IDirect2DProvider direct2D)
        {
            direct2D.Factory.RegisterEffect<StereoEffect>();
            stereoEffect = ToDispose(new Effect<StereoEffect>(direct2D.Context));
            stereoEffect.SetValue((int)StereoProperties.Depth, 10f);
            stereoEffect.SetInput(0, uiSurface, true);

            return stereoEffect;
        }

        protected override void Begin(IDirectXTarget target)
        {
            target.Device.Target = uiSurface; 
            target.Device.BeginDraw();
            target.Device.Clear(Color.Transparent);
        }

        protected override void OnRender()
        {
            Overlay.Render(target);
        }

        protected override void End(IDirectXProvider directX)
        {
            var context = directX.Direct2D.Context;
            context.EndDraw();
            IOdysseyTarget stereoTarget = (IOdysseyTarget)target;

            context.Target = stereoTarget.BitmapTarget;
            context.BeginDraw();

            //context.DrawImage(uiSurface, InterpolationMode.Linear, CompositeMode.SourceOver);
            context.DrawImage(effectGraph);
            context.EndDraw();

        }
    }
}