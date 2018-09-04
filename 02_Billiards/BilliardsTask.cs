using System;

namespace Billiards
{
    public static class BilliardsTask
    {
        // рассматриваем треугольник, где A - исходное направление, B - угол стены
        // B' - внутренний треугольника == дополнение до B: B' = Pi - B
        // С - третий угол треугольника = Pi - (A + B') == Pi - (A - Pi - B) == B - A
        // D - угол отражения = B + C == 2 * B - A
        public static double BounceWall(double directionRadians, double wallInclinationRadians) =>
            wallInclinationRadians * 2 - directionRadians;
    }
}
