﻿using Odyssey.Graphics.Shaders;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Operators;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Math
{
    public class InvertNode : NodeBase
    {
        private IVariable output;

        [SupportedType(Type.Vector)]
        public INode Input { get; set; }

        [SupportedType(Type.Vector)]
        public override IVariable Output
        {
            get
            {
                if (output == null)
                {
                    string name1 = Input.Output.Name.Substring(1, Input.Output.Name.Length - 1);

                    Type type = Input.Output.Type;
                    Output = new Vector
                    {
                        Name = string.Format("{0}{1}Inv", Variable.GetPrefix(type), name1),
                        Type = type
                    };
                }
                return output;
            }
            set { output = value; }
        }

        public override string Access()
        {
            return string.Format(Input.GetType() == typeof(ReferenceNode) ? "-{0}" : "-({0})", Input.Reference);
        }

        protected override void RegisterNodes()
        {
            Nodes.Add("Input", Input);
        }
    }
}