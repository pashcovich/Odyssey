using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Effects
{
    public enum StereoProperties
    {
        Depth = 0
    }

    [CustomEffect("Allows rendering at user defined screen depths", "Rendering", "Adalberto Simeone")]
    [CustomEffectInput("Source")]
    public class StereoEffect : CustomEffectBase, DrawTransform
    {
        private static readonly Guid GUID_StereoVertexShader = Guid.NewGuid();
        private StereoEffectConstantBuffer constants;
        private DrawInformation drawInformation;
        private Rectangle inputRectangle;
        private int numberOfVertices;
        private float sizeX;
        private float sizeY;
        private VertexBuffer vertexBuffer;
        [PropertyBinding((int)StereoProperties.Depth, "1.0", "100.0", "1.0")]
        public float Depth { get; set; }
        public int InputCount
        {
            get { return 1; }
        }

        public override void Initialize(EffectContext effectContext, TransformGraph transformGraph)
        {
            byte[] vertexShaderBytecode = NativeFile.ReadAllBytes("Effects\\StereoVS.cso");
            effectContext.LoadVertexShader(GUID_StereoVertexShader, vertexShaderBytecode);

            // Only generate the vertex buffer if it has not already been initialized.
            vertexBuffer = effectContext.FindVertexBuffer(GUID_StereoVertexShader);

            if (vertexBuffer == null)
            {
                var mesh = GenerateMesh();

                // Updating geometry every time the effect is rendered can be costly, so it is 
                // recommended that vertex buffer remain static if possible (which it is in this
                // sample effect).
                using (var stream = DataStream.Create(mesh, true, true))
                {
                    var vbProp = new VertexBufferProperties(1, VertexUsage.Static, stream);

                    var cvbProp = new CustomVertexBufferProperties(vertexShaderBytecode, new[] {
                        new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32_Float,0,0),
                    }, Utilities.SizeOf<Vector2>());

                    // The GUID is optional, and is provided here to register the geometry globally.
                    // As mentioned above, this avoids duplication if multiple versions of the effect
                    // are created. 
                    vertexBuffer = new VertexBuffer(effectContext, GUID_StereoVertexShader, vbProp, cvbProp);
                }
            }

            PrepareForRender(ChangeType.Properties | ChangeType.Context);
            transformGraph.SetSingleTransformNode(this);
        }

        public SharpDX.Rectangle MapInputRectanglesToOutputRectangle(SharpDX.Rectangle[] inputRects, SharpDX.Rectangle[] inputOpaqueSubRects, out SharpDX.Rectangle outputOpaqueSubRect)
        {
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");

            // The output of the transform will be the same size as the input.
            Rectangle outputRect = inputRects[0];


            // Store the size of the rect so we can pass it into the vertex shader later.
            int newSizeX = inputRectangle.Right - inputRectangle.Left;
            int newSizeY = inputRectangle.Bottom - inputRectangle.Top;

            if (sizeX != newSizeX || sizeY != newSizeY)
            {
                sizeX = newSizeX;
                sizeY = newSizeY;

                UpdateConstants();
            }

            // Indicate that the image's opacity has not changed.
            outputOpaqueSubRect = inputOpaqueSubRects[0];
            // The size of the input image can be saved here for subsequent operations.
            inputRectangle = inputRects[0];

            return outputRect;
        }

        public SharpDX.Rectangle MapInvalidRect(int inputIndex, SharpDX.Rectangle invalidInputRect)
        {
            return invalidInputRect;
        }

        public void MapOutputRectangleToInputRectangles(SharpDX.Rectangle outputRect, SharpDX.Rectangle[] inputRects)
        {
            if (inputRects.Length != 1)
                throw new ArgumentException("InputRects must be length of 1", "inputRects");

            inputRects[0] = inputRectangle;
        }

        public override void PrepareForRender(ChangeType changeType)
        {
            UpdateConstants();
        }

        public void SetDrawInformation(DrawInformation drawInfo)
        {
            this.drawInformation = drawInfo;
            //drawInformation.SetVertexProcessing(vertexBuffer, VertexOptions.UseDepthBuffer,
            //    vertexRange: new VertexRange(0, 6),

            drawInformation.SetVertexProcessing(null, VertexOptions.None,
                vertexShader: GUID_StereoVertexShader);
        }

        /// <inheritdoc/>
        public override void SetGraph(TransformGraph transformGraph)
        {
            throw new NotImplementedException();
        }

        Vector2[] GenerateMesh()
        {
            numberOfVertices = 6;
            return new[] {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,1),
                new Vector2(1,0),
                new Vector2(0,0)
            };
        }
        void UpdateConstants()
        {
            constants.Depth = Depth;
            constants.SizeX = sizeX;
            constants.SizeY = sizeY;

            // Only update the constant buffer if the vertex buffer has been initialized.
            if (drawInformation != null)
            {
                drawInformation.SetVertexConstantBuffer(ref constants);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct StereoEffectConstantBuffer
        {
            public float Depth;
            public float SizeX;
            public float SizeY;
        }
    }
}