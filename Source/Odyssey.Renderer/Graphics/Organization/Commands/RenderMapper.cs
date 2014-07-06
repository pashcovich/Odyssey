using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Graphics.Models;
using Odyssey.Graphics.Shaders;
using Odyssey.Talos;

namespace Odyssey.Graphics.Organization.Commands
{
    public class RenderMapper
    {
        private readonly Dictionary<Effect, Dictionary<Model, List<IEntity>>> effectMap;

        public IEnumerable<KeyValuePair<Effect, Dictionary<Model, List<IEntity>>>> EffectModelsPairs
        {
            get { return effectMap; }
        }

        public RenderMapper()
        {
            effectMap = new Dictionary<Effect, Dictionary<Model, List<IEntity>>>();
        }

        public void Clear()
        {
            effectMap.Clear();
        }

        public bool ContainsKey(string effectId)
        {
            return FindBy(effectId) != null;
        }

        public bool ContainsModel(Effect effect, Model model)
        {
            return effectMap[effect].ContainsKey(model);
        }

        public bool ContainsKey(Effect effect)
        {
            return effectMap.ContainsKey(effect);
        }

        public void AddMesh(Effect effect, Model model, IEntity entity)
        {
            var meshIds = effectMap[effect];
            meshIds[model].Add(entity);
        }

        private Effect FindBy(string name)
        {
            return effectMap.Keys.FirstOrDefault(effect => effect.Name == name);
        }

        public void DefineNewEffect(Effect effect)
        {
            Contract.Requires<NullReferenceException>(effect != null);
            effectMap.Add(effect, new Dictionary<Model, List<IEntity>>());
        }

        public void AssociateModel(Effect effect, Model model)
        {
            effectMap[effect].Add(model, new List<IEntity>()); ;
        }

        public IEnumerable<IEntity> this[string name]
        {
            get
            {
                Effect effect = FindBy(name);
                if (effect == null)
                    throw new InvalidOperationException("Effect not found.");

                return effectMap[effect].SelectMany(e => e.Value);
            }
        }

    }
}