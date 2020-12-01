using OpenTK;
using System;
using System.Collections.Generic;

namespace ClothForm
{
    public struct Triangle
    {
        public int A;
        public int B;
        public int C;
        public Vector3 normal;
    }

    public struct Vertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 uv;
    }

    public struct ClothSpring
    {
        //Индексы частиц на обоих концах пружины
        public int P1;
        public int P2;

        public float NaturalLength;
        public float InverseLength;

        public float _Stiffness;

        public ClothSpring(int PID1, int PID2, float Len, float Stiffness)
        {
            this.P1 = PID1;
            this.P2 = PID2;
            this.NaturalLength = Len;
            this.InverseLength = 1.0f / Len;
            this._Stiffness = Stiffness;
        }
    }

    public struct ClothParticle
    {
        public Vector3 currentPosition;
        public Vector3 currentVelocity;
        public Vector3 nextPosition;
        public Vector3 nextVelocity;
        public Vector3 tension;
        public float inverseMass;
        public bool pinned; // истина, если частица закреплена / зафиксирована в позиции
    }

    public struct ClothCollider
    {
        public Vector3 Position;
        public float Radius;

        public ClothCollider(Vector3 position, float radius)
        {
            this.Position = position;
            this.Radius = radius;
        }
    }

    public class Cloth
    {
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
        public const int gridSize = 13 * SimScale;// 13-
        private ClothSpring[] springs;
        private ClothParticle[] particles;
        private float _timeSinceLastUpdate;
        private Vector3 _gravity;
        private List<ClothCollider> _colliders = new List<ClothCollider>();
        private Vertex[] _vertices;
        public Vertex[] vertices { get { return _vertices; } }
        private Triangle[] _triangles;
        public Triangle[] triangles { get { return _triangles; } }
        public Cloth()
        {
            _gravity = new Vector3(0, -0.98f * SimScale, 0);
            //Рассчитать количество частиц
            int particleCount = gridSize * gridSize;
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
            particles = new ClothParticle[particleCount];
            springs = new ClothSpring[springCount];
            this.InitMesh();
            this.Reset();
        }

        public Cloth(float StretchStiffnessSpring, float BendStiffnessSpring, Vector3 grav)
        {
            StretchStiffness = StretchStiffnessSpring * clothScale; //Жесткость при растяжении
            BendStiffness = BendStiffnessSpring * clothScale; //Жесткость при сгибании
            _gravity = new Vector3(0, -0.98f * SimScale, 0);
            //Рассчитать количество частиц
            int particleCount = gridSize * gridSize;
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
            particles = new ClothParticle[particleCount];
            springs = new ClothSpring[springCount];
            this.InitMesh();
            this.Reset();
        }

        private void InitMesh()
        {
            int triangleCount = (gridSize * gridSize) * 2;
            _triangles = new Triangle[triangleCount];
            int vertexCount = (gridSize * gridSize);
            _vertices = new Vertex[vertexCount];
            int k = 0;
            //Считает стороны треугольников из которых состоит сетка
            for (int j = 0; j < gridSize - 1; j++) {
                for (int i = 0; i < gridSize - 1; i++) {
                    var i0 = j * gridSize + i;
                    var i1 = j * gridSize + i + 1;
                    var i2 = (j + 1) * gridSize + i;
                    var i3 = (j + 1) * gridSize + i + 1;
                    _triangles[k].A = i2;
                    _triangles[k].B = i1;
                    _triangles[k].C = i0;
                    k++;
                    _triangles[k].A = i2;
                    _triangles[k].B = i3;
                    _triangles[k].C = i1;
                    k++;
                }
            }
            //Назначаем координаты вершинам
            for (int j = 0; j < gridSize; j++) {
                for (int i = 0; i < gridSize; i++) {
                    int ballID = j * gridSize + i;
                    _vertices[ballID].uv = new Vector2(i / (float)gridSize, j / (float)gridSize);
                }
            }
        }

        public void Reset()
        {
            //Инициализируем частицы в равномерно распределенной сетке в плоскости x-z
            for (int j = 0; j < gridSize; j++) 
                for (int i = 0; i < gridSize; i++) {
                    float U = (i / (float)(gridSize - 1)) - 0.5f;
                    float V = (j / (float)(gridSize - 1)) - 0.5f;
                    int BallID = j * gridSize + i;
                    particles[BallID].currentPosition = new Vector3(clothScale * U, 8, clothScale * V);
                    particles[BallID].currentVelocity = Vector3.Zero;
                    particles[BallID].inverseMass = 1.0f / mass;
                    particles[BallID].pinned = false;
                    particles[BallID].tension = Vector3.Zero;
                }
            //Расстояние между частицами
            float naturalLength = (particles[0].currentPosition - particles[1].currentPosition).LengthFast;
            //Закрепляет верхнюю левую и верхнюю правую частицы на месте
            particles[0].pinned = true;
            particles[gridSize - 1].pinned = true;
            //Закрепляет нижнюю левую и нижнюю правую частицы
            particles[gridSize * (gridSize - 1)].pinned = true;
            particles[gridSize * gridSize - 1].pinned = true;
            //Инициализирует пружины
            int currentSpring = 0;
            //Первые (gridSize-1) * gridSize пружины переходят от одного шара к другому, за исключением тех, которые находятся на правом краю.
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize - 1; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, j * gridSize + i + 1, naturalLength, StretchStiffness);
                    currentSpring++;
                }
            //Следующие пружины (gridSize-1) * gridSize переходят от одного шара к следующему, за исключением тех, которые находятся на нижнем краю.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 0; i < gridSize; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, (j + 1) * gridSize + i, naturalLength, StretchStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-1) * (gridSize-1) переходят от шара к нижнему и правому, исключая те, которые находятся внизу или справа.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 0; i < gridSize - 1; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, (j + 1) * gridSize + i + 1, naturalLength * (float)Math.Sqrt(2.0f), BendStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-1) * (gridSize-1) переходят от шара к нижнему и левому, исключая те, которые находятся внизу или справа.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 1; i < gridSize; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, (j + 1) * gridSize + i - 1, naturalLength * (float)Math.Sqrt(2.0f), BendStiffness);
                    currentSpring++;
                }
            //Первые пружины (gridSize-2) * gridSize переходят от одного шара к следующему, кроме одного, за исключением тех, которые находятся на правом краю или рядом с ним.
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize - 2; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, j * gridSize + i + 2, naturalLength * 2, BendStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-2) * gridSize пружины переходят от одного шара к следующему, кроме одного ниже, за исключением тех, которые находятся на нижнем крае или рядом с ним.
            for (int j = 0; j < gridSize - 2; j++)
                for (int i = 0; i < gridSize; i++) {
                    springs[currentSpring] = new ClothSpring(j * gridSize + i, (j + 2) * gridSize + i, naturalLength * 2, BendStiffness);
                    currentSpring++;
                }
            UpdateMesh();
        }

        private static Vector3 CalculateNormal(Vector3 VA, Vector3 VB, Vector3 VC)
        {
            Vector3 a, b;
            a = VB - VA;
            b = VC - VA;
            var normal = Vector3.Cross(a, b);
            normal.Normalize();
            return normal;
        }

        private void UpdateMesh()
        {
            //Вычисляет нормали треугольников
            for (int i = 0; i < _triangles.Length; i++) {
                var t = _triangles[i];
                var A = vertices[t.A].position;
                var B = vertices[t.B].position;
                var C = vertices[t.C].position;
                _triangles[i].normal = CalculateNormal(A, B, C);
            }
            //Вычисляет нормали для текщих частиц
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize; i++) {
                    int BallID = j * gridSize + i;
                    Vector3 normal = Vector3.Zero;
                    int count = 0;
                    for (int Y = 0; Y <= 1; Y++)
                        for (int X = 0; X <= 1; X++) {
                            if (X + i < gridSize && Y + j < gridSize) {
                                int index = (j + Y) * gridSize + (i + X) * 2;
                                normal += _triangles[index].normal;
                                index++;
                                normal += _triangles[index].normal;
                                count += 2;
                            }
                        }
                    normal /= (float)count;
                    _vertices[BallID].normal = normal;
                }

            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize; i++)
                {
                    int BallID = j * gridSize + i;
                    _vertices[BallID].position = particles[BallID].currentPosition;
                }


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
                for (int i = 0; i < springs.Length; i++) {
                    Vector3 tensionDirection = (particles[springs[i].P1].currentPosition - particles[springs[i].P2].currentPosition);
                    float springLength = tensionDirection.LengthFast;
                    float extension = springLength - springs[i].NaturalLength;
                    float tension = springs[i]._Stiffness * (extension * springs[i].InverseLength);
                    tensionDirection *= (float)(tension / springLength);
                    particles[springs[i].P2].tension += tensionDirection;
                    particles[springs[i].P1].tension -= tensionDirection;
                }
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
                    //Forcse := VectorAdd(VectorScale(Particles[i].NextVelocity, 0.5), VectorScale(Particles[i].CurrentVelocity, 0.5));
                    force = particles[i].nextVelocity * timePassedInSeconds;
                    particles[i].nextPosition = particles[i].currentPosition + force;

                    //Check against floor
                    /* if(_Particles[i].NextPosition.y <= -sphereRadius* 0.92f) 
                    {
                          _Particles[i].NextPosition.y := -sphereRadius* 0.92f;
                            _Particles[i].NextVelocity.y := 0;
                            continue;
                    }*/

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
                //Меняем местами указатели текущих частиц и новых частиц
                for (int i = 0; i < particles.Length; i++) {
                    particles[i].currentPosition = particles[i].nextPosition;
                    particles[i].currentVelocity = particles[i].nextVelocity;
                    particles[i].tension = Vector3.Zero;
                }
            }
            //Обновляем сетку, если мы обновили позиции
            if (updateMade) UpdateMesh();
        }

        public void AddCollider(Vector3 position, float radius)
        {
            var col = new ClothCollider(position, radius);
            _colliders.Add(col);
        }

        public void UnpinParticle(int index)
        {
            particles[index].pinned = false;
        }
    }
}
