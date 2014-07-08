using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Odyssey.Graphics.Meshes;
using Odyssey.Graphics.Models;
using Odyssey.Tools.Compiler.Model;
using SharpDX;

namespace Odyssey.Tools.CompilerTask
{
    public class ModelConverterTask : ContentCompilerTask
    {

        public ITaskItem[] ExcludeElements { get; set; }
        public ITaskItem[] ModelOperations { get; set; }

        protected override bool ProcessFile(string inputFilePath, string outputFilePath, string dependencyFilePath, OdItem item)
        {
            ModelOperation op = ModelOperation.None;
            
            if (ModelOperations != null)
                op = ModelOperations.Aggregate(ModelOperation.None, (current, t) => current | (ModelOperation) Enum.Parse(typeof (ModelOperation), t.ItemSpec));

            var compilerOptions = new ModelCompilerOptions()
            {
                DependencyFile = dependencyFilePath,
                Quality = Debug ? ModelRealTimeQuality.Low : ModelRealTimeQuality.Maximum,
                ExcludeElements = ExcludeElements != null ? ExcludeElements.Select(element => element.ItemSpec).ToArray() : new string[0],
                ModelOperations = op
            };

            var result = ModelCompiler.CompileAndSave(inputFilePath, outputFilePath, compilerOptions);

            return result.HasErrors;
        }

        

    }
}
