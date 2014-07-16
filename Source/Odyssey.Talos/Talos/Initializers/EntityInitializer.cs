using System.Runtime.CompilerServices;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
using Odyssey.Utilities.Logging;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using ITextureResource = Odyssey.Talos.Components.ITextureResource;
using EngineReference = Odyssey.Graphics.Effects.EngineReference;
namespace Odyssey.Talos.Initializers
{
    public class EntityInitializer : Initializer<IEntity>
    {
        public EntityInitializer(IServiceRegistry services)
            : base(services, Reference.Group.Entity)
        {
        } 

        public override void Initialize(DirectXDevice device, Effect effect, IEntity source, InitializerParameters parameters)
        {
            base.Initialize(device, effect, source, parameters);
            if (!effect.UsesProceduralTextures)
                InitializeTextures(effect, source, parameters);
        }

        public override void SetupInitialization(ShaderInitializer initializer)
        {
            var services = initializer.Services;
            var effect = initializer.Effect;

            var scene = services.GetService<IScene>();

            var data = from e in scene.Entities
                        let techniqueComponents = e.Components.OfType<ITechniqueComponent>()
                        from cTechnique in techniqueComponents
                        from technique in cTechnique.Techniques
                        where technique.ActiveTechniqueId == effect.Name
                        select e.Id;

            foreach (long entityId in data)
            {
                InitializerParameters parameters = new InitializerParameters(entityId, initializer.Technique, services, InstanceSelector);
                initializer.Initialize(this, scene.SelectEntity(entityId), parameters);
            }
        }

        protected override IEnumerable<IParameter> CreateParameter(ConstantBufferDescription cbParent, IEntity entity, int parameterIndex, string reference, InitializerParameters initializerParameters)
        {
            if (!ReferenceActions.ContainsKey(reference))
                throw new InvalidOperationException(string.Format("[{0}]: Entity parameter not valid", reference));

            return ReferenceActions[reference](parameterIndex, entity, initializerParameters);
        }

        protected override bool ValidateConstantBuffer(ConstantBufferDescription cb)
        {
            return true;
        }

        /// <summary>
        /// Searches through components looking for a <see cref="Texture"/> resource associated to
        /// a <see cref="Shader"/> asset having the specified reference.
        /// </summary>
        /// <param name="components">A sequence of <see cref="Components.Component"/> belonging to an <see cref="Entity"/>.</param>
        /// <param name="key">The key value identifying the <see cref="Shader"/> asset.</param>
        /// <param name="reference">The reference value identifying the <see cref="Texture"/>.</param>
        /// <returns></returns>
        private static Texture FindResource(IEnumerable<IComponent> components, string key, string reference)
        {
            var enumerable = components as IComponent[] ?? components.ToArray();
            var resource = enumerable.OfType<ITextureResource>().FirstOrDefault(c => c.AssetName == key);

            return resource != null ? resource[reference] : null;
        }

        private static readonly Dictionary<string, ParameterMethod> ReferenceActions = new Dictionary<string, ParameterMethod>
        {
            {
                Reference.Matrix.World, (index, entity, parameters) => new[]
                {new MatrixParameter(index, Param.Matrices.World, () => entity.GetComponent<TransformComponent>().World)}
            },
            {
                Reference.Matrix.WorldInverse, (index, entity, parameters) => new[]
                {
                    new MatrixParameter(index, Param.Matrices.WorldInverse,
                        () => Matrix.Invert(entity.GetComponent<TransformComponent>().World))
                }
            },
            {
                Reference.Matrix.WorldInverseTranspose, (index, entity, parameters) => new[]
                {
                    new MatrixParameter(index, Param.Matrices.WorldInverseTranspose,
                        () => Matrix.Transpose(Matrix.Invert(entity.GetComponent<TransformComponent>().World)))
                }
            },
        };


        private void InitializeTextures(Effect effect, IEntity source, InitializerParameters parameters)
        {
            var referenceTable = from shaderObject in parameters.Technique.Shaders
                                 from textures in shaderObject.TextureReferences
                                 select new { Shader = shaderObject, TextureDesc = textures };

            foreach (var row in referenceTable)
            {
                var textureDesc = row.TextureDesc;
                if (!effect[textureDesc.ShaderType].HasTexture(textureDesc.Index))
                {
                    Texture texture = FindResource(source.Components, row.Shader.Name, row.TextureDesc.Texture);
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
    }
}