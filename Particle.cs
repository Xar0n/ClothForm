using OpenTK;
using System.Collections.Generic;
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
        private static int particleCount;
        private int gridSize;
        private Particle_s[] particles;
        private float mass;
        private float clothScale;
        public Particle(int gridSize, float mass, float clothScale)
        {
            this.gridSize = gridSize;
            particleCount = gridSize * gridSize;
            particles = new Particle_s[particleCount];
            this.mass = mass;
            this.clothScale = clothScale;
        }

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

        public void checkSphere(Sphere_s colliders, int index)
        {
            Vector3 P = particles[index].nextPosition - colliders.Position;
            float cR = colliders.Radius * 1.08f; // длина образующей поверхности 1,05 https://life-prog.ru/1_43663_bokovoy-poverhnosti-kolodki.html
            if (P.LengthSquared < cR * cR) {
                P.Normalize();
                P *= cR;
                particles[index].nextPosition = P + colliders.Position;
                particles[index].nextVelocity = Vector3.Zero;
            }
        }

        public void checkFloor(int index)
        {
            Vector3 P = particles[index].nextPosition;
            if (P.Y < -8.5f) {
                particles[index].nextPosition.Y = -8.5f;
                particles[index].nextVelocity = Vector3.Zero;
            }
        }

        public bool checkPin(int index)
        {
            if (particles[index].pinned)
            {
                particles[index].nextPosition = particles[index].currentPosition;
                particles[index].nextVelocity = Vector3.Zero;
                return true;
            }
            return false;
        }
        public Particle_s[] getParticles()
        {
            return particles;
        }

        public static int getCount()
        {
            return particleCount;
        }
    }
}
