using SFML.System;
using SFML.Window;

namespace RaycastingEngine
{
    internal class CameraMovement
    {
        Camera camera;
        Vector2i mousePos;

        public CameraMovement(Camera camera)
        {
            this.camera = camera;
            this.mousePos = (Vector2i)(camera.resolution / 2);
        }

        internal void Update(float deltaTime)
        {
            AVector2f inputDir = Controls.inputDirection;
            inputDir = MathV.Rotate(inputDir, camera.rot).normalized;
            camera.pos += inputDir * 2 * deltaTime;

            float mouseDelta = Controls.mousePos.X - mousePos.X;
            Mouse.SetPosition(mousePos);
            camera.rot += mouseDelta * 3 * deltaTime;
        }
    }
}
