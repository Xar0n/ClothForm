using OpenTK;
using System.Collections.Generic;

namespace ClothForm
{
    class Cloth
    {
        private Triangle triangle;
        private Vertex vertex;
        private Particle particle;
        private Spring spring;
        private const int SimScale = 1;
        private const float minimumPhysicsDelta = 0.01f;
        //Размер тканевой сетки
        private const float clothScale = 20.0f; //10        
        //Значения данные каждой пружине
        private float StretchStiffness = 2.5f * clothScale; //Жесткость при растяжении
        private float BendStiffness = 1.0f * clothScale; //Жесткость при сгибании
        private float mass = 0.01f * SimScale;
        //Коэффициент демпфирования. Скорость умножается на это
        private float dampFactor = 0.9f;
        public const int gridSize = 13 * SimScale;
        private Spring_s[] springs;
        private Particle_s[] particles;
        private float timeSinceLastUpdate;
        private Vector3 gravity;
        private List<Sphere_s> _colliders = new List<Sphere_s>();
        private Vertex_s[] _vertices;
        public Vertex_s[] vertices { get { return _vertices; } }
        private Triangle_s[] _triangles;
        public Triangle_s[] triangles { get { return _triangles; } }
        public Cloth()
        {
            gravity = new Vector3(0, -0.98f * SimScale, 0);
            particle = new Particle(gridSize, mass, clothScale);
            triangle = new Triangle(gridSize);
            vertex = new Vertex(gridSize);
            spring = new Spring(gridSize,StretchStiffness, BendStiffness);
            particles = particle.getParticles();
            springs = spring.getSprings();
            this.InitMesh();
            this.Reset();
        }

        public Cloth(float StretchStiffnessSpring, float BendStiffnessSpring, Vector3 grav)
        {
            StretchStiffness = StretchStiffnessSpring * clothScale; //Жесткость при растяжении
            BendStiffness = BendStiffnessSpring * clothScale; //Жесткость при сгибании
            gravity = new Vector3(0, -0.98f * SimScale, 0);
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
            this.InitMesh();
            this.Reset();
        }

        private void InitMesh()
        {
            triangle.calculateSides();
            _triangles = triangle.getTriangles();
            vertex.calculateVertices();
            _vertices = vertex.getVertices();
        }

        public void Reset()
        {
            particle.initInMesh();
            //Закрепляет верхнюю левую и верхнюю правую частицы на месте
           // particle.pin(0);
           // particle.pin(gridSize - 1);
            //Закрепляет нижнюю левую и нижнюю правую частицы
          //  particle.pin(gridSize * (gridSize - 1));
         //   particle.pin(gridSize * gridSize - 1);
            spring.init(particles);
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            triangle.calculateNormals(vertices);
            vertex.calculateNormal(triangles);
            vertex.calculatePosition(particles);
        }

        public void Simulate(float deltaTime)
        {
            //Если время не прошло ничего не обновлять
            if (deltaTime <= 0) return;
            //Обновляет физику с интервалом в 10 мс, чтобы предотвратить проблемы с разной частотой кадров, вызывающие разное затухание
            timeSinceLastUpdate += deltaTime;
            bool updateMade = false;
            float timePassedInSeconds = minimumPhysicsDelta;
            while (timeSinceLastUpdate > minimumPhysicsDelta) {
                timeSinceLastUpdate -= minimumPhysicsDelta;
                updateMade = true;
                particle.calculateTension(springs);
                //Вычисляет следующие частицы из текущих частиц

                for (int i = 0; i < particles.Length; i++) {
                    //Если шар зафиксирован, перенести положение и обнулить скорость, в противном случае рассчитать новые значения.
                    if (particle.checkPin(i)) continue;
                    Vector3 force = gravity + particles[i].tension;
                    Vector3 acceleration = force * particles[i].inverseMass;
                    //Обновление скорости
                    particles[i].nextVelocity = particles[i].currentVelocity + (acceleration * timePassedInSeconds);
                    //Демпфируем скорость
                    particles[i].nextVelocity *= dampFactor;
                    //Рассчитываем новую позицию
                    force = particles[i].nextVelocity * timePassedInSeconds;
                    particles[i].nextPosition = particles[i].currentPosition + force;
                   // particle.checkSphere(_colliders, i);
                    particle.checkFloor(_colliders, i);
                }
                particle.replaceCurrentNew();
            }
            if (updateMade) UpdateMesh();
        }

        public void addSphere(Vector3 position, float radius)
        {
            var col = new Sphere_s(position, radius);
            _colliders.Add(col);
        }

        public void pinParticle(int index)
        {
            particle.pin(index);
        }
    }
}
