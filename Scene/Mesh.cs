
using SFML.Graphics;




namespace RaycastingEngine
{
    public class Mesh : SceneObject
    {
        public AVector2f[] points;
        public (int p1, int p2)[] edges;

        public Image[] textures;


        public Mesh(int edgeCount, params string[] textureNames)
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
