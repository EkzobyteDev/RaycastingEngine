using SFML.System;
using SFML.Window;

namespace RaycastingEngine
{
    internal static class Controls
    {
        internal static AVector2f inputDirection
        {
            get
            {
                AVector2f inputDir;

                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) inputDir.y = -1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) inputDir.y = 1;
                else inputDir.y = 0;

                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) inputDir.x = 1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S)) inputDir.x = -1;
                else inputDir.x = 0;

                return inputDir;
            }
        }
        static bool wasPressed0;
        static bool wasPressed1;
        static bool wasPressed2;
        static bool wasPressed3;
        static bool wasPressed4;


        internal static bool lockCursor = true;
        internal static Vector2i mouseDelta { get; private set; }
        internal static void Handle(Vector2i resolution)
        {
            if (lockCursor)
            {
                mouseDelta = Mouse.GetPosition() - resolution/2;
                Mouse.SetPosition(resolution / 2);
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.Num0) && wasPressed0 == false) Core.showHelpMenu = !Core.showHelpMenu;
            wasPressed0 = Keyboard.IsKeyPressed(Keyboard.Key.Num0);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num1) && wasPressed1 == false) Camera.noHeight = !Camera.noHeight;
            wasPressed1 = Keyboard.IsKeyPressed(Keyboard.Key.Num1);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num2) && wasPressed2 == false) Camera.useFishEyeEffect = !Camera.useFishEyeEffect;
            wasPressed2 = Keyboard.IsKeyPressed(Keyboard.Key.Num2);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num3) && wasPressed3 == false) Camera.noTextures = !Camera.noTextures;
            wasPressed3 = Keyboard.IsKeyPressed(Keyboard.Key.Num3);

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num4) && wasPressed4 == false) Camera.noTransparency = !Camera.noTransparency;
            wasPressed4 = Keyboard.IsKeyPressed(Keyboard.Key.Num4);
        }
    }
}
