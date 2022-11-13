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

            camera.fov = 90;
            camera.pos = new Vector2f(1000, 500);
            camera.rot = 0;
            camera.resolution = windowSize;
            camera.renderDist = 200;

            scene = new Scene();
            scene.meshes = new Mesh[1];
            scene.meshes[0] = new Mesh(2);
            scene.meshes[0].edges[0] = new Edge(new Vector2f(3, 0), new Vector2f(6, 2));
            scene.meshes[0].edges[1] = new Edge(new Vector2f(3, 0), new Vector2f(6, -2));


            window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            image = new Image(windowSize.X, windowSize.Y);
            texture = new Texture(image);
            sprite = new Sprite(texture);

            window.Closed += (object sender, EventArgs e) => window.Close();

            float t = 0;
            while (window.IsOpen)
            {
                t += 0.01f;
                window.Clear();
                texture.Update(image.Pixels);

                window.Draw(camera.Render(scene));
                window.Display();
                window.DispatchEvents();
            }
        }
    }
}
