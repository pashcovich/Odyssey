using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Odyssey.Tools.Compiler;

namespace Odyssey.Tools.CompilerTask
{
    public abstract class ContentCompilerTask : CompilerDependencyTask
    {
        protected override bool ProcessItem(OdItem odItem)
        {
            var hasErrors = false;

            var inputFilePath = odItem.InputFilePath;
            var outputFilePath = odItem.OutputFilePath;

            try
            {
                var dependencyFilePath =
                    Path.Combine(Path.Combine(ProjectDirectory.ItemSpec, IntermediateDirectory.ItemSpec),
                        FileDependencyList.GetDependencyFileNameFromSourcePath(odItem.LinkName));

                CreateDirectoryIfNotExists(dependencyFilePath);

                Log.LogMessage(MessageImportance.Low, "Check Toolkit file to compile {0} with dependency file {1}", inputFilePath, dependencyFilePath);
                if (FileDependencyList.CheckForChanges(dependencyFilePath) || !File.Exists(outputFilePath))
                {
                    Log.LogMessage(MessageImportance.Low, "Starting compilation of {0}", inputFilePath);

                    if (!ProcessFile(inputFilePath, outputFilePath, dependencyFilePath, odItem))
                        hasErrors = false;
                    else
                    {
                        Log.LogMessage(MessageImportance.High, "Compilation successful of {0} to {1}", inputFilePath, outputFilePath);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.LogError("Cannot process file '{0}' : {1}", inputFilePath, ex.Message);
                hasErrors = true;
            }

            return !hasErrors;
        }

        protected abstract bool ProcessFile(string inputFilePath, string outputFilePath, string dependencyFilePath, OdItem item);
    }
}
