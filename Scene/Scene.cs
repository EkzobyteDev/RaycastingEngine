using SFML.Graphics;

namespace RaycastingEngine
{
    public class Scene
    {
        public static Scene instance { get; set; }

        public List<Mesh> meshes = new List<Mesh>();




        //public void LoadTextures()
        //{
        //    defaultImg = new Image(Environment.CurrentDirectory + "/Textures/0.jpg");
        //    skyImg = new Image(Environment.CurrentDirectory + "/Textures/Sky.jpg");

        //    images = new Dictionary<string, Image>();
        //    foreach (Mesh mesh in meshes)
        //    {
        //        if (images.Keys.Contains(mesh.textureName)) continue;

        //        images[mesh.textureName] = new Image(Environment.CurrentDirectory + $"/Textures/{mesh.textureName}.jpg");
        //    }
        //}
    }
}
