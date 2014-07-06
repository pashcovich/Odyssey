using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Interaction
{
    public interface IInputController
    {
        
        void Add();
        void Remove();
        string Name { get; }
    }

    public interface IObjectController : IInputController
    {
        IInteractive3DModel Mesh { get; set; }
    }




}
