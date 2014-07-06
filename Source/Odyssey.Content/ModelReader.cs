using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Geometry;
using Odyssey.Graphics.Meshes;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Toolkit.Content;

namespace Odyssey.Content
{
    [SupportedType(typeof(TkModel))]
    public class ModelReader : Component,IContentReader
    {

        public static TkModel ConvertToOdysseyModel(string assetName, SharpDX.Toolkit.Graphics.Model model)
        {
            Contract.Requires<InvalidOperationException>(model.Meshes.Count >= 1);

            List<TkMeshPart> meshParts = new List<TkMeshPart>();
            
            foreach (var mesh in model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    VertexDescription vertexDescription = ConvertVertexElements(meshPart.VertexBuffer.Resource.Layout.BufferLayouts[0].VertexElements);
                    TkMeshPart tkMeshPart = new TkMeshPart(name: meshPart.Name,
                        vertexFormat: vertexDescription.Format,
                        vertexSize: vertexDescription.Stride,
                        indexFormat: meshPart.IndexBuffer.Resource.ElementSize == 2 ? Format.R16_UInt : Format.R32_UInt,
                        vertexBuffer: meshPart.VertexBuffer.Resource.Buffer,
                        indexBuffer: meshPart.IndexBuffer.Resource);
                    meshParts.Add(tkMeshPart);
                }
            }

            return new TkModel(model.Name, meshParts);
        }

        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            var service = assetManager.Services.GetService<SharpDX.Toolkit.Graphics.IGraphicsDeviceService>();
            SharpDX.Toolkit.Graphics.ModelReader modelReader = new SharpDX.Toolkit.Graphics.ModelReader(service.GraphicsDevice, parameters.Stream,
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
            var odyssseyModel = ConvertToOdysseyModel(parameters.AssetName, model);
            model.Dispose();
            modelReader.Dispose();

            return odyssseyModel; 
        }


        public static VertexDescription ConvertVertexElements(ReadOnlyArray<VertexElement> vertexElements)
        {
            VertexFormat vertexFormat = VertexFormat.Unknown;
            int vertexSize = 0;
            foreach (var ve in vertexElements)
            {
                string semantic = (string)ve.SemanticName;
                if (string.Equals(semantic, "SV_POSITION"))
                {
                    vertexFormat |= VertexFormat.Position;
                    vertexSize += 12;
                }
                else if (semantic.StartsWith("NORMAL"))
                {
                    vertexFormat |= VertexFormat.Normal;
                    vertexSize += 12;
                }
                else if (semantic.StartsWith("TEXCOORD"))
                {
                    vertexFormat |= VertexFormat.TextureUV;
                    vertexSize += 8;
                }
                else if (semantic.StartsWith("TANGENT"))
                {
                    vertexFormat |= VertexFormat.Tangent;
                    vertexSize += 12;
                }
                else if (semantic.StartsWith("BITANGENT"))
                {
                    vertexFormat |= VertexFormat.Bitangent;
                    vertexSize += 12;
                }
            }
            return new VertexDescription(vertexFormat, vertexSize);
        }
    }
}
