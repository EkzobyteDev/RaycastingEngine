using SFML.System;

namespace RaycastingEngine
{
    internal class Mesh
    {
        internal Edge[] edges;

        public Mesh(int edgeCount)
        {
            this.edges = new Edge[edgeCount];
        }
    }
    internal class Edge
    {
        internal Vector2f p1, p2;

        public Edge(Vector2f p1, Vector2f p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
