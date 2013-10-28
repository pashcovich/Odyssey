using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.ViewModel.Messages
{
    public enum TechniqueAction
    {
        None,
        Add,
        Remove,
        Update,
        Unassign
    }

    public class TechniqueMessage
    {
        public TechniqueAction Action { get; private set; }
        public TechniqueMappingViewModel Technique { get; private set;}

        public TechniqueMessage(TechniqueAction action, TechniqueMappingViewModel vmTechnique)
        {
            Action = action;
            Technique = vmTechnique;
        }
    }
}
