using Odyssey.Engine;
using Odyssey.Properties;
using Odyssey.Utils.Logging;
using SharpDX;
using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Odyssey
{
    internal static class Global
    {
        public const string EngineTag = "Odyssey";
        public const string NetworkTag = "Network";
        public const string RenderingTag = "Rendering";
        public const string UITag = "UI";
        public const string IoTag = "IO";
        public const string ToolTag = "Tool";

        public const string Resources = "Resources/";
        public const string ModelPath = Resources + "Models/";
        public const string EffectPath = Resources + "Effects/";
        public const string XmlPath = Resources + "Data/";
        public const string TexturePath = Resources + "Textures/";
        public const string GUIPath = Resources + "GUI/";

        public static string RootPath { get; set; }


        public static ManualResetEventSlim RenderEvent { get; private set; }
        public static bool IsRunning { get; private set; }
        public static bool IsInputEnabled { get; private set; }
        public static IOdysseyTarget Target { get; internal set; }
        public static DeviceSettings Settings { get; set; }

        static Global()
        {
            RenderEvent = new ManualResetEventSlim(false);
            IsRunning = true;
        }

        public static void Initialize(DeviceManager deviceManager)
        {
            Contract.Requires<NullReferenceException>(deviceManager != null);

            LogEvent.Engine.Info(Info.INFO_OE_Starting);

            Settings = deviceManager.Settings;
            LogEvent.Engine.Info(Info.INFO_OE_Started);


        }


    }
}
