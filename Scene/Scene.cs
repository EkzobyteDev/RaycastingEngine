using SFML.Graphics;

namespace RaycastingEngine
{
    internal class Scene
    {
        internal Image defaultImg;
        internal Image skyImg;
        internal Dictionary<string, Image> images;

        internal float lightIntensity = 1.5f;
        internal Mesh[] meshes;

        internal void LoadTextures()
        {
            defaultImg = new Image(Environment.CurrentDirectory + "/Textures/0.jpg");
            skyImg = new Image(Environment.CurrentDirectory + "/Textures/Sky.jpg");

            images = new Dictionary<string, Image>();
            foreach (Mesh mesh in meshes)
            {
                if (images.Keys.Contains(mesh.textureName)) continue;

                images[mesh.textureName] = new Image(Environment.CurrentDirectory + $"/Textures/{mesh.textureName}.jpg");
            }
        }
    }
}
