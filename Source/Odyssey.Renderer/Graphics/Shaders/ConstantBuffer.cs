using System.Diagnostics;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Text.Logging;
using SharpDX.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Graphics.Shaders
{
    [DebuggerDisplay("[{description.ShaderType}] {description.UpdateFrequency} Slot:{description.Index}")]
    public class ConstantBuffer : Component, IEnumerable<IParameter>
    {
        private readonly DirectXDevice device;
        private readonly List<IParameter> parameters;
        private Buffer buffer;
        private readonly string technique;
        private readonly ConstantBufferDescription description;

        public bool IsInited { get; private set; }

        public int Index { get { return description.Index; } }
        public string Technique { get { return technique; } }

        public ConstantBufferDescription Description { get { return description; } }

        public int ElementSize { get; private set; }

        public Buffer Buffer { get { return buffer; } }

        private Vector4[] data;

        internal Vector4[] Data { get { return data; } }

        public ConstantBuffer(DirectXDevice device, ConstantBufferDescription description, string technique)
        {
            Name = description.Name;
            this.description = description;
            this.device = device;
            this.technique = technique;
            parameters = new List<IParameter>();
        }

        public void Clear()
        {
            description.ClearParsed();
            parameters.Clear();
        }

        public void AddParameter(int index, IParameter parameter)
        {
            parameters.Add(parameter);
        }

        public void AddParameterRange(int baseIndex, IEnumerable<IParameter> parameterCollection)
        {
            Contract.Requires<ArgumentNullException>(parameterCollection != null, "parameterCollection");
            foreach (IParameter parameter in parameterCollection)
            {
                parameters.Add(parameter);
            }
        }

        private void CheckPacking()
        {
            var tempParameters = new List<IParameter>(parameters);
            var newParameters = new List<IParameter>();

            var pack = new List<IParameter>();

            int bytes=0;
            foreach (var parameter in tempParameters)
            {
                if (parameter.Size == 16)
                    newParameters.Add(parameter);
                else if ((parameter.Size + bytes) > 16)
                {
                    bytes = 0;
                    newParameters.AddRange(pack);
                    newParameters.Add(parameter);
                    pack.Clear();
                }
                else if ((parameter.Size + bytes) < 16)
                {
                    bytes += parameter.Size;
                    pack.Add(parameter);
                }
                else
                {
                    pack.Add(parameter);
                    newParameters.Add(Combine(pack));
                    bytes = 0;
                    pack.Clear();
                }
            }
            if (pack.Count > 0)
                newParameters.AddRange(pack);

            parameters.Clear();
            parameters.AddRange(newParameters);
        }

        public void Assemble()
        {
            if (IsInited)
                LogEvent.Engine.Warning("Attempted to reinitialize [{0}]", Name);

            if (parameters.Count == 0)
                LogEvent.Engine.Warning("No parameters found while initializing [{0}]", Name);

            RemoveAndDispose(ref buffer);
            CheckPacking();
            parameters.Sort((p1, p2) => p1.Index.CompareTo(p2.Index));
            data = parameters.Select(p => p.ToArray()).Aggregate((i, j) => i.Concat(j).ToArray());
            ElementSize = Vector4.SizeInBytes * data.Length;
            buffer = ToDispose(Buffer.Constant.New(device, ElementSize));
            buffer.SetData(data);
            buffer.DebugName = Name;

            IsInited = true;
        }

        public void Update()
        {
            data = parameters.Select(p => p.ToArray()).Aggregate((i, j) => i.Concat(j).ToArray());
            buffer.SetData(data);
        }

        IEnumerator<IParameter> IEnumerable<IParameter>.GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IParameter>)this).GetEnumerator();
        }

        static Float4Parameter Combine(IEnumerable<IParameter> parameters)
        {
            var paramArray = parameters.ToArray();
            // case 1: 4x FloatParameters
            if (paramArray.All(p => p.Size == 4))
            {
                FloatParameter p0 = (FloatParameter)paramArray[0];
                FloatParameter p1 = (FloatParameter)paramArray[1];
                FloatParameter p2 = (FloatParameter)paramArray[2];
                FloatParameter p3 = (FloatParameter)paramArray[3];

                LogEvent.Engine.Info("Packed [{0} {1} {2} {3}] into a Float4Parameter", p0.ParamHandle, p1.ParamHandle, p2.ParamHandle, p3.ParamHandle);

                return new Float4Parameter(p0.Index, p0.ParamHandle,
                    () => new Vector4(p0.Method(), p1.Method(), p2.Method(), p3.Method()));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}