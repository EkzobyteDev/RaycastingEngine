using SFML.System;
using SFML.Graphics;


#pragma warning disable CS8618
namespace RaycastingEngine
{
    public class Camera
    {
        // Физические параметры камеры
        public AVector2f pos;
        public float rot;

        // Параметры, отвечающие за отрисовку
        internal Vector2u resolution;
        internal float fov;
        internal float renderDist;

        internal bool expandTextures = true;

        public Camera() { }
        public Camera(AVector2f pos, float rot)
        {
            this.pos = pos;
            this.rot = rot;
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

        Texture floorTex;
        Image floorImg;

        internal Image Render(Scene scene)
        {
            Image returnImage = new Image(resolution.X, resolution.Y);
            #region Подготовка некоторых полей
            if (floorImg == null)
            {
                floorTex = new Texture("D:/Projects/Git/RaycastingEngine/Scene/Textures/StoneFloor_3.jpg");
                floorImg = new Image(floorTex.CopyToImage());
            }

            AVector2f norm = MathV.Rotate(AVector2f.right, rot); // Camera look in the direction of this vector

            // r и l - точки, между которыми смотрит камера
            AVector2f r = MathV.Rotate(norm, fov / 2).normalized * renderDist + pos;
            AVector2f l = MathV.Rotate(norm, 360 - (fov / 2)).normalized * renderDist + pos;
            #endregion

            Parallel.For(0, resolution.X, (x) =>
            {
                #region Нахождение точек пересечения с объектами на сцене и получение некоторых важных параметров
                // p1 и p2 - крайние точки, "луча", который мы пускаем
                AVector2f p1 = pos;
                AVector2f p2 = (r - l) * (((float)x / (float)resolution.X) - (1 / ((float)resolution.X * 2))) + l;
                float minM = 0;

                float minDist = float.PositiveInfinity;

                foreach (Mesh mesh in scene.meshes)
                {
                    foreach (Edge edge in mesh.edges)
                    {
                        AVector2f p3 = MathV.Rotate(mesh.points[edge.p1], mesh.rot) + mesh.pos;
                        AVector2f p4 = MathV.Rotate(mesh.points[edge.p2], mesh.rot) + mesh.pos;

                        // n и m - множители, подставив которые в уравнения прямых, получим точку на них
                        float n = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) /
                                  ((p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x));

                        float m = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) /
                                  ((p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x));


                        // принадлежит ли точка пересечения "лучу" и ребру
                        if (n < 0 || n > 1 || m < 0 || m > 1) continue;

                        AVector2f intersectionPoint = new AVector2f(p1.x + n * (p2.x - p1.x), p1.y + n * (p2.y - p1.y));
                        float dist = (intersectionPoint - pos).length;

                        // Находим минимальную точку пересечения
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minM = expandTextures ? m * (p4 - p3).length : m;
                        }
                    }
                }
                // Умножаем дистанцию до объекта на косинус угла между лучом и направлением взгляда камеры.
                // Нужно, чтобы избавиться от эффекта "рыбьего глаза"
                if (minDist >= 0 && minDist <= float.PositiveInfinity)
                {
                    float cos = MathV.Cos(p2, norm);
                    if (cos > 0) minDist *= cos;
                }

                // Если будет меньше 0, то будут возникать артефакты
                minDist = Math.Clamp(minDist, 1, float.PositiveInfinity);
                float invDist = 1 / minDist;
                #endregion

                #region Отрисовка изображения на основе полученных данных
                // height - это высота (в пикселях) столбца, который будет отрисован на экране
                int height;

                if (minDist == renderDist) height = 0; // Не попали ни в один объект
                else height = (int)(resolution.Y * invDist);

                // gap - это высота пустоты (в пикселях)
                int gap = (int)((resolution.Y - height) / 2);
                gap = (int)Math.Clamp(gap, 0, resolution.Y);

                // Самое простое затенение. Чем объект дальше, тем он темнее
                float channel = Math.Clamp(invDist, 0, 1);

                //Вывод столбца в текстуру
                for (int y = gap; y < gap + height; y++)
                {
                    float Pixelx = expandTextures ? floorImg.Size.X * minM % floorImg.Size.X : floorImg.Size.X * minM;

                    Color color = floorImg.GetPixel((uint)Pixelx, (uint)((float)floorImg.Size.Y / (float)height * (float)(y - gap)));
                    color = new Color((byte)((float)color.R * channel), (byte)((float)color.G * channel), (byte)((float)color.B * channel));
                    returnImage.SetPixel((uint)x, (uint)y, color);
                }
                #endregion
            });
            return returnImage;
        }
    }
}
