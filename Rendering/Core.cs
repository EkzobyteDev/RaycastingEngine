using SFML.System;
using SFML.Window;
using SFML.Graphics;


#pragma warning disable CS8618
namespace RaycastingEngine
{
    public class Core
    {
        static Camera camera;
        static RenderWindow window;
        static Vector2u windowSize = new Vector2u(1920, 1080);

        static Image image;
        static Texture texture;
        static Sprite sprite;

        static Scene scene;

        static void Main()
        {
            camera = new Camera();

            camera.fov = 45;
            camera.pos = new Vector2f(0, 0);
            camera.rot = 0;
            camera.resolution = windowSize;
            camera.renderDist = 5;

            scene = new Scene();
            scene.meshes = new Mesh[1];
            scene.meshes[0] = new Mesh(3);
            scene.meshes[0].edges[0] = new Edge(new Vector2f(1, 0), new Vector2f(2, 1));
            scene.meshes[0].edges[1] = new Edge(new Vector2f(2, 1), new Vector2f(2, -1));
            scene.meshes[0].edges[2] = new Edge(new Vector2f(2, -1), new Vector2f(1, 0));


            window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            image = new Image(windowSize.X, windowSize.Y);
            texture = new Texture(image);
            sprite = new Sprite(texture);

            window.Closed += (object sender, EventArgs e) => window.Close();
            window.SetMouseCursorVisible(false);

            float t = 0;
            Vector2i defaultMousePos = (Vector2i)(windowSize / 2);
            Vector2f inputDir;

            while (window.IsOpen)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) inputDir.X = 1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) inputDir.X = -1;
                else inputDir.X = 0;
                if (Keyboard.IsKeyPressed(Keyboard.Key.W)) inputDir.Y = -1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S)) inputDir.Y = 1;
                else inputDir.Y = 0;
                camera.pos += inputDir / 20;


                float mouseDelta = Mouse.GetPosition().X - defaultMousePos.X;
                Mouse.SetPosition(defaultMousePos);
                camera.rot += mouseDelta / 7.5f;
                t += 0.01f;
                window.Clear();
                image = camera.Render(scene);
                texture.Update(image.Pixels);
                window.Draw(sprite);

                window.Display();
                window.DispatchEvents();
            }
        }
    }
}
