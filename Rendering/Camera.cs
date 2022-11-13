using SFML.System;
using SFML.Graphics;


#pragma warning disable CS8618
namespace RaycastingEngine
{
    public class Camera
    {
        public Vector2f pos;
        public float rot;

        internal Vector2u resolution;
        internal float fov;
        internal float renderDist;


        public Camera() { }
        public Camera(Vector2f pos, float rot)
        {
            this.pos = pos;
            this.rot = rot;
        }
        public Camera(Vector2f pos, float rot, Vector2u resolution) : this(pos, rot) => this.resolution = resolution;
        public Camera(Vector2f pos, float rot, Vector2u resolution, float fov, float renderDist) : this(pos, rot, resolution)
        {
            this.fov = fov;
            this.renderDist = renderDist;
        }

        internal VertexArray Render(Scene scene)
        {
            Image renderedImage = new Image(resolution.X, resolution.Y);

            Vector2f norm = Rotate(new Vector2f(1, 0), rot); // Camera look in the direction of this vector

            // r и l - точки, между которыми смотрит камера
            Vector2f r = Normalize(Rotate(norm, fov / 2)) * renderDist + pos;
            Vector2f l = Normalize(Rotate(norm, 360 - (fov / 2))) * renderDist + pos;

            VertexArray line = new VertexArray(PrimitiveType.TriangleFan);
            line.Append(new Vertex(pos));
            line.Append(new Vertex(r));
            line.Append(new Vertex(l));

            //for (int x = 0; x < resolution.X; x++)
            //{
            //    // p1 и p2 - крайние точки, "луча", который мы пускаем
            //    Vector2f p1 = pos;
            //    Vector2f p2 = (r - l) * (x / resolution.X);

            //    float minDist = float.PositiveInfinity;

            //    foreach (Mesh mesh in scene.meshes)
            //    {
            //        foreach (Edge edge in mesh.edges)
            //        {
            //            Vector2f p3 = edge.p1;
            //            Vector2f p4 = edge.p2;

            //            float n = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) /
            //                      ((p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X));

            //            float m = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) /
            //                      ((p2.X - p1.X) * (p4.Y - p3.Y) - (p2.Y - p1.Y) * (p4.X - p3.X));

            //            // принадлежит ли точка пересечения "лучу" и ребру
            //            if (n < 0 || n > 1 || m < 0 || m > 1) continue;

            //            Vector2f intersectionPoint = new Vector2f(p1.X + n * (p2.X - p1.X), p1.Y + n * (p2.Y - p1.Y));
            //            float dist = Length(intersectionPoint - pos);

            //            if (dist < minDist) minDist = dist;
            //        }
            //    }

            //    // height - это высота (в пикселях) столбца, который будет отрисован на экране
            //    float height;

            //    if (minDist == 0) height = resolution.Y;
            //    else height = -(1 - (minDist / renderDist)) * resolution.Y;

            //    // gap - это высота пустоты (в пикселях)
            //    float gap = (resolution.Y - height) / 2;

            //    for (int y = (int)gap; y < (int)(gap + height); y++)
            //    {
            //        renderedImage.SetPixel((uint)x, (uint)y, Color.White);
            //    }
            //}

            return line;


            float Length(Vector2f a) => (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
            Vector2f Rotate(Vector2f a, float angle)
            {
                Vector2f b = new Vector2f();
                angle *= (float)(Math.PI / 180);
                b.X = (float)(a.X * Math.Cos(angle) - a.Y * Math.Sin(angle));
                b.Y = (float)(a.Y * Math.Cos(angle) + a.X * Math.Sin(angle));

                return b;
            }
            Vector2f Normalize(Vector2f a) => new Vector2f(a.X / Length(a), a.Y / Length(a));
        }
    }
}
