using OpenTK;
using System;
using System.Collections.Generic;

namespace ClothForm
{
    public struct Collider_s
    {
        public Vector3 Position;
        public float Radius;

        public Collider_s(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
        }
    }

    public class Cloth
    {
        private Triangle triangle;
        private Vertex vertex;
        private Particle particle;
        private Spring spring;
        private const int SimScale = 1;
        private const float minimumPhysicsDelta = 0.01f; // в секундах
        //Размер тканевой сетки
        private const float clothScale = 20.0f; //10        
        //Значения данные каждой пружине
        private float StretchStiffness = 2.5f * clothScale; //Жесткость при растяжении
        private float BendStiffness = 1.0f * clothScale; //Жесткость при сгибании
        //Значения данные каждому шару
        private float mass = 0.01f * SimScale;
        //Коэффициент демпфирования. Скорость умножается на это
        private float dampFactor = 0.9f;
        //Сложность сетки. Это количество частиц поперек и вниз в модели.
        public const int gridSize = 13 * SimScale;
        private Spring_s[] springs;
        private Particle_s[] particles;
        private float _timeSinceLastUpdate;
        private Vector3 _gravity;
        private List<Collider_s> _colliders = new List<Collider_s>();
        private Vertex_s[] _vertices;
        public Vertex_s[] vertices { get { return _vertices; } }
        private Triangle_s[] _triangles;
        public Triangle_s[] triangles { get { return _triangles; } }
        public Cloth()
        {
            _gravity = new Vector3(0, -0.98f * SimScale, 0);
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
            _gravity = new Vector3(0, -0.98f * SimScale, 0);
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
            particle.pin(0);
            particle.pin(gridSize - 1);
            //Закрепляет нижнюю левую и нижнюю правую частицы
            particle.pin(gridSize * (gridSize - 1));
            particle.pin(gridSize * gridSize - 1);
            spring.init(particles);
            UpdateMesh();
        }

        private void UpdateMesh()
        {
            triangle.calculateNormals(vertices);
            vertex.calculateNormal(triangles);
            vertex.calculatePosition(particles);
            // TODO .UpdateBoundingBox();
        }

        public void Simulate(float deltaTime)
        {
            //Если время не прошло ничего не обновлять
            if (deltaTime <= 0) return;
            //Обновляет физику с интервалом в 10 мс, чтобы предотвратить проблемы с разной частотой кадров, вызывающие разное затухание
            _timeSinceLastUpdate += deltaTime;
            bool updateMade = false;//мы обновили позиции и т.д. на этот раз?
            float timePassedInSeconds = minimumPhysicsDelta;
            while (_timeSinceLastUpdate > minimumPhysicsDelta) {
                _timeSinceLastUpdate -= minimumPhysicsDelta;
                updateMade = true;
                //Рассчитывает натяжение пружин
                particle.calculateTension(springs);
                //Вычисляет следующие частицы из текущих частиц
                for (int i = 0; i < particles.Length; i++) {
                    //Если шар зафиксирован, перенести положение и обнулить скорость, в противном случае рассчитать новые значения.
                    if (particles[i].pinned) {
                        particles[i].nextPosition = particles[i].currentPosition;
                        particles[i].nextVelocity = Vector3.Zero;
                        // If MoveCloth Then _Particles[i].NextPosition.Add(VectorCreate(0, 2 * timePassedInSeconds, 5 * timePassedInSeconds));
                        continue;
                    }

                    //Рассчитываем силу, действующую на этот шар
                    Vector3 force = _gravity + particles[i].tension;
                    //Рассчитываем ускорение
                    Vector3 acceleration = force * (float)particles[i].inverseMass;
                    //Обновление скорости
                    particles[i].nextVelocity = particles[i].currentVelocity + (acceleration * timePassedInSeconds);
                    //Демпфируем скорость
                    particles[i].nextVelocity *= dampFactor;
                    //Рассчитываем новую позицию
                    force = particles[i].nextVelocity * timePassedInSeconds;
                    particles[i].nextPosition = particles[i].currentPosition + force;

                    //Проверяем против коллайдеров
                    for (int j = 0; j < _colliders.Count; j++) {
                        Vector3 P = particles[i].nextPosition - _colliders[j].Position;
                        float cR = _colliders[j].Radius * 1.08f; // длина образующей поверхности 1,05 https://life-prog.ru/1_43663_bokovoy-poverhnosti-kolodki.html
                        if (P.LengthSquared < cR * cR) {
                            P.Normalize();
                            P *= cR;
                            particles[i].nextPosition = P + _colliders[j].Position;
                            particles[i].nextVelocity = Vector3.Zero;
                            break;
                        }
                    }
                }
                particle.replaceCurrentNew();
            }
            //Обновляем сетку, если мы обновили позиции
            if (updateMade) UpdateMesh();
        }

        public void AddCollider(Vector3 position, float radius)
        {
            var col = new Collider_s(position, radius);
            _colliders.Add(col);
        }

        public void pinParticle(int index)
        {
            particle.pin(index);
        }
    }
}
