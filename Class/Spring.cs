using System;
namespace ClothForm
{
    public struct Spring_s
    {
        public int P1;
        public int P2;
        public float NaturalLength;
        public float InverseLength;
        public float Stiffness;

        public Spring_s(int P1, int P2, float NaturalLength, float Stiffness)
        {
            this.P1 = P1;
            this.P2 = P2;
            this.NaturalLength = NaturalLength;
            InverseLength = 1.0f / NaturalLength;
            this.Stiffness = Stiffness;
        }
    }
    public class Spring
    {
        private Spring_s[] springs;
        private int gridSize;
        private float StretchStiffness;
        private float BendStiffness;
        public Spring(int gridSize, float StretchStiffness, float BendStiffness)
        {
            this.gridSize = gridSize;
            int springCount = (gridSize - 1) * gridSize * 2;
            springCount += (gridSize - 1) * (gridSize - 1) * 2;
            springCount += (gridSize - 2) * gridSize * 2;
            springs = new Spring_s[springCount];
            this.StretchStiffness = StretchStiffness;
            this.BendStiffness = BendStiffness;
        }

        public void init(Particle_s[] particles)
        {
            //Расстояние между частицами
            float naturalLength = (particles[0].currentPosition - particles[1].currentPosition).LengthFast;
            //Инициализирует пружины
            int currentSpring = 0;
            //Первые (gridSize-1) * gridSize пружины переходят от одного шара к другому, за исключением тех, которые находятся на правом краю.
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize - 1; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, j * gridSize + i + 1, naturalLength, StretchStiffness);
                    currentSpring++;
                }
            //Следующие пружины (gridSize-1) * gridSize переходят от одного шара к следующему, за исключением тех, которые находятся на нижнем краю.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 0; i < gridSize; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, (j + 1) * gridSize + i, naturalLength, StretchStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-1) * (gridSize-1) переходят от шара к нижнему и правому, исключая те, которые находятся внизу или справа.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 0; i < gridSize - 1; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, (j + 1) * gridSize + i + 1, naturalLength * (float)Math.Sqrt(2.0f), BendStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-1) * (gridSize-1) переходят от шара к нижнему и левому, исключая те, которые находятся внизу или справа.
            for (int j = 0; j < gridSize - 1; j++)
                for (int i = 1; i < gridSize; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, (j + 1) * gridSize + i - 1, naturalLength * (float)Math.Sqrt(2.0f), BendStiffness);
                    currentSpring++;
                }
            //Первые пружины (gridSize-2) * gridSize переходят от одного шара к следующему, кроме одного, за исключением тех, которые находятся на правом краю или рядом с ним.
            for (int j = 0; j < gridSize; j++)
                for (int i = 0; i < gridSize - 2; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, j * gridSize + i + 2, naturalLength * 2, BendStiffness);
                    currentSpring++;
                }
            //Следующие (gridSize-2) * gridSize пружины переходят от одного шара к следующему, кроме одного ниже, за исключением тех, которые находятся на нижнем крае или рядом с ним.
            for (int j = 0; j < gridSize - 2; j++)
                for (int i = 0; i < gridSize; i++) {
                    springs[currentSpring] = new Spring_s(j * gridSize + i, (j + 2) * gridSize + i, naturalLength * 2, BendStiffness);
                    currentSpring++;
                }
        }

        public Spring_s[] getSprings()
        {
            return springs;
        }
    }
}
