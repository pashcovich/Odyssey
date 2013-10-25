using Odyssey.Content.Shaders;
using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using Odyssey.Utils;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    [DataContract(IsReference = true)]
    [KnownType("KnownTypes")]
    public class Shader
    {
        string sourceCode;
        int cbCount;
        int customTypeCount;
        int samplerCount;
        int textureCount;
        [DataMember]
        Dictionary<string, IVariable> variables;
        Dictionary<int, ConstantBufferDescription> cbReferences;
        Dictionary<int, TextureDescription> textureReferences;
        Dictionary<int, Odyssey.Content.Shaders.SamplerStateDescription> samplerReferences;

        public Shader()
        {
            variables = new Dictionary<string, IVariable>();
        }


        public string SourceCode
        {
            get
            {
                if (string.IsNullOrEmpty(sourceCode))
                    Build();
                return sourceCode;
            }
        }

        public IEnumerable<Struct> ConstantBuffers
        {
            get
            {
                return (from kvp in variables
                        where kvp.Value.Type == Shaders.Type.ConstantBuffer
                        select kvp.Value).Cast<Struct>();
            }
        }

        public IEnumerable<Struct> CustomTypes
        {
            get
            {
                return (from kvp in variables
                        where kvp.Value.Type == Shaders.Type.Struct
                        let varStruct = (Struct)kvp.Value
                        where varStruct.CustomType != CustomType.None
                        select varStruct);
            }
        }

        [DataMember]
        public bool EnableSeparators { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public FeatureLevel FeatureLevel { get; set; }
        [DataMember]
        public IStruct InputStruct { get; set; }
        [DataMember]
        public IStruct OutputStruct { get; protected set; }
        [DataMember]
        public INode Result { get; protected set; }
        [DataMember]
        public ShaderType Type { get; set; }

        public IEnumerable<Sampler> Samplers
        {
            get
            {
                return variables.Values.OfType<Sampler>();
            }
        }

        public IEnumerable<ConstantBufferDescription> References { get { return cbReferences.Values; } }

        public string Signature
        {
            get
            {
                Contract.Requires<InvalidOperationException>(InputStruct.CustomType != CustomType.None);
                return string.Format("{0} {1}({2} {3}) {4}", OutputStruct.CustomType, Name, InputStruct.CustomType, InputStruct.Name,
                    Type == ShaderType.Pixel ? ": " + Param.SemanticVariables.SVTarget : string.Empty);
            }
        }

        public string Disclaimer
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("// {0}", Name));
                sb.AppendLine(string.Format("// {0} Shader ({1})", Type, FeatureLevel.ToString().ToLowerInvariant()));
                sb.AppendLine("//");
                sb.AppendLine(string.Format("// Built with Odyssey Shader Generator v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString()));
                sb.AppendLine(string.Format("// {0:dd/MM/yyyy H:mm:ss }", DateTime.Now));

                return sb.ToString();
            }
        }

        public IEnumerable<Variable> Textures
        {
            get
            {
                return (from kvp in variables
                        where kvp.Value.Type == Shaders.Type.Texture2D
                        select kvp.Value).Cast<Variable>();
            }
        }

        public IEnumerable<TextureDescription> TextureReferences { get { return textureReferences.Values; } }
        public IEnumerable<Odyssey.Content.Shaders.SamplerStateDescription> SamplerReferences { get { return samplerReferences.Values; } }

        public void Add(IVariable variable)
        {
            Contract.Requires<ArgumentException>(variable != null);

            IStruct varStruct = variable as IStruct;
            switch (variable.Type)
            {
                case Shaders.Type.ConstantBuffer:
                    variable.Index = cbCount++;
                    break;

                case Shaders.Type.Texture2D:
                    variable.Index = textureCount++;
                    break;

                case Shaders.Type.Sampler:
                case Shaders.Type.SamplerComparisonState:
                    variable.Index = samplerCount++;
                    break;
            }
                
            variables.Add(Variable.GetRegister(variable), variable);

            if (varStruct == null)
                return;

            var childStructs = from vS in varStruct.Variables.OfType<IStruct>()
                               where vS.CustomType != CustomType.None
                               select vS;

            foreach (IStruct childStruct in childStructs)
            {
                customTypeCount++;
                Add(childStruct);
            }

        }

        public void Add(IEnumerable<IVariable> variables)
        {
            foreach (IVariable variable in variables)
                Add(variable);
        }

        public void Remove(IVariable variable)
        {
            Contract.Requires<ArgumentException>(Contains(variable));
            variables.Remove(Variable.GetRegister(variable));
        }

        public void Clear()
        {
            variables.Clear();
            cbCount = 0;
            customTypeCount = 0;
            textureCount = 0;
            samplerCount = 0;
        }

        void CollectReferences()
        {
            cbReferences = new Dictionary<int, ConstantBufferDescription>();
            textureReferences = new Dictionary<int, TextureDescription>();
            samplerReferences = new Dictionary<int, Content.Shaders.SamplerStateDescription>();

            foreach (ConstantBuffer cb in ConstantBuffers)
            {
                ConstantBufferDescription cbReference = new ConstantBufferDescription(cb.Index, cb.UpdateFrequency, cb.References);
                cbReferences.Add(cbReference.Index, cbReference);
            }
            foreach (var texture in Textures)
            {
                TextureReference reference =  (TextureReference)texture.ShaderReference.Value;
                string key = texture.ContainsMarkup(Texture.Key) ? texture.GetMarkupValue<string>(Texture.Key) : "Empty";
                int samplerIndex = texture.GetMarkupValue<int>(Texture.SamplerIndex);
                UpdateFrequency updateFrequency = texture.ContainsMarkup(Texture.UpdateFrequency) ? 
                    texture.GetMarkupValue<UpdateFrequency>(Texture.UpdateFrequency)
                    : UpdateFrequency.Static;
                TextureDescription tDescription = new TextureDescription(texture.Index, key, reference, samplerIndex, updateFrequency);
                textureReferences.Add(tDescription.Index, tDescription);
            }
            foreach (var sampler in Samplers)
            {
                Filter filter = sampler.GetMarkupValue<Filter>(Sampler.Filter);
                TextureAddressMode tAddressMode = sampler.GetMarkupValue<TextureAddressMode>(Sampler.TextureAddressMode);
                Comparison comparison = sampler.GetMarkupValue<Comparison>(Sampler.Comparison);
                Odyssey.Content.Shaders.SamplerStateDescription samplerDesc = new Content.Shaders.SamplerStateDescription
                {
                    Index = sampler.Index,
                    Comparison = comparison,
                    Filter = filter,
                    TextureAddressMode = tAddressMode,
                };
                samplerReferences.Add(samplerDesc.Index, samplerDesc);
            }
        }

        public void Build()
        {
           
            ShaderBuilder sb = new ShaderBuilder(GenerateKeyPartRequirements(this));
            IEnumerable<IMethod> requiredMethods;
            sb.BuildMethod(Signature, Result, out requiredMethods);
            CollectReferences();

            sb.Add(Disclaimer);
            if (EnableSeparators)
                sb.AddSeparator("Input/Output Structs");
            sb.Add(InputStruct.Definition);
            sb.Add(OutputStruct.Definition);

            if (EnableSeparators && customTypeCount > 0)
                sb.AddSeparator("Custom Struct Definition");

            foreach (IVariable customType in CustomTypes)
                sb.Add(customType.Definition);

            if (EnableSeparators && cbCount > 0) 
                sb.AddSeparator("ConstantBuffers");

            foreach (IVariable variable in ConstantBuffers)
                sb.Add(variable.Definition);

            if (EnableSeparators && textureCount > 0)
                sb.AddSeparator("Textures");

            foreach (IVariable texture in Textures)
                sb.Add(texture.Definition);

            if (EnableSeparators && samplerCount > 0)
                sb.AddSeparator("Samplers");

            foreach (IVariable sampler in Samplers)
                sb.Add(sampler.Definition);

            

            if (EnableSeparators && requiredMethods.Count()>0)
                sb.AddSeparator("Required methods");

            foreach (IMethod method in requiredMethods)
                sb.Add(method.Definition);

            if (EnableSeparators)
                sb.AddSeparator(string.Format("{0} Shader", Type));

            sb.Add(sb.EntryPoint);

            sourceCode = sb.Output;
        }

        public bool Contains(IVariable variable)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variable.Name));
            return variables.ContainsKey(Variable.GetRegister(variable));
        }

        public override string ToString()
        {
            return SourceCode;
        }

        internal static System.Type[] KnownTypes()
        {
            return ReflectionHelper.GetDerivedTypes(typeof(Shader)).Concat(ReflectionHelper.GetDerivedTypes(typeof(Variable))).ToArray();
        }

        internal static ShaderModel FromFeatureLevel(FeatureLevel featureLevel)
        {
            switch (featureLevel)
            {
                case FeatureLevel.VS_4_0_Level_9_1:
                case FeatureLevel.PS_4_0_Level_9_1:
                    return ShaderModel.SM20_level_9_1;
                case FeatureLevel.VS_4_0_Level_9_3:
                case FeatureLevel.PS_4_0_Level_9_3:
                    return ShaderModel.SM20_level_9_3;

                default:
                    return ShaderModel.SM40;
            }
        }

        public static TechniqueKey GenerateKeyPartRequirements(Shader shader)
        {
            var vsAttributes = shader.GetType().GetCustomAttributes<VertexShaderAttribute>();
            var psAttributes = shader.GetType().GetCustomAttributes<PixelShaderAttribute>();
            VertexShaderFlags vsFlags = VertexShaderFlags.None;
            PixelShaderFlags psFlags = PixelShaderFlags.None;
            ShaderModel model = FromFeatureLevel(shader.FeatureLevel);

            foreach (var vsAttribute in vsAttributes)
                vsFlags |= vsAttribute.Features;
            foreach (var psAttribute in psAttributes)
                psFlags |= psAttribute.Features;

            return new TechniqueKey(vsFlags, psFlags, sm: model);
        }


        public static TechniqueKey GenerateKeyPart(Shader shader)
        {
            
            VertexShaderFlags vsFlags = VertexShaderFlags.None;
            PixelShaderFlags psFlags = PixelShaderFlags.None;
            ShaderModel model = FromFeatureLevel(shader.FeatureLevel);
            
            foreach(INode node in shader.Result.DescendantNodes)
            {
                var properties = node.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(INode));
                foreach (var property in properties)
                {
                    INode variable = (INode)property.GetValue(node);

                    switch (shader.Type)
                    {
                        case ShaderType.Vertex:
                            foreach (VertexShaderAttribute attribute in property.GetCustomAttributes(true).OfType<VertexShaderAttribute>())
                            {
                                VertexShaderAttribute vsAttribute = (VertexShaderAttribute)attribute;
                                vsFlags |= vsAttribute.Features;
                            }
                            break;
                        case ShaderType.Pixel:
                            foreach (object attribute in property.GetCustomAttributes(true).OfType<PixelShaderAttribute>())
                            {
                                PixelShaderAttribute psAttribute = (PixelShaderAttribute)attribute;
                                psFlags |= psAttribute.Features;
                            }
                            break;
                    }
                }
            }

            return new TechniqueKey(vsFlags, psFlags, sm: model);
        }

    }
}
