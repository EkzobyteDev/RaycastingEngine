
using SFML.Graphics;




namespace RaycastingEngine
{
    public class Mesh : SceneObject
    {
        public AVector2f[] points; // Массив вершин (хранятся только координаты)
        public (int p1, int p2)[] edges; // Массив рёбер (хранятся индексы соотверствующих ребру вершин)

        public Image[] textures; // Набор текстур


        public Mesh(int edgeCount, params string[] textureNames) // Создание меша
        {
            this.edges = new (int p1, int p2)[edgeCount];

            textures = new Image[textureNames.Length];
            for (int i = 0; i < textureNames.Length; i++)
            {
                textures[i] = new Texture(Core.texturesPath + textureNames[i]).CopyToImage();
            }
        }
    }
}
