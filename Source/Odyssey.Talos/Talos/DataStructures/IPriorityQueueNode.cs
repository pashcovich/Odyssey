using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Talos.DataStructures
{
    public interface IPriorityQueueNode
    {
        double Priority { get; set;}
        long InsertionIndex { get; set; }
        int QueueIndex { get; set; }
    }
}
