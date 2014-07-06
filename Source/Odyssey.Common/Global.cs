namespace Odyssey
{
    public static class Global
    {
        public const string EngineTag = "Odyssey";
        public const string NetworkTag = "Network";
        public const string RenderingTag = "Rendering";
        public const string UITag = "UI";
        public const string IoTag = "IO";
        public const string ToolTag = "Tool";

        public const string Assets = "Assets/";
        public const string ModelPath = Assets + "Models/";
        public const string EffectPath = Assets + "Effects/";
        public const string XmlPath = Assets + "Data/";
        public const string TexturePath = Assets + "Textures/";
        public const string GUIPath = Assets + "GUI/";

        /// <summary>
        /// Default width for the back buffer.
        /// </summary>
        public static readonly int DefaultBackBufferWidth = 1280;

        /// <summary>
        /// Default height for the back buffer.
        /// </summary>
        public static readonly int DefaultBackBufferHeight = 720;

        public static string RootPath { get; set; }
    }
}