using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public static class Mapper
    {
        public static string Map(Type type)
        {
            return HLSLTypes.Map(type);
        }

        public static string Map(CustomType type)
        {
            return HLSLTypes.Map(type);
        }

        public static string Map(Semantic semantic)
        {
            return HLSLSemantics.Map(semantic);
        }
    }
}
