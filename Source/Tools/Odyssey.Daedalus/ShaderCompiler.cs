using Odyssey.Daedalus.Model;
using Odyssey.Daedalus.Properties;
using Odyssey.Daedalus.ViewModel;
using Odyssey.Graphics.Effects;
using SharpDX;
using SharpDX.D3DCompiler;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SharpDX.IO;
using ShaderDescription = Odyssey.Graphics.Shaders.ShaderDescription;

namespace Odyssey.Daedalus
{
    public class ShaderCompiler
    {
        List<ErrorModel> errorList;

        public IEnumerable<ErrorModel> CompilationErrors { get { return errorList; } }

        public ShaderCompiler()
        {}

        CompilationResult Compile(IShaderViewModel shader, out List<ErrorModel> errors)
        {
            CompilationResult result = null;
            errorList = new List<ErrorModel>();
            try
            {
                IncludeHandler includeHandler = new IncludeHandler();
                result = ShaderBytecode.Compile(shader.SourceCode, shader.Name, shader.FeatureLevel.ToString().ToLowerInvariant(),
                    FromShaderConfiguration(), EffectFlags.None, null, includeHandler);
                includeHandler.Dispose();
                
                // In case compilation information could be needed - i.e. number of instructions
                //using (ShaderReflection sReflection = new ShaderReflection(result.Bytecode))
                //    Log.Daedalus.Info(sReflection.ToString());

                if (!string.IsNullOrEmpty(result.Message))
                {
                    string[] warnings = result.Message.Split('\n');
                    foreach (string warning in warnings)
                        if (!string.IsNullOrEmpty(warning))
                            errorList.Add(new ErrorModel(shader.Name, warning));
                    errors = errorList;
                }
                else
                    errors = null;
                return result;
            }
            catch (InvalidOperationException exIO)
            {
                Log.Daedalus.Error(exIO.ToString());
                errorList.Add(new ErrorModel(shader.Name, exIO.Message, false));
                errors = errorList;
                return null;
            }
            catch (CompilationException exComp)
            {
                Log.Daedalus.Error(exComp.ToString());
                string[] errorMessages = exComp.Message.Split('\n');
                foreach (string error in errorMessages)
                {
                    if (!string.IsNullOrEmpty(error))
                        errorList.Add(new ErrorModel(shader.Name, error));
                }
                errors = errorList;
                return null;
            }
        }

        public bool CompileShader(string techniqueName, ShaderDescriptionViewModel vmShader, out ShaderDescription shaderObject, out IEnumerable<ErrorViewModel> errors)
        {
            List<ErrorModel> errorList;
            CompilationResult compilationResult = Compile(vmShader, out errorList);
            bool result = compilationResult != null && !compilationResult.HasErrors;
            vmShader.CompilationStatus = result ? CompilationStatus.Successful : CompilationStatus.Failed;
            if (!result)
            {
                errors = errorList.Select(e => new ErrorViewModel { ErrorModel = e });
                shaderObject = null;
            }
            else
            {
                var references = vmShader.ShaderDescriptionModel.Shader.References;
                var textureReferences = vmShader.ShaderDescriptionModel.Shader.TextureReferences;
                var samplerReferences = vmShader.ShaderDescriptionModel.Shader.SamplerReferences;
                shaderObject = new ShaderDescription(vmShader.Name, vmShader.Type, vmShader.FeatureLevel,
                    compilationResult.Bytecode, references, textureReferences, samplerReferences);
                errors = null;
            }
            return result;
        }

        public bool CompileShader(ShaderCodeViewModel vmShader, out IEnumerable<ErrorViewModel> errors)
        {
            List<ErrorModel> errorList;
            CompilationResult compilationResult = Compile(vmShader, out errorList);
            bool result = compilationResult != null && !compilationResult.HasErrors;
            vmShader.CompilationStatus = result ? CompilationStatus.Successful : CompilationStatus.Failed;
            if (!result)
                errors = errorList.Select(e => new ErrorViewModel { ErrorModel = e });
            else
            {
                string path = Path.Combine(Settings.Default.OutputPath, $"{vmShader.Name}.fxo");
                var stream = new NativeFileStream(path, NativeFileMode.Create, NativeFileAccess.Write);
                compilationResult.Bytecode.Save(stream);
                errors = null;
            }

            return result;
        }

        static ShaderFlags FromShaderConfiguration()
        {
            ShaderConfiguration configuration = Settings.Default.DefaultShaderConfiguration;
            switch (configuration)
            {
                case ShaderConfiguration.Debug:
                    return ShaderFlags.Debug;

                default:
                case ShaderConfiguration.Release:
                    return ShaderFlags.None;
            }
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
