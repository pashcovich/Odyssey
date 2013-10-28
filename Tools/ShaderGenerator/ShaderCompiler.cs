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
using Odyssey.Tools.ShaderGenerator.Properties;

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

        bool Compile(IShaderViewModel shader, out CompilationResult result)
        {
            result = null;
            try
            {
                IncludeHandler includeHandler = new IncludeHandler();
                result = ShaderBytecode.Compile(shader.SourceCode, shader.Name, shader.FeatureLevel.ToString().ToLowerInvariant(),
                    ShaderFlags.Debug, EffectFlags.None, null, includeHandler);
                includeHandler.Dispose();
                
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

        public bool CompileShader(string techniqueName, ShaderDescriptionViewModel vmShader,out ShaderObject shaderObject, out IEnumerable<ErrorViewModel> errors)
        {
            List<ErrorViewModel> errorList = new List<ErrorViewModel>();

            CompilationResult compilationResult;
            errors = null;
            bool result = Compile(vmShader, out compilationResult);
            vmShader.CompilationStatus = result == true ? CompilationStatus.Successful : CompilationStatus.Failed;
            if (!result)
            {
                foreach (ErrorModel error in CompilationErrors)
                    errorList.Add(new ErrorViewModel { ErrorModel = error });
                errors = errorList;
                shaderObject = null;
            }
            else
            {
                
                var references = vmShader.ShaderDescriptionModel.Shader.References;
                var textureReferences = vmShader.ShaderDescriptionModel.Shader.TextureReferences;
                var samplerReferences = vmShader.ShaderDescriptionModel.Shader.SamplerReferences;
                shaderObject = new ShaderObject(vmShader.Name, vmShader.Type, vmShader.FeatureLevel,
                    compilationResult.Bytecode, references, textureReferences, samplerReferences);
            }

            return result;
        }

        public bool CompileShader(ShaderCodeViewModel vmShader, out IEnumerable<ErrorViewModel> errors)
        {
            List<ErrorViewModel> errorList = new List<ErrorViewModel>();
            
            CompilationResult compilationResult;
            errors = null;
            bool result = Compile(vmShader, out compilationResult);
            vmShader.CompilationStatus = result == true ? CompilationStatus.Successful : CompilationStatus.Failed;
            if (!result)
            {
                foreach (ErrorModel error in CompilationErrors)
                    errorList.Add(new ErrorViewModel { ErrorModel = error });
                errors = errorList;
            }
            else
                compilationResult.Bytecode.Save(Path.Combine(Settings.Default.OutputPath,vmShader.Name));

            return result;
        }

    }

    public class IncludeHandler : CallbackBase ,Include
    {
        internal static string BaseDirectory {get; set;}

        public void Close(Stream stream)
        {
            stream.Close();
        }


        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            return new FileStream(Path.Combine(BaseDirectory, fileName), FileMode.Open, FileAccess.Read);
        }

       
    }
}
