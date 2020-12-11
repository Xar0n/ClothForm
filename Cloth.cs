using OpenTK;
using System.Collections.Generic;

namespace ClothForm
{
    public class Cloth
    {
        private Triangle triangle;
        private Vertex vertex;
        private Particle particle;
        private Spring spring;
        private const int simScale = 1;
        private const float minimumPhysicsDelta = 0.01f;
        //Размер тканевой сетки
        private const float clothScale = 20.0f; //10        
        //Значения данные каждой пружине
        private float stretchStiffness = 2.5f * clothScale; //Жесткость при растяжении
        private float bendStiffness = 1.0f * clothScale; //Жесткость при сгибании
        private float mass = 0.01f * simScale;
        //Коэффициент демпфирования. Скорость умножается на это
        private float dampFactor = 0.9f;
        public const int gridSize = 13 * simScale;
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

        public Cloth(float StretchStiffnessSpring, float BendStiffnessSpring, Vector3 grav)
        {
            stretchStiffness = StretchStiffnessSpring * clothScale; //Жесткость при растяжении
            bendStiffness = BendStiffnessSpring * clothScale; //Жесткость при сгибании
            gravity = new Vector3(0, -0.98f * simScale, 0);
            // Подсчитываем количество пружин
            // Есть пружина, указывающая вправо для каждого шара, который не находится на правом краю,
            // и пружина направлена ​​вниз для каждого шара не на нижнем крае
            int springCount = (gridSize - 1) * gridSize * 2;
            // Пружина направлена ​​вниз и вправо для каждого шара не снизу или справа, 
            //и одна пружина направлена ​​вниз и влево для каждого шара не снизу или слева
            springCount += (gridSize - 1) * (gridSize - 1) * 2;
            // Имеется пружина, указывающая вправо (к следующему, кроме одного шара) 
            //для каждого шара, который не находится на правом краю или рядом с ним, 
            //и одна направленная вниз для каждого шара, не находящегося на нижнем крае или рядом с ним
            springCount += (gridSize - 2) * gridSize * 2;
            //Создание пространства для частиц и пружин
            springs = new Spring_s[springCount];
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
            //Закрепляет верхнюю левую и верхнюю правую частицы на месте
            particle.pin(0);
            particle.pin(gridSize - 1);
            //Закрепляет нижнюю левую и нижнюю правую частицы
            //particle.pin(gridSize * (gridSize - 1));
            //particle.pin(gridSize * gridSize - 1);
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
            //Обновляет физику с интервалом в 10 мс, чтобы предотвратить проблемы с разной частотой кадров, вызывающие разное затухание
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
    }
}