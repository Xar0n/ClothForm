using OpenTK;

namespace ClothForm
{
    public struct Particle_s
    {
        public Vector3 currentPosition;
        public Vector3 currentVelocity;
        public Vector3 nextPosition;
        public Vector3 nextVelocity;
        public Vector3 tension;
        public float inverseMass;
        public bool pinned; // истина, если частица закреплена / зафиксирована в позиции
    }
    class Particle
    {
        private int gridSize;
        private Particle_s[] particles;
        private float mass;
        private float clothScale;
        public Particle(int gridSize, float mass, float clothScale)
        {
            this.gridSize = gridSize;
            int particleCount = gridSize * gridSize;
            particles = new Particle_s[particleCount];
            this.mass = mass;
            this.clothScale = clothScale;
        }

        //Инициализируем частицы в равномерно распределенной сетке в плоскости x-z
        public void initInMesh()
        {
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize; i++)
                {
                    float U = (i / (float)(gridSize - 1)) - 0.5f;
                    float V = (j / (float)(gridSize - 1)) - 0.5f;
                    int BallID = j * gridSize + i;
                    particles[BallID].currentPosition = new Vector3(clothScale * U, 8, clothScale * V);
                    particles[BallID].currentVelocity = Vector3.Zero;
                    particles[BallID].inverseMass = 1.0f / mass;
                    particles[BallID].pinned = false;
                    particles[BallID].tension = Vector3.Zero;
                }
        }

        public void calculateTension(Spring_s[] springs)
        {
            for (int i = 0; i < springs.Length; i++)
            {
                Vector3 tensionDirection = (particles[springs[i].P1].currentPosition - particles[springs[i].P2].currentPosition);
                float springLength = tensionDirection.LengthFast;
                float extension = springLength - springs[i].NaturalLength;
                float tension = springs[i].Stiffness * (extension * springs[i].InverseLength);
                tensionDirection *= (float)(tension / springLength);
                particles[springs[i].P2].tension += tensionDirection;
                particles[springs[i].P1].tension -= tensionDirection;
            }
        }

        public void replaceCurrentNew()
        {
            //Меняем местами указатели текущих частиц и новых частиц
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].currentPosition = particles[i].nextPosition;
                particles[i].currentVelocity = particles[i].nextVelocity;
                particles[i].tension = Vector3.Zero;
            }
        }

        public void pin(int index)
        {
            particles[index].pinned = !particles[index].pinned;
        }

        public Particle_s[] getParticles()
        {
            return particles;
        }
    }
}
