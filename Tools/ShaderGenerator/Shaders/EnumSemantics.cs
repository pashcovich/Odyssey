using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public enum Semantic
    {
        Null,
        Position,
        Color,
        Normal,
        SV_Position,
        SV_Target,
        Texcoord,
        ViewDirection
    }

    internal static class HLSLSemantics
    {
        internal const string Position = "POSITION";
        internal const string Color = "COLOR";
        internal const string Normal = "NORMAL";
        internal const string Texcoord = "TEXCOORD";
        internal const string SV_Position = "SV_POSITION";
        internal const string SV_Target = "SV_Target";

        internal const string ViewDirection = "VIEWDIRECTION";


        static Dictionary<Semantic, string> semantics;
 
        static HLSLSemantics()
        {
            semantics = new Dictionary<Semantic, string>();
            semantics.Add(Semantic.Position, Position);
            semantics.Add(Semantic.Color, Color);
            semantics.Add(Semantic.Texcoord, Texcoord);
            semantics.Add(Semantic.Normal, Normal);
            semantics.Add(Semantic.SV_Position, SV_Position);
            semantics.Add(Semantic.SV_Target, SV_Target);
            semantics.Add(Semantic.ViewDirection, ViewDirection);
        }

        internal static string Map(Semantic semantic)
        {
            return semantics[semantic];
        }
    }
}
