using OpenTK;

namespace ClothForm
{
    public class Cloth
    {
        private Triangle triangle;
        private Vertex vertex;
        private Particle particle;
        public Spring spring;
        private const int simScale = 1;
        private const float minimumPhysicsDelta = 0.01f;
        private const float clothScale = 20.0f;       
        private float stretchStiffness = 2.5f * clothScale;
        private float bendStiffness = 1.0f * clothScale;
        private float mass = 0.01f * simScale;
        private float dampFactor = 0.9f;
        public int gridSize = 13 * simScale;
        private Spring_s[] springs;
        private Particle_s[] particles;
        private float timeSinceLastUpdate;
        public Vector3 gravity;
        private Sphere_s sphere = new Sphere_s();
        private Vertex_s[] vertices;
        public Vertex_s[] getVertices { get { return vertices; } }
        private Triangle_s[] triangles;
        public Triangle_s[] getTriangles { get { return triangles; } }
        public Particle_s[] getParticles { get { return particles; } }
        public Spring_s[] getSprings { get { return springs; } }
        public Cloth()
        {
            gravity = new Vector3(0, -0.98f * simScale, 0);
            particle = new Particle(gridSize, mass, clothScale);
            triangle = new Triangle(gridSize);
            vertex = new Vertex(gridSize);
            spring = new Spring(gridSize,stretchStiffness, bendStiffness);
            particles = particle.getParticles();
            springs = spring.getSprings();
            this.initMesh();
            this.reset();
        }

        public Cloth(float clothScale, int gridSize, float stretchStiffness, float bendStiffness)
        {
            this.stretchStiffness = stretchStiffness * clothScale;
            this.bendStiffness = bendStiffness * clothScale;
            this.gridSize = gridSize * simScale;
            gravity = new Vector3(0, -0.98f * simScale, 0);
            particle = new Particle(this.gridSize, mass, clothScale);
            triangle = new Triangle(this.gridSize);
            vertex = new Vertex(this.gridSize);
            spring = new Spring(this.gridSize, this.stretchStiffness, this.bendStiffness);
            particles = particle.getParticles();
            springs = spring.getSprings();
            this.initMesh();
            this.reset();
        }

        private void initMesh()
        {
            triangle.calculateSides();
            triangles = triangle.getTriangles();
            vertex.calculateVertices();
            vertices = vertex.getVertices();
        }

        public void reset()
        {
            particle.initInMesh();
            particle.pin(0);
            particle.pin(gridSize - 1);
            spring.init(particles);
            updateMesh();
        }

        private void updateMesh()
        {
            triangle.calculateNormals(getVertices);
            vertex.calculateNormal(getTriangles);
            vertex.calculatePosition(particles);
        }

        public void simulate(float deltaTime)
        {
            if (deltaTime <= 0) return;
            timeSinceLastUpdate += deltaTime;
            bool updateMade = false;
            float timePassedInSeconds = minimumPhysicsDelta;
            while (timeSinceLastUpdate > minimumPhysicsDelta) {
                timeSinceLastUpdate -= minimumPhysicsDelta;
                updateMade = true;
                particle.calculateTension(springs);
                for (int i = 0; i < particles.Length; i++) {
                    if (particle.checkPin(i)) continue;
                    Vector3 force = gravity + particles[i].tension;
                    Vector3 acceleration = force * particles[i].inverseMass;
                    particles[i].nextVelocity = particles[i].currentVelocity + (acceleration * timePassedInSeconds);
                    particles[i].nextVelocity *= dampFactor;
                    force = particles[i].nextVelocity * timePassedInSeconds;
                    particles[i].nextPosition = particles[i].currentPosition + force;
                    particle.checkSphere(sphere, i);
                    particle.checkFloor(i);
                }
                particle.replaceCurrentNew();
            }
            if (updateMade) updateMesh();
        }

        public void addSphere(Vector3 position, float radius)
        {
            var col = new Sphere_s(position, radius);
            sphere = col;
        }

        public void changePosistionSphere(Vector3 position)
        {
            sphere.Position = position;
        }

        public void pinParticle(int index)
        {
            particle.pin(index);
        }

        public void setStretchStiffness(float koef)
        {
            stretchStiffness = koef * clothScale;
        }

        public void setBendStiffness(float koef)
        {
            bendStiffness = koef * clothScale;
        }

        public float getStretchStiffness()
        {
            return (stretchStiffness / clothScale);
        }
        public float getBendStiffness()
        {
            return (bendStiffness / clothScale);
        }
    }
}