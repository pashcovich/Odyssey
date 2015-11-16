#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Odyssey.Daedalus.Serialization;
using Odyssey.Daedalus.Shaders.Methods;
using Odyssey.Daedalus.Shaders.Nodes;
using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using Odyssey.Text.Logging;
using Odyssey.Utilities;
using SharpDX.Direct3D11;
using Odyssey.Serialization;
using SamplerStateDescription = Odyssey.Graphics.Shaders.SamplerStateDescription;

#endregion

namespace Odyssey.Daedalus.Shaders
{
    [DataContract(IsReference = true)]
    public class Shader : IDataSerializable, IContainer
    {
        private int cbCount;
        private Dictionary<int, ConstantBufferDescription> cbReferences;
        private int constantsCount;
        private int customTypeCount;
        private bool enableSeparators;
        private FeatureLevel featureLevel;
        private IStruct inputStruct;
        private TechniqueKey keyPart;
        private Dictionary<string, string> metaData;
        private string name;
        private IStruct outputStruct;
        private INode result;
        private int samplerCount;
        private Dictionary<int, SamplerStateDescription> samplerReferences;
        private string sourceCode;
        private int textureCount;
        private Dictionary<int, TextureDescription> textureReferences;
        private ShaderType type;
        private VariableCollection variables;

        public Shader()
        {
            NodeBase.NodeCounter.Clear();
            variables = new VariableCollection() {Owner = this};
        }

        #region Properties

        public string SourceCode
        {
            get
            {
                if (string.IsNullOrEmpty(sourceCode))
                    Build();
                return sourceCode;
            }
        }

        public IEnumerable<Structs.ConstantBuffer> ConstantBuffers
        {
            get
            {
                return (from kvp in variables
                    where kvp.Value.Type == Shaders.Type.ConstantBuffer
                    select kvp.Value).Cast<Structs.ConstantBuffer>();
            }
        }

        public IEnumerable<Struct> CustomTypes
        {
            get
            {
                return (from kvp in variables
                    where kvp.Value.Type == Shaders.Type.Struct
                    let varStruct = (Struct) kvp.Value
                    where varStruct.CustomType != CustomType.None
                    select varStruct);
            }
        }

        public IEnumerable<Variable> MiscVariables
        {
            get
            {
                Type[] valueTypes =
                {
                    Shaders.Type.Float, Shaders.Type.Float2, Shaders.Type.Float3, Shaders.Type.Float4,
                    Shaders.Type.Matrix, Shaders.Type.Float3x3, Shaders.Type.Float4x4, Shaders.Type.FloatArray
                };
                return from kvp in variables
                    where valueTypes.Contains(kvp.Value.Type)
                    select (Variable) kvp.Value;
                ;
            }
        }

        public IEnumerable<IVariable> Variables
        {
            get { return variables.Values; }
        }

        [DataMember]
        public bool EnableSeparators
        {
            get { return enableSeparators; }
            set { enableSeparators = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public FeatureLevel FeatureLevel
        {
            get { return featureLevel; }
            set { featureLevel = value; }
        }

        [DataMember]
        public IStruct InputStruct
        {
            get { return inputStruct; }
            set { inputStruct = value; }
        }

        [DataMember]
        public IStruct OutputStruct
        {
            get { return outputStruct; }
            set { outputStruct = value; }
        }

        [DataMember]
        public INode Result
        {
            get { return result; }
            set { result = value; }
        }

        [DataMember]
        public ShaderType Type
        {
            get { return type; }
            set { type = value; }
        }

        [DataMember]
        public TechniqueKey KeyPart
        {
            get { return keyPart; }
            set { keyPart = value; }
        }

        public IEnumerable<Sampler> Samplers
        {
            get { return variables.Values.OfType<Sampler>(); }
        }

        public IEnumerable<ConstantBufferDescription> References
        {
            get { return cbReferences.Values; }
        }

        public string Signature
        {
            get
            {
                Contract.Requires<InvalidOperationException>(InputStruct.CustomType != CustomType.None);
                return string.Format("{0} {1}({2} {3}) {4}", OutputStruct.CustomType, Name, InputStruct.CustomType,
                    InputStruct.Name,
                    Type == ShaderType.Pixel ? ": " + Param.SemanticVariables.SVTarget : string.Empty);
            }
        }

        public string Disclaimer
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Format("// {0}", Name));
                sb.AppendLine(string.Format("// {0} Shader ({1})", Type, FeatureLevel.ToString().ToLowerInvariant()));
                sb.AppendLine("//");
                sb.AppendLine(string.Format("// Built with Odyssey Shader Generator v{0}",
                    Assembly.GetExecutingAssembly().GetName().Version));
                sb.AppendLine(string.Format("// {0:dd/MM/yyyy H:mm:ss }", DateTime.Now));

                return sb.ToString();
            }
        }

        public IEnumerable<Variable> Textures
        {
            get
            {
                return (from kvp in variables
                    where kvp.Value.Type == Shaders.Type.Texture2D ||
                          kvp.Value.Type == Shaders.Type.Texture3D ||
                          kvp.Value.Type == Shaders.Type.TextureCube
                    select kvp.Value).Cast<Variable>();
            }
        }

        public IEnumerable<TextureDescription> TextureReferences
        {
            get { return textureReferences.Values; }
        }

        public IEnumerable<SamplerStateDescription> SamplerReferences
        {
            get { return samplerReferences.Values; }
        }

        #endregion

        public void Add(IVariable variable)
        {
            Contract.Requires<ArgumentException>(variable != null);

            var varStruct = variable as IStruct;
            switch (variable.Type)
            {
                case Shaders.Type.ConstantBuffer:
                    variable.Index = cbCount++;
                    break;

                case Shaders.Type.Texture2D:
                case Shaders.Type.Texture3D:
                case Shaders.Type.TextureCube:
                    variable.Index = textureCount++;
                    break;

                case Shaders.Type.Sampler:
                case Shaders.Type.SamplerComparisonState:
                    variable.Index = samplerCount++;
                    break;

                case Shaders.Type.Struct:
                    variable.Index = customTypeCount++;
                    break;

                case Shaders.Type.Matrix:
                case Shaders.Type.Vector:
                case Shaders.Type.FloatArray:
                case Shaders.Type.Float:
                case Shaders.Type.Float2:
                case Shaders.Type.Float3:
                case Shaders.Type.Float4:
                    variable.Index = constantsCount++;
                    break;
            }

            if (variables.All(kvp => kvp.Value.Name != variable.Name))
                variables.Add(Variable.GetRegister(variable), variable);

            if (varStruct == null)
                return;

            var childStructs = from vS in varStruct.Variables.OfType<IStruct>()
                where vS.CustomType != CustomType.None
                select vS;

            foreach (var childStruct in childStructs)
            {
                customTypeCount++;
                Add(childStruct);
            }
        }

        public void Serialize(BinarySerializer serializer)
        {
            var sgs = (ShaderGraphSerializer) serializer;
            sgs.Clear();

            serializer.BeginChunk("SHAD");

            serializer.BeginChunk("PROP");
            serializer.Serialize(ref name);
            serializer.Serialize(ref keyPart);
            serializer.Serialize(ref enableSeparators);
            serializer.SerializeEnum(ref featureLevel);
            serializer.SerializeEnum(ref type);
            serializer.EndChunk();

            variables.Serialize(serializer);

            var sInput = (Struct) inputStruct;
            var sOutput = (Struct) outputStruct;
            serializer.BeginChunk("STRT");
            serializer.Serialize(ref sInput);
            serializer.Serialize(ref sOutput);
            serializer.EndChunk();

            serializer.BeginChunk("RSLT");

            if (serializer.Mode == SerializerMode.Write)
                NodeBase.WriteNode(serializer, Result);
            else
                Result = NodeBase.ReadNode(serializer);
            serializer.EndChunk();

            serializer.EndChunk();

            if (serializer.Mode == SerializerMode.Read)
            {
                InputStruct = sInput;
                OutputStruct = sOutput;
            }
        }

        public void Add(IEnumerable<IVariable> newVariables)
        {
            foreach (var variable in newVariables)
                Add(variable);
        }

        public TVariable Get<TVariable>(string name)
            where TVariable : IVariable
        {
            Contract.Requires<ArgumentException>(Contains(name), "name");
            return (TVariable) variables.First(kvp => string.Equals(kvp.Value.Name, name)).Value;
        }

        public void Remove(IVariable variable)
        {
            Contract.Requires<ArgumentException>(Contains(variable));
            variables.Remove(Variable.GetRegister(variable));
        }

        public void Remove(string variableName)
        {
            Contract.Requires<ArgumentException>(Contains(variableName));
            variables.Remove(variables.First(kvp => kvp.Value.Name == variableName).Key);
        }

        public void Clear()
        {
            variables.Clear();
            cbCount = 0;
            customTypeCount = 0;
            textureCount = 0;
            samplerCount = 0;
        }

        private void CollectReferences()
        {
            cbReferences = new Dictionary<int, ConstantBufferDescription>();
            textureReferences = new Dictionary<int, TextureDescription>();
            samplerReferences = new Dictionary<int, SamplerStateDescription>();
            metaData = new Dictionary<string, string>();

            // Collect Instance references
            var instanceReferences = (from v in InputStruct.Variables
                where v.EngineReference != null
                group v.EngineReference by v.GetMarkupValue(Param.Properties.InstanceSlot)
                into instanceBuffers
                select new {Slot = Int32.Parse(instanceBuffers.Key), References = instanceBuffers}).ToArray();

            foreach (var instanceBuffer in instanceReferences)
            {
                cbReferences.Add(cbCount++,
                    new ConstantBufferDescription("InstanceCB", instanceBuffer.Slot, CBUpdateType.InstanceFrame, ShaderType.Vertex,
                        instanceBuffer.References, Enumerable.Empty<KeyValuePair<string, string>>()));
            }

            // Collect ConstantBuffer references
            foreach (var cb in ConstantBuffers)
            {
                var markupData = from v in cb.Variables
                    from markup in v.Markup
                    where v.HasMarkup
                    select markup;

                foreach (var kvp in markupData)
                {
                    // Check whether we defined multiple instances of the same metadata property 
                    if (!metaData.ContainsKey(kvp.Key))
                        metaData.Add(kvp.Key, kvp.Value);
                    else if (metaData[kvp.Key] != kvp.Value)
                        LogEvent.Tool.Error("Conflicting metadata: [{0}] and [{1}]", kvp.Value, metaData[kvp.Key]);
                }

                var cbReference = new ConstantBufferDescription(cb.Name, cb.Index.Value, cb.CbUpdateType, Type,
                    cb.References, metaData);
                cbReferences.Add(cbCount++, cbReference);
                metaData.Clear();
            }

            // Collect Texture references
            foreach (var texture in Textures.Where(t => t.EngineReference != null))
            {
                var reference = texture.EngineReference.Value;
                var key = texture.ContainsMarkup(Texture.Key)
                    ? texture.GetMarkupValue(Texture.Key)
                    : string.Format("{0}.{1}", Name, reference);
                var cbUpdateType = CBUpdateType.None;
                var samplerIndex = 0;
                if (texture.HasMarkup)
                {
                    // Write metadata indicating preferred metadata
                    samplerIndex = int.Parse(texture.GetMarkupValue(Texture.SamplerIndex));
                    cbUpdateType = texture.ContainsMarkup(Texture.UpdateType)
                        ? ReflectionHelper.ParseEnum<CBUpdateType>(texture.GetMarkupValue(Texture.UpdateType))
                        : CBUpdateType.SceneStatic;
                }
                var tDescription = new TextureDescription(texture.Index.Value, key, reference, samplerIndex, cbUpdateType,
                    Type);
                textureReferences.Add(tDescription.Index, tDescription);
            }

            // Collect sampler references
            foreach (var sampler in Samplers)
            {
                var name = sampler.Name;
                var filter = ReflectionHelper.ParseEnum<Filter>(sampler.GetMarkupValue(Sampler.Filter));
                var tAddressMode =
                    ReflectionHelper.ParseEnum<TextureAddressMode>(sampler.GetMarkupValue(Sampler.TextureAddressMode));
                var comparison = ReflectionHelper.ParseEnum<Comparison>(sampler.GetMarkupValue(Sampler.Comparison));
                var samplerDesc = new SamplerStateDescription
                {
                    Index = sampler.Index.Value,
                    Name = name,
                    Comparison = comparison,
                    Filter = filter,
                    TextureAddressMode = tAddressMode,
                };
                samplerReferences.Add(samplerDesc.Index, samplerDesc);
            }
        }

        public void Build()
        {
            var sb = new ShaderBuilder(KeyPart);
            IEnumerable<IMethod> requiredMethods;
            sb.BuildMethod(Signature, Result, out requiredMethods);
            CollectReferences();

            sb.Add(Disclaimer);
            if (EnableSeparators)
                sb.AddSeparator("Input/Output Structs");
            sb.Add(InputStruct.Definition);
            sb.Add(OutputStruct.Definition);

            if (EnableSeparators && constantsCount > 0)
                sb.AddSeparator("Constants");

            foreach (IVariable variable in MiscVariables)
                sb.Add(variable.Definition);

            if (EnableSeparators && customTypeCount > 0)
                sb.AddSeparator("Custom Struct Definitions");

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

            var methods = requiredMethods as IMethod[] ?? requiredMethods.ToArray();

            if (EnableSeparators && methods.Length > 0)
                sb.AddSeparator("Required methods");

            foreach (var method in methods)
                sb.Add(method.Definition);

            if (EnableSeparators)
                sb.AddSeparator($"{Type} Shader");

            sb.Add(sb.EntryPoint);

            sourceCode = sb.Output;
        }

        [Pure]
        public bool Contains(IVariable variable)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variable.Name));
            return variables.ContainsKey(Variable.GetRegister(variable));
        }

        [Pure]
        public bool Contains(string variableName)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(variableName));
            return variables.Any(kvp => kvp.Value.Name == variableName);
        }

        public override string ToString()
        {
            return SourceCode;
        }

        internal static System.Type[] KnownTypes()
        {
            return ReflectionHelper.GetDerivedTypes(typeof (Shader)).Concat(ReflectionHelper.GetDerivedTypes(typeof (Variable))).ToArray();
        }

        internal static ShaderModel FromFeatureLevel(FeatureLevel featureLevel)
        {
            var featureLevelValue = featureLevel.ToString();

            var shaderModelValue = "SM" + featureLevelValue.Substring(2, featureLevelValue.Length - 2);
            return (ShaderModel) Enum.Parse(typeof (ShaderModel), shaderModelValue);
        }

        internal static FeatureLevel FromShaderModel(ShaderModel model, ShaderType type)
        {
            var shaderCode = string.Empty;
            switch (type)
            {
                case ShaderType.Pixel:
                    shaderCode = "PS";
                    if (model == ShaderModel.Any)
                        return FeatureLevel.PS_2_0;
                    break;

                case ShaderType.Vertex:
                    shaderCode = "VS";
                    if (model == ShaderModel.Any)
                        return FeatureLevel.VS_2_0;
                    break;
            }
            
            var shaderModelValue = model.ToString();
            var featureLevelValue = shaderCode + shaderModelValue.Substring(2, shaderModelValue.Length - 2);
            if (type == ShaderType.Vertex && model == ShaderModel.SM_2_B)
                featureLevelValue = featureLevelValue.Replace('B', 'A');
            return (FeatureLevel) Enum.Parse(typeof (FeatureLevel), featureLevelValue);
        }

        internal static TechniqueKey GenerateKeyPartRequirements(Shader shader)
        {
            var vsAttributes = shader.GetType().GetCustomAttributes<VertexShaderAttribute>();
            var psAttributes = shader.GetType().GetCustomAttributes<PixelShaderAttribute>();

            var vsFlags = VertexShaderFlags.None;
            var psFlags = PixelShaderFlags.None;
            var model = FromFeatureLevel(shader.FeatureLevel);

            foreach (var vsAttribute in vsAttributes)
                vsFlags |= vsAttribute.Features;
            foreach (var psAttribute in psAttributes)
                psFlags |= psAttribute.Features;


            return new TechniqueKey(vsFlags, psFlags, sm: model);
        }


        public static TechniqueKey GenerateKeyPart(Shader shader)
        {
            var vsFlags = VertexShaderFlags.None;
            var psFlags = PixelShaderFlags.None;
            var model = FromFeatureLevel(shader.FeatureLevel);

            foreach (var property in shader.Result.DescendantNodes
                .Select(node => node.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof (INode))).SelectMany(properties => properties))
            {
                switch (shader.Type)
                {
                    case ShaderType.Vertex:
                        vsFlags = property.GetCustomAttributes(true)
                            .OfType<VertexShaderAttribute>()
                            .Aggregate(vsFlags, (current, vsAttribute) => current | vsAttribute.Features);
                        break;
                    case ShaderType.Pixel:
                        psFlags = property.GetCustomAttributes(true).OfType<PixelShaderAttribute>()
                            .Aggregate(psFlags, (current, psAttribute) => current | psAttribute.Features);
                        break;
                }
            }

            return new TechniqueKey(vsFlags, psFlags, sm: model);
        }
    }
}