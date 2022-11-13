using SFML.System;
using SFML.Window;
using SFML.Graphics;

#pragma warning disable CS8618
namespace RaycastingEngine
{
    internal class Program_circle
    {
        static RenderWindow window;
        static Vector2u windowSize = new Vector2u(1920, 1080);

        static void Main()
        {
            window = new RenderWindow(new VideoMode(windowSize.X, windowSize.Y), "Raycasting Engine", Styles.Fullscreen);
            window.Closed += (object sender, EventArgs e) => window.Close();


            Random random = new Random();
            CircleShape circle = new CircleShape();
            CircleShape circleF1 = new CircleShape();
            CircleShape circleF2 = new CircleShape();
            VertexArray line = new VertexArray(PrimitiveType.Lines, 2);

            circle.FillColor = Color.Transparent;
            circle.OutlineColor = Color.White;
            circle.OutlineThickness = 1;
            circleF1.Radius = 7;
            circleF2.Radius = 7;

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
                window.Draw(circle);
                window.Draw(circleF1);
                window.Draw(circleF2);
                window.Display();
            }

            void Calculate()
            {
                line.Clear();

                Vector2f p1 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                Vector2f p2 = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                Vector2f o = new Vector2f(random.Next((int)windowSize.X), random.Next((int)windowSize.Y));
                float r = random.Next(50) + 100;

                line.Append(new Vertex(p1));
                line.Append(new Vertex(p2));
                circle.Position = o - new Vector2f(r, r);
                circle.Radius = r;

                float c = (r * r) - (2 * (p1.X - o.X + p1.Y - o.Y)) - (float)Math.Pow(p1.X - o.X, 2) - (float)Math.Pow(p1.Y - o.Y, 2);
                c /= 2;
                float b = p2.X - p1.X + p2.Y - p1.Y;
                float d = b * b + 4 * c;

                if (d < 0)
                {
                    circleF1.FillColor = Color.Transparent;
                    circleF2.FillColor = Color.Transparent;
                }
                else
                {
                    float n1 = (-b + (float)Math.Sqrt(d)) / 2;
                    float n2 = (-b - (float)Math.Sqrt(d)) / 2;

                    if (n1 >= 0 && n1 <= 1)
                    {
                        circleF1.FillColor = Color.Red;
                        circleF1.Position = n1 * (p2 - p1) + p1 - new Vector2f(7, 7);
                    }
                    else circleF1.FillColor = Color.Transparent;

                    if (n2 >= 0 && n2 <= 1)
                    {
                        circleF2.FillColor = Color.Red;
                        circleF2.Position = n2 * (p2 - p1) + p1 - new Vector2f(7, 7);
                    }
                    else circleF2.FillColor = Color.Transparent;
                }
            }
        }
    }

}
