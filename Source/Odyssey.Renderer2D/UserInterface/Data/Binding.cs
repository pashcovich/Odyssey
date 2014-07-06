using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Data
{
    public class Binding
    {
        public Binding(string path, string targetElement)
        {
            Path = path;
            TargetElement = targetElement;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }

        public string Path { get; set; }

        public object Source { get; set; }

        public string TargetElement { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
    }
}