using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics.Models;

namespace Odyssey.Tools.Compiler
{
    /// <summary>
    /// Result of a compilation.
    /// </summary>
    public sealed class ContentCompilerResult
    {
        public bool IsContentGenerated { get; set; }

        public ModelData ModelData { get;set; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; internal set; }
    }

}