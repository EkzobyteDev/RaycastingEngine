using SFML.System;
using SFML.Graphics;


#pragma warning disable CS8618
namespace RaycastingEngine
{
    public class Camera
    {
        // Физические параметры камеры
        public AVector2f position;
        public float rotation;
        internal Vector2f size;

        // Параметры, отвечающие за отрисовку
        internal Vector2u resolution;
        internal float fov;
        internal float renderDist;

        internal bool expandTextures = false;

        public Camera() { }
        public Camera(AVector2f pos, float rot)
        {
            this.position = pos;
            this.rotation = rot;
        }
        public Camera(AVector2f pos, float rot, Vector2u resolution) : this(pos, rot)
        {
            this.resolution = resolution;
        }
        public Camera(AVector2f pos, float rot, Vector2u resolution, float fov, float renderDist) : this(pos, rot, resolution)
        {
            this.fov = fov;
            this.renderDist = renderDist;
        }

        internal Image Render(Scene scene)
        {
            Image returnImage = new Image(resolution.X, resolution.Y);

            #region Подготовка некоторых полей
            AVector2f norm = MathV.Rotate(AVector2f.right, rotation); // Camera look in the direction of this vector

            // r и l - точки, между которыми смотрит камера
            AVector2f r = MathV.Rotate(norm, fov / 2).normalized * renderDist + position;
            AVector2f l = MathV.Rotate(norm, 360 - (fov / 2)).normalized * renderDist + position;
            #endregion

            Parallel.For(0, resolution.X, (x) =>
            {
                #region Нахождение точек пересечения с объектами на сцене и получение некоторых важных параметров
                // p1 и p2 - крайние точки, "луча", который мы пускаем
                AVector2f p1 = position;
                AVector2f p2 = (r - l) * (((float)x / (float)resolution.X) - (1 / ((float)resolution.X * 2))) + l;
                float minM = 0;

                float minDist = float.PositiveInfinity;
                Image image = scene.defaultImg;

                foreach (Mesh mesh in scene.meshes)
                {
                    foreach (Edge edge in mesh.edges)
                    {
                        AVector2f p3 = MathV.Rotate(mesh.points[edge.p1], mesh.rotation) * mesh.scale + mesh.position;
                        AVector2f p4 = MathV.Rotate(mesh.points[edge.p2], mesh.rotation) * mesh.scale + mesh.position;

                        // n и m - множители, подставив которые в уравнения прямых, получим точку на них
                        float n = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) /
                                  ((p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x));

                        float m = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) /
                                  ((p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x));


                        // принадлежит ли точка пересечения "лучу" и ребру
                        if (n < 0 || n > 1 || m < 0 || m > 1) continue;

                        AVector2f intersectionPoint = new AVector2f(p1.x + n * (p2.x - p1.x), p1.y + n * (p2.y - p1.y));
                        float dist = (intersectionPoint - position).length;

                        // Находим минимальную точку пересечения
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minM = expandTextures ? m : m * (p4 - p3).length;
                            image = scene.images[mesh.textureName];
                        }
                    }
                }
                // Умножаем дистанцию до объекта на косинус угла между лучом и направлением взгляда камеры.
                // Нужно, чтобы избавиться от эффекта "рыбьего глаза"
                minDist = Math.Clamp(minDist, 0.01f, float.PositiveInfinity);
                float cos = MathV.Cos(p2, norm);
                if (cos > 0) minDist *= cos;

                float invDist = 1 / minDist;
                #endregion

                #region Отрисовка изображения на основе полученных данных
                // height - это высота (в пикселях) столбца, который будет отрисован на экране
                int height;

                if (minDist == renderDist) height = 0; // Не попали ни в один объект
                else height = (int)(resolution.Y * invDist);

                // gap - это высота пустоты (в пикселях)
                int gap = (int)((resolution.Y - height) / 2);

                //Вывод столбца в текстуру

                for (int y = 0; y < resolution.Y; y++)
                {
                    Color color = Color.Black;
                    // Стены
                    if (y >= gap && y <= gap + height)
                    {
                        // Самое простое затенение. Чем объект дальше, тем он темнее
                        float channel = Math.Clamp(invDist * scene.lightIntensity, 0, 1);

                        float Pixelx = expandTextures ? image.Size.X * minM : (image.Size.X * minM) % image.Size.X;
                        float Pixely = ((float)image.Size.Y / (float)height * (float)(y - gap)) % image.Size.Y;
                        color = image.GetPixel((uint)Pixelx, (uint)Pixely);
                        color = Multiply(color, channel);

                    }

                    // Небо
                    else if (y < resolution.Y / 2)
                    {
                        float pixelX = Math.Abs((x + (rotation / 360 * resolution.X)) % scene.skyImg.Size.X);
                        color = scene.skyImg.GetPixel((uint)pixelX, (uint)(y % scene.skyImg.Size.Y));
                    }

                    // Пол
                    else if (y > resolution.Y / 2)
                    {
                        color = Multiply(new Color(52, 40, 38), (float)Math.Pow((float)y / (float)resolution.Y - 0.5f, scene.lightIntensity/2f));
                    }

                    returnImage.SetPixel((uint)x, (uint)y, color);
                }
                #endregion
            });
            return returnImage;
        }
        public static Color Multiply(Color a, float b) => new Color((byte)((float)a.R * b), (byte)((float)a.G * b), (byte)((float)a.B * b));
    }
}
