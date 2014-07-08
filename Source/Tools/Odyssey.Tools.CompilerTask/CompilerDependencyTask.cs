using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Odyssey.Tools.Compiler;

namespace Odyssey.Tools.CompilerTask
{
    public class CompilerDependencyTask : Microsoft.Build.Utilities.Task
    {
        protected class OdItem
        {
            public string Name { get; set; }
            public string LinkName { get; set; }
            public string InputFilePath { get; set; }
            public string InputFilename { get; set; }
            public string InputExtension { get; set; }
            public string OutputLink { get; set; }
            public string OutputFilePath { get; set; }
            public string OutputExtension { get; set; }

            public TaskItem ToTaskItem()
            {
                var item = new TaskItem(OutputFilePath);

                item.SetMetadata("CopyToOutputDirectory", "PreserveNewest");
                item.SetMetadata("Link", OutputLink);
                return item;
            }

        }

       
        [Required]
        public ITaskItem ProjectDirectory { get; set; }
        [Required]
        public ITaskItem[] Files { get; set; }
        [Required]
        public ITaskItem IntermediateDirectory { get; set; }
        [Output]
        public ITaskItem[] ContentFiles { get; set; }

        public bool Debug { get; set; }
        public string RootNamespace { get; set; }

        public override bool Execute()
        {
            bool hasErrors = false;
            try
            {
                var contentFiles = new List<ITaskItem>();
                foreach (ITaskItem item in Files)
                {
                    OdItem odItem = new OdItem
                    {
                        Name = item.ItemSpec,
                        InputFilename = item.GetMetadata("Filename"),
                        InputExtension = item.GetMetadata("Extension"),
                        LinkName = item.GetMetadata("Link", item.ItemSpec),
                    };

                    odItem.OutputExtension = FindExtension(odItem.InputExtension);
                    odItem.OutputLink = Path.ChangeExtension(odItem.LinkName, odItem.OutputExtension);
                    odItem.OutputFilePath = Path.Combine(ProjectDirectory.ItemSpec, IntermediateDirectory.ItemSpec,
                        odItem.OutputLink);
                    odItem.InputFilePath = Path.Combine(ProjectDirectory.ItemSpec, odItem.Name);

                    if (!ProcessItem(odItem))
                        hasErrors = true;

                    var returnedItem = odItem.ToTaskItem();

                    contentFiles.Add(returnedItem);
                }
                ContentFiles = contentFiles.ToArray();
            }
            catch (Exception ex)
            {
                Log.LogError("Unexpected exception: {0}", ex);
                hasErrors = true;
            }
            return !hasErrors;
        }

        protected virtual bool ProcessItem(OdItem odItem)
        {
            return true;
        }

        protected void CreateDirectoryIfNotExists(string filePath)
        {
            var dependencyDirectoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dependencyDirectoryPath))
                Directory.CreateDirectory(dependencyDirectoryPath);
        }

        public static string FindExtension(string inputExtension)
        {
            string extension = inputExtension.ToLowerInvariant();
            switch (extension)
            {
                case ".dae":
                case ".fbx":
                    return ".omd";
            }

            return ".unknown";
        }
    }
}
