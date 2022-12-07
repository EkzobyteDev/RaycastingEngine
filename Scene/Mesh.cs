

namespace RaycastingEngine
{
    internal class Mesh
    {
        internal AVector2f position;
        internal float rotation;
        internal float scale;

        internal string textureName;

        internal AVector2f[] points;
        internal Edge[] edges;

        public Mesh(int edgeCount)
        {
            this.edges = new Edge[edgeCount];
        }
    }
    internal class Edge
    {
        internal int p1, p2;

        public Edge(int p1, int p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
