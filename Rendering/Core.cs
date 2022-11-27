using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;


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

        static SceneImporter sceneImporter;
        static CameraMovement cameraMovement;

        static void Main()
        {
            #region Создание камеры

            camera = new Camera();

            camera.fov = 45;
            camera.pos = new AVector2f(0, 0);
            camera.rot = 0;
            camera.resolution = windowSize;
            camera.renderDist = 100;

            cameraMovement = new CameraMovement(camera);

            #endregion
            #region Создание окна

            window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            window.Closed += (object sender, EventArgs e) => window.Close();
            window.SetMouseCursorVisible(false);

            bool inFocus = true;
            window.LostFocus += (object? sender, EventArgs e) => inFocus = false;
            window.GainedFocus += (object? sender, EventArgs e) => inFocus = true;

            #endregion
            #region Создание сцены

            sceneImporter = new SceneImporter();
            sceneImporter.CreateScene();
            Mesh cube = sceneImporter.scene.meshes[0];

            #endregion

            image = new Image(windowSize.X, windowSize.Y);
            texture = new Texture(image);
            sprite = new Sprite(texture);

            Clock clock = new Clock();
            clock.Restart();
            float deltaTime = 0; // Время, за которое рендерится кдр
            float time;

            Music music = new Music("D:/Projects/Git/RaycastingEngine/Scene/Sounds/Music.wav");
            //music.Play();

            while (window.IsOpen)
            {
                window.DispatchEvents();
                if (!inFocus) continue;

                time = clock.ElapsedTime.AsSeconds();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

                #region Обновление сцены

                cube.rot = time * 45;
                sceneImporter.scene.meshes[2].pos.x = (float)Math.Sin(time) * 2 + 1.5f;

                #endregion

                cameraMovement.Update(deltaTime);

                RenderAndDisplay();

                deltaTime = (clock.ElapsedTime.AsSeconds() - time);
            }
            void RenderAndDisplay()
            {
                image = camera.Render(sceneImporter.scene);
                texture.Update(image);
                sprite.Texture = texture;
                window.Draw(sprite);

                window.Display();
            }
        }
    }
}
