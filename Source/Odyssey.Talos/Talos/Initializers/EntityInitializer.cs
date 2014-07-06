using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Utilities.Logging;
using SharpDX;
using ITextureResource = Odyssey.Talos.Components.ITextureResource;

namespace Odyssey.Talos.Initializers
{
    public class EntityInitializer : Initializer<IEntity>
    {
        private readonly long kPosition;
        private readonly long kTransform;

        public EntityInitializer()
            : base(new[] {EngineReference.EntityMatrixWorld, EngineReference.EntityMatrixWorldInverse,
                EngineReference.EntityMatrixWorldInverseTranspose,
                EngineReference.EntityBloomThreshold,
                EngineReference.EntityBloomParameters,
                EngineReference.EntityBlurOffsetsWeights,
                EngineReference.EntityGlowStrength,
                EngineReference.EntitySpritePosition,
                EngineReference.EntitySpriteSize})
        {
            kPosition = ComponentTypeManager.GetKeyPart<PositionComponent>();
            kTransform = ComponentTypeManager.GetKeyPart<TransformComponent>();
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return true;
        }

        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var effect = initializer.Effect;
            
            var scene = services.GetService<IScene>();

            var data = from e in scene.Entities
                       let techniqueComponents = e.Components.OfType<ITechniqueComponent>()
                       from techniqueRange in techniqueComponents
                       from technique in techniqueRange.Techniques
                       where technique.ActiveTechniqueId == effect.Name
                       select e.Id;

            foreach (long entityId in data)
            {
                InitializerParameters parameters = new InitializerParameters(entityId, initializer.Technique, services, InstanceSelector);
                initializer.Initialize(this, scene.SelectEntity(entityId), parameters);
            }
        }

        public override void Initialize(DirectXDevice device, Effect effect, IEntity source, InitializerParameters parameters)
        {
            base.Initialize(device, effect, source, parameters);
            if (!effect.UsesProceduralTextures)
                InitializeTextures(effect, source, parameters);
        }

        void InitializeTextures(Effect effect, IEntity source, InitializerParameters parameters)
        {
            var referenceTable = from shaderObject in parameters.Technique.Shaders
                                 from textures in shaderObject.TextureReferences
                                 select new { Shader = shaderObject, TextureDesc = textures };

            foreach (var row in referenceTable)
            {
                var textureDesc = row.TextureDesc;
                if (!effect[textureDesc.ShaderType].HasTexture(textureDesc.Index))
                {
                    var texture = FindResource(source.Components, row.Shader.Name, row.TextureDesc.Texture);
                    if (texture == null)
                    {
                        LogEvent.Engine.Error("[{0}] is missing a component containing a [{1}] texture and tagged with [{2}].", source.Name,
                            textureDesc.Texture, row.Shader.Name);
                        throw new InvalidOperationException("No suitable textures found.");
                    }
                    effect[textureDesc.ShaderType].AddTexture(new TextureMapping(texture, textureDesc));
                }
            }
        }

        static Texture FindResource(IEnumerable<IComponent> components, string key, TextureReference type)
        {
            var enumerable = components as IComponent[] ?? components.ToArray();
            var resource  = enumerable.OfType<ITextureResource>().FirstOrDefault(c => c.AssetName == key);

            return resource != null ? resource[type] : null;
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IEntity entity, int parameterIndex, EngineReference reference, InitializerParameters initializerParameters)
        {
           return CreateParameter(entity, parameterIndex, reference, initializerParameters);
        }

        IEnumerable<IParameter> CreateParameter(IEntity entity, int parameterIndex, EngineReference reference, InitializerParameters initializerParameters)
        {
            IDirectXDeviceSettings deviceSettings = initializerParameters.Services.GetService<IDirectXDeviceSettings>();
            var cPosition = entity.GetComponent<PositionComponent>(kPosition);
            var cTransform = entity.GetComponent<TransformComponent>(kTransform);

            switch (reference)
            {
                case EngineReference.EntityMatrixWorld:
                    return new [] {new MatrixParameter(parameterIndex, Param.Matrices.World, () => cTransform.World)};

                case EngineReference.EntityMatrixWorldInverse:
                    return new[]
                    {
                        new MatrixParameter(parameterIndex, Param.Matrices.WorldInverse,
                            () => Matrix.Invert(cTransform.World))
                    };

                case EngineReference.EntityMatrixWorldInverseTranspose:
                    return new[]
                    {
                        new MatrixParameter(parameterIndex, Param.Matrices.WorldInverseTranspose,
                            () => Matrix.Transpose(Matrix.Invert(cTransform.World)))
                    };

                case EngineReference.EntityBlurOffsetsWeights:
                    bool isHorizontal = initializerParameters.Technique.Name == "GaussianBlurH";
                    float texelWidth;
                    float texelHeight;
                    var cBlur = entity.GetComponent<BlurComponent>();
                    if (isHorizontal)
                    {
                        texelWidth = 1.0f/ (deviceSettings.PreferredBackBufferWidth *cBlur.DownScale);
                        texelHeight = 0;
                    }
                    else
                    {
                        texelWidth = 0;
                        texelHeight = 1.0f / (deviceSettings.PreferredBackBufferHeight * cBlur.DownScale);
                    }
                    Vector2[] offsets;
                    float[] weights;
                    Blur.ComputeParameters(texelWidth, texelHeight, out weights, out offsets);
                    Vector4[] data = new Vector4[Blur.SampleCount];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = new Vector4(offsets[i].X, offsets[i].Y, weights[i], 0);
                    }
                    return new[] { new Float4ArrayParameter(parameterIndex, data.Length, Param.Floats.BlurOffsetsAndWeights, () => data)};
                    //return new IParameter[]
                    //{
                    //    new Float2ArrayParameter(parameterIndex, offsets.Length, Param.Floats.BlurOffsets, () => offsets),
                    //    new FloatArrayParameter(parameterIndex, offsets.Length, Param.Floats.BlurWeights, () => weights),
                    //};

                case EngineReference.EntityBloomThreshold:
                    return new[]
                    {
                        new FloatParameter(parameterIndex, Param.Floats.BloomThreshold,
                            () => entity.GetComponent<BloomComponent>().Threshold),
                    };

                case EngineReference.EntityBloomParameters:
                    var cBloom = entity.GetComponent<BloomComponent>();
                    return new []
                    {
                        
                        new Float4Parameter(parameterIndex, "BloomParameters",
                            () => new Vector4(cBloom.Intensity,cBloom.BaseIntensity,cBloom.Saturation, cBloom.BaseSaturation))
                    };

                case EngineReference.EntityGlowStrength:
                    return new[]
                    {
                        new FloatParameter(parameterIndex, Param.Floats.GlowStrength,
                            () => entity.GetComponent<GlowComponent>().GlowStrength),
                    };

                case EngineReference.EntitySpritePosition:
                    return new[]
                    {
                        new Float3Parameter(parameterIndex, Param.Vectors.SpritePosition,
                            () => new Vector3(entity.GetComponent<SpriteComponent>().Position, 1)),
                    };

                case EngineReference.EntitySpriteSize:
                    return new[]
                    {
                        new Float2Parameter(parameterIndex, Param.Vectors.SpriteSize,
                            () => entity.GetComponent<SpriteComponent>().Size)
                    };

                default:
                    throw new ArgumentException(string.Format("EngineReference: {0} not valid.", reference));
            }
        }
    }
}
