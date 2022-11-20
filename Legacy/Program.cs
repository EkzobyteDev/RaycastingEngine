using SFML.System;
using SFML.Window;
using SFML.Graphics;

#pragma warning disable CS8618
namespace RaycastingEngine
{
    internal class Core2
    {
        static RenderWindow window;
        static Vector2u windowSize = new Vector2u(1920, 1080);

        static void Maein()
        {
            window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            window.Closed += (object sender, EventArgs e) => window.Close();


            Random random = new Random();
            VertexArray line = new VertexArray(PrimitiveType.Lines, 2);
            VertexArray ray = new VertexArray(PrimitiveType.Lines, 2);
            CircleShape circle = new CircleShape();


            int a = 0;
            Calculate();

            while (window.IsOpen)
            {
                if (++a >= 3000)
                {
                    Calculate();
                    a = 0;
                }

                window.DispatchEvents();
                window.Clear(Color.Black);
                window.Draw(line);
                window.Draw(ray);
                window.Draw(circle);
                window.Display();
            }

            void Calculate()
            {
                line.Clear();
                ray.Clear();

                Vector2f p1 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                Vector2f p2 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                Vector2f p3 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                Vector2f p4 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));

                line.Append(new Vertex(p3));
                line.Append(new Vertex(p4));
                ray.Append(new Vertex(p1));
                ray.Append(new Vertex(p2));



                float n = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) /
                          ((p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X));

                float m = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) /
                          ((p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X));

                if (n < 0 || n > 1 || m < 0 || m > 1) circle.FillColor = Color.Transparent;
                else circle.FillColor = Color.Red;

                circle.Position = new Vector2f(p1.X + n * (p2.X - p1.X), p1.Y + n * (p2.Y - p1.Y));
                circle.Position = circle.Position - new Vector2f(7, 7);

                circle.Radius = 7;
            }
        }
    }
}
