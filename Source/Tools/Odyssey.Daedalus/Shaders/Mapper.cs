namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public static class Mapper
    {
        public static string Map(Type type)
        {
            return HLSLTypes.Map(type);
        }

    }
}
