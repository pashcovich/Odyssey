﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    public class BinaryFunctionNode : UnaryFunctionNode
    {
        [DataMember]
        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input2 { get; set; }

        public override string Access()
        {
            return Function.Call(Input1.Reference, Input2.Reference);
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode("Input2", Input2);
        }
    }
}