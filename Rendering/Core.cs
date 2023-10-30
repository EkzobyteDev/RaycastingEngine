using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;



namespace RaycastingEngine
{
    public class Core
    {
        public static string texturesPath = Environment.CurrentDirectory + @"\Assets\Textures\";

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


            #region Создание камеры

            //    camera = new Camera();

            //    camera.fov = 45;
            //    camera.position = AVector2f.zero;
            //    camera.rotation = 0;
            //    camera.resolution = windowSize;
            //    camera.renderDist = 100;

            //    cameraMovement = new CameraMovement(camera);

            //    #endregion
            //    #region Создание окна

            //    window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            //    window.Closed += (object sender, EventArgs e) => window.Close();
            //    window.SetMouseCursorVisible(false);

            //    bool inFocus = true;
            //    window.LostFocus += (object? sender, EventArgs e) => inFocus = false;
            //    window.GainedFocus += (object? sender, EventArgs e) => inFocus = true;

            //    #endregion
            //    #region Создание сцены

            //    sceneImporter = new SceneImporter();
            //    sceneImporter.CreateScene();
            //    sceneImporter.scene.LoadTextures();
            //    Mesh cube = sceneImporter.scene.meshes[0];

            //    #endregion

            //    image = new Image(windowSize.X, windowSize.Y);
            //    texture = new Texture(image);
            //    sprite = new Sprite(texture);

            //    Clock clock = new Clock();
            //    clock.Restart();
            //    float deltaTime = 0; // Время, за которое рендерится кдр
            //    float time;

            //    while (window.IsOpen)
            //    {
            //        window.DispatchEvents();
            //        if (!inFocus) continue;

            //        time = clock.ElapsedTime.AsSeconds();
            //        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

            //        #region Обновление сцены

            //        //cube.rotation = time * 45;
            //        //sceneImporter.scene.meshes[2].position.x = (float)Math.Sin(time) * 2 + 1.5f;
            //        //cube.scale = (float)Math.Abs(Math.Sin(time)) * 1;

            //        #endregion

            //        cameraMovement.Update(deltaTime);

            //        RenderAndDisplay();

            //        deltaTime = (clock.ElapsedTime.AsSeconds() - time);
            //    }
            //    void RenderAndDisplay()
            //    {
            //        image = camera.Render(sceneImporter.scene);
            //        texture.Update(image);
            //        sprite.Texture = texture;
            //        window.Draw(sprite);

            //        window.Display();
            //    }
            #endregion

            CreateWindow();
            CreateScene();
            CreateCamera();

            Image renderedImage = new Image(windowResolution.X, windowResolution.Y);
            Texture renderedTexture = new Texture(renderedImage);
            Sprite renderedSprite = new Sprite(renderedTexture);

            while (window.IsOpen)
            {
                window.Clear();
                text.DisplayedString = "";

                window.DispatchEvents();

                Controls.Handle((Vector2i)windowResolution);
                camera.rotation += -Controls.mouseDelta.X * 0.025f;
                camera.position += MathV.Rotate(Controls.inputDirection, camera.rotation) * 0.035f * 2;

                //if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                //    camera.useFishEyeEffect = true;
                //else
                //    camera.useFishEyeEffect = false;

                camera.Render(renderedImage);
                renderedTexture.Update(renderedImage);
                window.Draw(renderedSprite);




                window.Draw(text);
                window.Display();
            }
        }



        static void CreateWindow()
        {
            window = new RenderWindow(new VideoMode(windowResolution.X, windowResolution.Y), "Raycasting Engine", Styles.Fullscreen);
            window.Closed += (object sender, EventArgs e) => window.Close();
            window.LostFocus += (_, _) => Controls.lockCursor = false;
            window.GainedFocus += (_, _) => Controls.lockCursor = true;
            window.SetFramerateLimit(60);
            window.SetMouseCursorVisible(false);
        }

        static void CreateScene()
        {
            scene = new Scene();
            Scene.instance = scene;

            Mesh mesh = new Mesh(4, "Brick_1.png");
            mesh.points = new AVector2f[] { new AVector2f(0, 0), new AVector2f(1, 0), new AVector2f(1, 1), new AVector2f(0, 1) };
            mesh.edges = new (int p1, int p2)[] { (0, 1), (1, 2), (2, 3), (3, 0) };
            mesh.position = new AVector2f(10, 0);

            Mesh mesh1 = new Mesh(4, "Brick_1.png");
            mesh1.points = new AVector2f[] { new AVector2f(0, 0), new AVector2f(1, 0), new AVector2f(1, 1), new AVector2f(0, 1) };
            mesh1.edges = new (int p1, int p2)[] { (0, 1), (1, 2), (2, 3), (3, 0) };
            mesh1.position = new AVector2f(15, 0);


            scene.meshes.Add(mesh);
            scene.meshes.Add(mesh1);
        }
        static void CreateCamera()
        {
            camera = new Camera();
            camera.resolution = windowResolution;
            camera.fov = 60;
            camera.renderDist = 1000;

            camera.Initialise();
        }




        public static void Log(string message)
        {
            text.DisplayedString += message + '\n';
        }
    }
}
