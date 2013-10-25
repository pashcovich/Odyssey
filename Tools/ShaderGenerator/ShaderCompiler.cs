using Odyssey.Content.Shaders;
using Odyssey.Tools.ShaderGenerator.Model;
using Odyssey.Tools.ShaderGenerator.Shaders;
using Odyssey.Tools.ShaderGenerator.ViewModel;
using Odyssey.Utils.Logging;
using SharpDX;
using SharpDX.D3DCompiler;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

namespace Odyssey.Tools.ShaderGenerator
{
    public class ShaderCompiler
    {
        List<ErrorModel> errorList;

        public IEnumerable<ErrorModel> CompilationErrors { get { return errorList; } }

        public ShaderCompiler()
        {
            errorList = new List<ErrorModel>();
        }

        public bool Compile(ShaderDescriptionViewModel shader, out CompilationResult result)
        {
            result = null;
            try
            {
                string source = shader.SourceCode;
                result = ShaderBytecode.Compile(source, shader.Name, shader.FeatureLevel.ToString().ToLowerInvariant(), ShaderFlags.Debug, EffectFlags.None);
                
                // In case compilation information could be needed - i.e. number of instructions
                //using (ShaderReflection sReflection = new ShaderReflection(result.Bytecode))
                //    LogEvent.Tool.Info(sReflection.ToString());

                return true;
            }
            catch (InvalidOperationException exIO)
            {
                LogEvent.Tool.Error(exIO.ToString());
                errorList.Add(new ErrorModel(shader.Name, exIO.Message, false));
                return false;
            }
            catch (CompilationException exComp)
            {
                LogEvent.Tool.Error(exComp.ToString());
                string[] errors = exComp.Message.Split('\n');
                foreach (string error in errors)
                {
                    if (!string.IsNullOrEmpty(error))
                        errorList.Add(new ErrorModel(shader.Name, error));
                }
                return false;
            }
        }

        
    }
}
