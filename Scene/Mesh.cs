

namespace RaycastingEngine
{
    internal class Mesh
    {
        internal AVector2f pos;
        internal float rot;
        internal Edge[] edges;

        public Mesh(int edgeCount)
        {
            this.edges = new Edge[edgeCount];
        }
    }
    internal class Edge
    {
        internal AVector2f p1, p2;

        public Edge(AVector2f p1, AVector2f p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
