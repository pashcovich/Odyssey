using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Animation
{
    public interface IAnimatable
    {
        bool IsAnimatable(string propertyName);
    }
}
