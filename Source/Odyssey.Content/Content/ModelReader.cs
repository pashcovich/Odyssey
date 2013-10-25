using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using Odyssey.Graphics.Meshes;
using Odyssey.Geometry;
using System.Diagnostics.Contracts;
using SharpDX.Direct3D11;
using SharpDX;
using Odyssey.Engine;
using SharpDX.DXGI;


namespace Odyssey.Content
{
    [SupportedType(typeof(Mesh))]
    public class ModelReader : Component,IResourceReader, IDeviceDependentComponent
    {
        static IDirect3DProvider direct3D;
        static GraphicsDevice graphicsDevice;


        public static Mesh ConvertToOdysseyModel(SharpDX.Toolkit.Graphics.Model model)
        {
            Contract.Requires(model.Meshes.Count == 1);
            //Contract.Requires(model.Meshes[0].MeshParts.Count == 1);

            VertexFormat vertexFormat = VertexFormat.Unknown;
            var modelVB = model.Meshes[0].MeshParts[0].VertexBuffer;
            foreach (var ve in modelVB.Resource.Layout.BufferLayouts[0].VertexElements)
            {
                string semantic = (string)ve.SemanticName;
                if (string.Equals(semantic, "SV_POSITION"))
                        vertexFormat |= VertexFormat.Position;
                else if (semantic.StartsWith("NORMAL"))
                    vertexFormat |= VertexFormat.Normal;
                else if (semantic.StartsWith("TEXCOORD"))
                    vertexFormat |= VertexFormat.TextureUV;
                else if (semantic.StartsWith("TANGENT"))
                    vertexFormat |= VertexFormat.Tangent;
                else if (semantic.StartsWith("BITANGENT"))
                    vertexFormat |= VertexFormat.Bitangent;
            }
            var modelIB = model.Meshes[0].MeshParts[0].IndexBuffer;
            Format indexFormat = modelIB.Resource.ElementSize == 2 ? Format.R16_UInt : Format.R32_UInt;

            return new Content.Mesh(
                vertexFormat: vertexFormat,
                indexFormat: indexFormat,
                vertexBuffer: modelVB.Resource.Buffer,
                indexBuffer: modelIB.Resource
                );
        }

        public object ReadContent(string resourceName, Stream stream)
        {
            SharpDX.Toolkit.Graphics.ModelReader modelReader = new SharpDX.Toolkit.Graphics.ModelReader(graphicsDevice, stream,
                 name =>
                 {
                     try
                     {
                         //var assetPath = Path.GetDirectoryName(resourceName);
                         //var texturePath = Path.Combine(assetPath ?? string.Empty, name);
                         //return Global.Content.Load<Texture>(texturePath);
                         return null;
                     }
                     catch (AssetNotFoundException ex)
                     {
                         // TODO: Because some models could have some textures that are not found, the model should load, even if the
                         // Texture was not found, but we define what to do in this case. Log error?
                     }
                     return null;
                 });
            var model = modelReader.ReadModel();
            var odyssseyModel = ConvertToOdysseyModel(model);
            model.Dispose();
            modelReader.Dispose();

            return odyssseyModel; 
        }

        public void Initialize(InitializeDirectXEventArgs e)
        {
            direct3D = e.DirectX.Direct3D;
            graphicsDevice = ToDispose(GraphicsDevice.New(direct3D.Device));
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);
            GraphicsAdapter.Dispose();
        } 
    }
}
