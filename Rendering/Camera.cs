using SFML.System;
using SFML.Graphics;


namespace RaycastingEngine
{
    public class Camera : SceneObject
    {
        // Render parametres
        internal Vector2u resolution;
        internal float fov;
        internal float renderDist;

        public bool expandTextures = false;
        public bool useFishEyeEffect = false; // To disable the 'fish eye' effect, you should divide ray distance by cos of angle btw the ray and the camera


        Ray[] rays;



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



        public void Initialise()
        {
            uint raysCount = resolution.X;
            float angleBtwRays = fov / raysCount;

            rays = new Ray[raysCount];
            for (int i = 0; i < raysCount; i++)
                rays[i] = new Ray();
        }


        internal void Render(Image frame)
        {
            Parallel.For(0, rays.Length, i =>
            //for (int i = 0; i < rays.Length; i++)
            {
                float angleBtwRays = fov / rays.Length;
                float rayAngle = rotation + (fov / 2) - angleBtwRays * i;
                float rayLengthCorrection = useFishEyeEffect ? 1 : (float)Math.Cos((rayAngle - rotation) * Math.PI / 180);
                float rayDist = renderDist / rayLengthCorrection;



                rays[i].SetPoints(Ray.PointsFromRay(position, MathV.DirectionVector(rayAngle), rayDist));

                Color[] result = RenderRay(i, rayLengthCorrection);
                for (int j = 0; j < resolution.Y; j++)
                {
                    frame.SetPixel((uint)i, (uint)j, result[j]);
                }
            });
        }

        // A vertical line from top (id = 0) to bottom (id = resolution.y)
        Color[] RenderRay(int rayID, float heightCorrection)
        {
            List<(float distance, int edgeID, Mesh mesh, float f)> rayCollisions = GetRayCollisions(rays[rayID]);

            Color[] result = new Color[resolution.Y];


            for (int i = rayCollisions.Count - 1; i >= 0; i--)
            {
                int height = (int)Math.Round(resolution.Y * (1 / rayCollisions[i].distance) / heightCorrection); // Height of a column
                int top = (int)Math.Round((resolution.Y - height) / 2f); // Highest point of column
                int bottom = top + height; // Lowest point of column

                Image img = rayCollisions[i].mesh.textures[0];
                for (int y = 0; y < resolution.Y; y++)
                {
                    Color color = Color.Transparent;

                    if (y >= top && y < bottom)
                    {
                        // textureX = x size of texture / length of edge * position on edge
                        // textureY = y size of texture / height of column * position on column

                        (int p1, int p2) edge = rayCollisions[i].mesh.edges[rayCollisions[i].edgeID];
                        float textureX = (float)img.Size.X / (rayCollisions[i].mesh.points[edge.p1] - rayCollisions[i].mesh.points[edge.p2]).length * rayCollisions[i].f;
                        float textureY = (float)img.Size.Y / (float)height * (y - top);
                        color = img.GetPixel((uint)textureX, (uint)textureY);
                    }

                    if (result[y] == Color.Transparent)
                        result[y] = color;
                    else
                        result[y] = Blend(color, result[y]);

                    //result[y] = color;
                }
            }
            return result;
        }

        // Returns info about all collisions of ray
        // Does not use culling
        List<(float distance, int edgeID, Mesh mesh, float f)> GetRayCollisions(Ray ray)
        {
            List<(float distance, int edgeID, Mesh mesh, float f)> collisions = new List<(float, int, Mesh, float)>();

            string s = "";
            foreach (Mesh mesh in Scene.instance.meshes)
            {
                for (int i = 0; i < mesh.edges.Length; i++)
                {
                    (bool exists, AVector2f point, float f) intersection = ray.GetIntersectionPoint_ParametricEquations(mesh.points[mesh.edges[i].p1] + mesh.position, mesh.points[mesh.edges[i].p2] + mesh.position);

                    s += intersection.exists + " " + intersection.point + " || ";

                    if (intersection.exists)
                        collisions.Add(((intersection.point - position).length, i, mesh, intersection.f));
                }
            }

            return collisions.OrderBy(collision => collision.distance).ToList();
        }




        public static Color Multiply(Color a, float b) => new Color((byte)((float)a.R * b), (byte)((float)a.G * b), (byte)((float)a.B * b));
        public static Color Blend(Color a, Color b)
        {
            // Formula was taken from
            // https://en.wikipedia.org/wiki/Alpha_compositing#Alpha_blending

            float aA = a.A / 255f;
            float bA = b.A / 255f;

            float red = a.R * aA + b.R * bA * (1 - aA);
            float green = a.G * aA + b.G * bA * (1 - aA);
            float blue = a.B * aA + b.B * bA * (1 - aA);
            float alfa = aA + bA * (1 - aA);
            alfa *= 255;
            return new Color((byte)red, (byte)green, (byte)blue, (byte)alfa);
        }
    }
}
