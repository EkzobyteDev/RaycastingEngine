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


        internal static bool lockCursor = true;
        internal static Vector2i mouseDelta { get; private set; }
        internal static void Handle(Vector2i resolution)
        {
            if (lockCursor)
            {
                mouseDelta = Mouse.GetPosition() - resolution/2;
                Mouse.SetPosition(resolution / 2);
            }
        }
    }
}
