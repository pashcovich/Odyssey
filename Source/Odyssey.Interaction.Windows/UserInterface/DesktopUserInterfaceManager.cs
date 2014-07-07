#region Using Directives

using Odyssey.Engine;
using Odyssey.Interaction;
using SharpDX;

#endregion Using Directives

namespace Odyssey.UserInterface
{
    public class DesktopUserInterfaceManager : UserInterfaceManager
    {
        private readonly KeyboardManager keyboardManager;
        private KeyboardState keyboardState;

        public DesktopUserInterfaceManager(IServiceRegistry services)
            : base(services)
        {
            keyboardManager = new KeyboardManager(services);
            PointerPlatform = new DesktopPointerPlatform(PointerManager);
        }

        public override void Initialize()
        {
            base.Initialize();
            IWindowService windowService = Services.GetService<IWindowService>();
            KeyboardPlatform keyboardPlatform = new DesktopKeyboardPlatform(keyboardManager);
            keyboardPlatform.Initialize(windowService.NativeWindow);
            keyboardManager.Associate(keyboardPlatform);
        }

        public override void Update()
        {
            // read the current keyboard state
            keyboardManager.Update();
            keyboardState = keyboardManager.GetState();

            base.Update();
        }
    }
}