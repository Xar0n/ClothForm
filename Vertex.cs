using OpenTK;

namespace ClothForm
{
    public struct Vertex_s
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
    }
    class Vertex
    {
        private int gridSize;
        private Vertex_s[] vertices;
        private int vertexCount;
        public Vertex(int gridSize)
        {
            this.gridSize = gridSize;
            vertexCount = (gridSize * gridSize);
        }

        public void calculateVertices()
        {
            int k;
            vertices = new Vertex_s[vertexCount];
            for (int j = 0; j < gridSize; j++)
            {
                for (int i = 0; i < gridSize; i++)
                {
                    k = j * gridSize + i;
                    vertices[k].uv = new Vector2(i / (float)gridSize, j / (float)gridSize);
                }
            }
        }

        public void calculateNormal(Triangle_s[] triangles)
        {
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize; i++)
                {
                    int BallID = j * gridSize + i;
                    Vector3 normal = Vector3.Zero;
                    int count = 0;
                    for (int Y = 0; Y <= 1; Y++)
                        for (int X = 0; X <= 1; X++)
                        {
                            if (X + i < gridSize && Y + j < gridSize)
                            {
                                int index = (j + Y) * gridSize + (i + X) * 2;
                                normal += triangles[index].normal;
                                index++;
                                normal += triangles[index].normal;
                                count += 2;
                            }
                        }
                    normal /= (float)count;
                    vertices[BallID].normal = normal;
                }
        }
        
        public void calculatePosition(Particle_s[] particles)
        {
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize; i++)
                {
                    int BallID = j * gridSize + i;
                    vertices[BallID].position = particles[BallID].currentPosition;
                }
        }

        public Vertex_s[] getVertices()
        {
            return vertices;
        }
    }
}
