

namespace RaycastingEngine
{
    internal class SceneImporter
    {
        internal Scene scene;

        public Scene ImportScene() { return null; }
        public Scene CreateScene()
        {
            scene = new Scene();
            scene.meshes = new Mesh[3];

            // Квадрат
            scene.meshes[0] = new Mesh(4);
            scene.meshes[0].pos = new AVector2f(2, 0);
            scene.meshes[0].edges[0] = new Edge(new AVector2f(-0.5f, 0.5f), new AVector2f(0.5f, 0.5f));
            scene.meshes[0].edges[1] = new Edge(new AVector2f(0.5f, 0.5f), new AVector2f(0.5f, -0.5f));
            scene.meshes[0].edges[2] = new Edge(new AVector2f(0.5f, -0.5f), new AVector2f(-0.5f, -0.5f));
            scene.meshes[0].edges[3] = new Edge(new AVector2f(-0.5f, -0.5f), new AVector2f(-0.5f, 0.5f));

            // Треугольник
            scene.meshes[1] = new Mesh(3);
            scene.meshes[1].edges[0] = new Edge(new AVector2f(0, 1), new AVector2f(1, -1));
            scene.meshes[1].edges[1] = new Edge(new AVector2f(1, -1), new AVector2f(-1, -1));
            scene.meshes[1].edges[2] = new Edge(new AVector2f(-1, -1), new AVector2f(0, 1));
            scene.meshes[1].pos = new AVector2f(-2, -2);

            // Отрезок
            scene.meshes[2] = new Mesh(1);
            scene.meshes[2].edges[0] = new Edge(new AVector2f(-0.5f, 0), new AVector2f(0.5f, 0));
            scene.meshes[2].pos = new AVector2f(-1.5f, 2);

            return scene;
        }
    }
}
