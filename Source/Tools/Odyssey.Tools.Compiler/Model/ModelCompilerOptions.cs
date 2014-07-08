using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Models;

namespace Odyssey.Tools.Compiler.Model
{
    public enum ModelRealTimeQuality
    {
        Low,

        Default,

        Maximum,
    }



    public class ModelCompilerOptions
    {
        public ModelRealTimeQuality Quality { get; set; }
        public string[] ExcludeElements { get; set; }
        public string DependencyFile { get; set; }
        public ModelOperation ModelOperations { get; set; }
    }
}
