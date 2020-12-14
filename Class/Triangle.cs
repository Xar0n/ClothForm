using OpenTK;

namespace ClothForm
{
    public struct Triangle_s
    {
        public int A;
        public int B;
        public int C;
        public Vector3 normal;
    }
    class Triangle
    {
        private int triangleCount;
        private Triangle_s[] triangles;
        private int gridSize;
        public Triangle(int gridSize)
        {
            this.gridSize = gridSize;
            triangleCount = (gridSize * gridSize) * 2;
            triangles = new Triangle_s[triangleCount];
        }
        
        public void calculateSides()
        {
            int k = 0;
            for (int j = 0; j < gridSize - 1; j++)
            {
                for (int i = 0; i < gridSize - 1; i++)
                {
                    var i0 = j * gridSize + i;
                    var i1 = j * gridSize + i + 1;
                    var i2 = (j + 1) * gridSize + i;
                    var i3 = (j + 1) * gridSize + i + 1;
                    triangles[k].A = i2;
                    triangles[k].B = i1;
                    triangles[k].C = i0;
                    k++;
                    triangles[k].A = i2;
                    triangles[k].B = i3;
                    triangles[k].C = i1;
                    k++;
                }
            }
        }

        private Vector3 CalculateNormal(Vector3 VA, Vector3 VB, Vector3 VC)
        {
            Vector3 a, b;
            a = VB - VA;
            b = VC - VA;
            var normal = Vector3.Cross(a, b);
            normal.Normalize();
            return normal;
        }

        public void calculateNormals(Vertex_s[] vertices)
        {
            for (int i = 0; i < triangles.Length; i++)
            {
                var t = triangles[i];
                var A = vertices[t.A].position;
                var B = vertices[t.B].position;
                var C = vertices[t.C].position;
                triangles[i].normal = CalculateNormal(A, B, C);
            }
        }

        public Triangle_s[] getTriangles()
        {
            return triangles;
        }
    }
}
