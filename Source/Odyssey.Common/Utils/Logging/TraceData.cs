using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Diagnostics.Contracts;

namespace Odyssey.Utils.Logging
{
    public struct TraceData
    {
        public Type Type { get; private set; }
        public MethodInfo Method { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public Dictionary<string, string> AdditionalData;

        public TraceData(Type type, MethodInfo method, params KeyValuePair<string, string>[] additionalData)
            : this()
        {
            Type = type;
            Method = method;
            Parameters = method.GetParameters();
            AdditionalData = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in additionalData)
                AdditionalData.Add(kvp.Key, kvp.Value);
        }

        public TraceData(Type type, string method, params KeyValuePair<string, string>[] additionalData) 
            : this(type, type.GetTypeInfo().GetDeclaredMethod(method), additionalData)
        {
        }

        public void AddData(string key, string value)
        {
            AdditionalData.Add(key, value);
        }


        public string MethodSignature 
        {
            get
            {
                string arguments = string.Empty;
                for (int i = 0; i < Parameters.Length; i++)
                {
                    arguments += string.Format("{0} {1}", Parameters[i].ParameterType.Name, Parameters[i].Name);
                    if (i < Parameters.Length-1)
                        arguments += ", ";
                }

                return string.Format("{0}.{1}({2})", Type.Name, Method.Name, arguments);
            }
        }

        public string GetValue(string key)
        {
            Contract.Ensures(AdditionalData.ContainsKey(key));
            return AdditionalData[key];
        }
    }
}
