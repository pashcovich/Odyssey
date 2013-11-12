using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Utils;
using ShaderGenerator.Data;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    public class YamlVariable
    {
        [YamlMember(0)]
        [DefaultValue(null)]
        public string name { get; set; }
        
        [YamlMember(1)]
        public Type type { get; set; }
        
        [YamlMember(2)]
        [DefaultValue(Semantic.None)]
        public Semantic semantic { get; set; }
        
        [YamlMember(3)]
        [DefaultValue(0)]
        public int? index { get; set; }
        
        [YamlMember(5)]
        [DefaultValue(null)]
        public string comments { get; set; }
        
        [YamlMember(6)]
        [DefaultValue(null)]
        public Dictionary<string, string> markup { get; set; }

        [YamlMember(7)]
        [DefaultValue(null)]
        public string reference { get; set; }

        [YamlIgnore]
        public YamlStruct owner { get; set; }

        public string fullName
        {
            get { 
                
                return owner != null ? string.Format("{0}.{1}", owner.name, name) : name; }
        }

        public YamlVariable()
        { }

        public YamlVariable(IVariable variable)
        {
            name = variable.Name;
            type = variable.Type;
            semantic = variable.Semantic;
            index = variable.Index;
            comments = variable.Comments;
            markup = variable.Markup.Any() ? variable.Markup.ToDictionary(kvp=> kvp.Key, kvp=> kvp.Value.ToString()) : null;
            reference = variable.ShaderReference != null ? string.Format("{0}.{1}", variable.ShaderReference.Type, variable.ShaderReference.Value) : null;
        }

        public virtual IVariable ToVariable()
        {
            return null;
        }

        protected static TVariable ConvertYamlVariable<TVariable>(YamlVariable variable)
            where TVariable : Variable, new()
        {

            TVariable v = new TVariable
            {
                Name = variable.name,
                Type = variable.type,
                Semantic = variable.semantic,
                Index = variable.index,
                Comments = variable.comments,
            };

            if (variable.reference != null)
            {
                string[] reference = variable.reference.Split('.');

                switch (reference[0])
                {
                    case "Engine":
                        v.ShaderReference = new ShaderReference((EngineReference)Enum.Parse(typeof(EngineReference), reference[1]));
                        break;

                    case "Texture":
                        v.ShaderReference = new ShaderReference((TextureReference)Enum.Parse(typeof(TextureReference), reference[1]));
                        break;
                }
            }

            if (variable.markup != null)
                v.SetMarkup(variable.markup);
            return v;
        }


    }
}
