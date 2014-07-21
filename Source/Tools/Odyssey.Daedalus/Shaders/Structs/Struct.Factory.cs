using Odyssey.Daedalus.Data;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders.Structs
{
    public partial class Struct
    {

        public static Struct VertexPositionTexture
        {
            get
            {
                Struct vpt = new Struct
                {
                    Name = "input",
                    CustomType = Shaders.CustomType.VSIn,
                };
                vpt.Add(Vector.ObjectPosition);
                vpt.Add(Vector.TextureUV);
                return vpt;
            }
        }
        public static Struct VertexPositionNormalTexture
        {
            get
            {
                Struct vpnt = new Struct
                {
                    Name = "input",
                    CustomType = Shaders.CustomType.VSIn,
                };
                vpnt.Add(Vector.ObjectPosition);
                vpnt.Add(Vector.Normal);
                vpnt.Add(Vector.TextureUV);
                return vpnt;
            }
        }

        

        public static Struct VertexPositionNormalTextureTangent
        {
            get
            {
                Struct vpntt = new Struct
                {
                    Name = "input",
                    CustomType = Shaders.CustomType.VSIn,
                };
                vpntt.Add(Vector.ObjectPosition);
                vpntt.Add(Vector.Normal);
                vpntt.Add(Vector.TextureUV);
                vpntt.Add(Vector.Tangent);
                return vpntt;
            }
        }

        public static Struct VertexPositionNormalTextureUVW
        {
            get
            {
                Struct vpt = new Struct
                {
                    Name = "input",
                    CustomType = Shaders.CustomType.VSIn,
                };
                vpt.Add(Vector.ObjectPosition);
                vpt.Add(Vector.Normal);
                vpt.Add(Vector.TextureUVW);
                return vpt;
            }
        }

        public static Struct VertexPositionColor 
        {
            get
            {
                Struct vpc = new Struct
                {
                    Name = "input",
                    CustomType = Shaders.CustomType.VSIn
                };
                vpc.Add(Vector.ObjectPosition);
                vpc.Add(Vector.Color);
                return vpc;
            }
        }

        public static Struct VertexPositionNormalTextureOut
        {
            get
            {
                Struct vpt = new Struct
                {
                    CustomType = Shaders.CustomType.VSOut,
                    Name = "output",
                };
                vpt.Add(Vector.ClipPosition);
                vpt.Add(Vector.WorldPosition4);
                vpt.Add(Vector.Normal);
                vpt.Add(Vector.TextureUV);
                return vpt;
            }
        }

        

        public static Struct VertexPositionNormalTextureTangentOut
        {
            get
            {
                Struct vpntt = new Struct
                {
                    CustomType = Shaders.CustomType.VSOut,
                    Name = "output",
                };
                vpntt.Add(Vector.ClipPosition);
                vpntt.Add(Vector.WorldPosition4);
                vpntt.Add(Vector.Normal);
                vpntt.Add(Vector.TextureUV);
                vpntt.Add(Vector.Tangent);
                
                return vpntt;
            }
        }

        public static Struct PixelShaderOutput
        {
            get
            {
                Struct psOut = new Struct
                {
                    CustomType = Shaders.CustomType.PSOut,
                    Name = "output"
                };
                psOut.Add(Vector.ColorPSOut);
                return psOut;
            }
        }

        public static Struct Material
        {
            get
            {
                Struct material = new Struct
                {
                    CustomType = Shaders.CustomType.Material,
                    Name = "material",
                    EngineReference = ReferenceFactory.Material.StructMaterialPS
                };

                material.Add(new Vector { Name = Param.Material.kA, Type = Type.Float });
                material.Add(new Vector { Name = Param.Material.kD, Type = Type.Float });
                material.Add(new Vector { Name = Param.Material.kS, Type = Type.Float });
                material.Add(new Vector { Name = Param.Material.SpecularPower, Type = Type.Float });
                material.Add(new Vector { Name = Param.Material.Ambient, Type = Type.Float4 });
                material.Add(new Vector { Name = Param.Material.Diffuse, Type = Type.Float4 });
                material.Add(new Vector { Name = Param.Material.Specular, Type = Type.Float4 });
                material.SetMarkup(Param.Properties.Material, "Phong");
                return material;
            }
        }


        public static Struct PointLight
        {
            get
            {
                Struct lPoint = new Struct
                {
                    CustomType = Shaders.CustomType.PointLight,
                    Name = "lPoint",
                    EngineReference = ReferenceFactory.Light.StructPointPS
                };

                lPoint.Add(new Vector { Name = Param.Light.Intensity, Type = Type.Float });
                lPoint.Add(new Vector { Name = Param.Light.Radius, Type = Type.Float });
                lPoint.Add(new Vector { Name = Param.Light.Diffuse, Type = Type.Float4 });
                lPoint.Add(new Vector { Name = Param.Light.Position, Type = Type.Float3 });
                //lPoint.Add(new Vector { Name = Param.Light.Direction, Type = Type.Float4 });

                lPoint.SetMarkup(Param.Properties.LightId, 0);

                return lPoint;
            }
        }

        public static Struct PointLightVS
        {
            get
            {
                Struct lPoint = new Struct
                {
                    CustomType = Shaders.CustomType.PointLight,
                    Name = "lPoint",
                    EngineReference = ReferenceFactory.Light.StructPointVS
                };
                lPoint.Add(Matrix.LightView);
                lPoint.Add(Matrix.LightProjection);
                return lPoint;
            }
        }
    }
}
