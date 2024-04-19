using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;



namespace RaycastingEngine
{
    public class Core
    {
        public static string texturesPath = Environment.CurrentDirectory + @"\Assets\Textures\";
        public static bool showHelpMenu = false;
        // 
        static RenderWindow window; 
        static Vector2u windowResolution = new Vector2u(1920, 1080);

        static Scene scene;
        static Camera camera;

        static Text text;


        static void Main()
        {

            Font font = new Font(@"D:\Projects\RaycastingEngine\consolas.ttf");
            text = new Text("", font);
            text.FillColor = Color.Red;
            text.CharacterSize = 16;

            Controls.Handle((Vector2i)windowResolution);

            CreateWindow();
            CreateScene();
            CreateCamera();

            Image renderedImage = new Image(windowResolution.X, windowResolution.Y);
            Texture renderedTexture = new Texture(renderedImage);
            Sprite renderedSprite = new Sprite(renderedTexture);

            Clock clock = new Clock();
            float runTime = CurrentTime();

            while (window.IsOpen)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

                float renderStartTime = CurrentTime();

                window.Clear();
                text.DisplayedString = "";

                window.DispatchEvents();

                Controls.Handle((Vector2i)windowResolution);
                camera.rotation += -Controls.mouseDelta.X * 0.025f;
                camera.position += MathV.Rotate(Controls.inputDirection, camera.rotation) * 0.035f * 2;

                camera.Render(renderedImage);
                renderedTexture.Update(renderedImage);
                window.Draw(renderedSprite);

                //scene.meshes[1].position.x = MathF.Sin((renderStartTime - runTime))*10;
                //scene.meshes[1].position.y = MathF.Cos((renderStartTime - runTime)) * 10;

                if (!showHelpMenu) Log("Нажмите 0 чтобы открыть меню помощи");
                else
                {
                    Log("Нажмите 0 чтобы закрыть меню помощи");
                    Log("Нажмите Esc чтобы закрыть программу");
                    Log("Нажимайте WASD чтобы перемещаться по сцене");
                    Log("\n");
                    Log("Нажмите 1 чтобы включить/выключить режим \"Вертикальные линии без высоты\"");
                    Log("Нажмите 2 чтобы включить/выключить эффект \"рыбьего глаза\"");
                    Log("Нажмите 3 чтобы включить/выключить отображение текстур");
                    Log("Нажмите 4 чтобы включить/выключить отображение прозрачных текстур");
                }

                bool debug = false;
                if (debug)
                {
                    Log($"Current time: {(CurrentTime() - runTime).ToString("0.00")} s");
                    Log($"FPS: {(1 / (CurrentTime() - renderStartTime)).ToString("0.00")}");

                    Log($"Camera position: {camera.position.ToString()}  Camera rotation: {camera.rotation.ToString("0.00")}");
                }

                window.Draw(text);
                window.Display();
            }


            float CurrentTime() => clock.ElapsedTime.AsMilliseconds() / 1000f;
        }


        static void CreateWindow()
        {
            window = new RenderWindow(new VideoMode(windowResolution.X, windowResolution.Y), "Raycasting Engine", Styles.Fullscreen);
            window.Closed += (object sender, EventArgs e) => window.Close();
            window.LostFocus += (_, _) => Controls.lockCursor = false;
            window.GainedFocus += (_, _) => Controls.lockCursor = true;
            //window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
        }

        static void CreateScene()
        {
            scene = new Scene();
            Scene.instance = scene;

            Mesh mesh = new Mesh(4, "Brick.png");
            mesh.points = new AVector2f[] { new AVector2f(0, 0), new AVector2f(1, 0), new AVector2f(1, 1), new AVector2f(0, 1) };
            mesh.edges = new (int p1, int p2)[] { (0, 1), (1, 2), (2, 3), (3, 0) };
            mesh.position = new AVector2f(1, 0);

            Mesh mesh1 = new Mesh(4, "Brick_1.png");
            mesh1.points = new AVector2f[] { new AVector2f(0, 0), new AVector2f(1, 0), new AVector2f(1, 1), new AVector2f(0, 1) };
            mesh1.edges = new (int p1, int p2)[] { (0, 1), (1, 2), (2, 3), (3, 0) };
            mesh1.position = new AVector2f(-1, 0);


            scene.meshes.Add(mesh);
            scene.meshes.Add(mesh1);
        }
        static void CreateCamera()
        {
            camera = new Camera();
            camera.resolution = windowResolution;
            camera.fov = 60;
            camera.renderDist = 1000;

            camera.position = new AVector2f(0.5f, 5);
            camera.rotation = -90;


            camera.Initialize();
        }




        public static void Log(string message)
        {
            text.DisplayedString += message + '\n';
        }
    }
}
