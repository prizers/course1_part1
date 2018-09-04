using System;

namespace DistanceTask
{
    public static class DistanceTask
    {
        static double SquareOfDistanceToPoint(double ax, double ay, double x, double y)
        {
            var dx = x - ax;
            var dy = y - ay;
            return dx * dx + dy * dy;
        }
        static double DistanceToPoint(double ax, double ay, double x, double y) =>
             Math.Sqrt(SquareOfDistanceToPoint(ax, ay, x, y));

        static double DistanceToLine(double ax, double ay, double bx, double by, double x, double y)
        {
            var dx = bx - ax;
            var dy = by - ay;
            return Math.Abs(dy * x - dx * y + bx * ay - by * ax) / Math.Sqrt(dx * dx + dy * dy);
        }

        // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
        public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
        {
            if (ax == bx && ay == by) return DistanceToPoint(ax, ay, x, y); // вырожденный случай
            var distance0 = SquareOfDistanceToPoint(ax, ay, bx, by);
            var distance1 = SquareOfDistanceToPoint(ax, ay, x, y);
            var distance2 = SquareOfDistanceToPoint(bx, by, x, y);
            if (distance0 + distance1 <= distance2)
            { // треугольник c первым тупым углом ==> ближайшая точка (ax,ay)
                return Math.Sqrt(distance1);
            }
            if (distance0 + distance2 <= distance1)
            { // треугольник c вторым тупым углом ==> ближайшая точка (bx,by)
                return Math.Sqrt(distance2);
            }
            // нормаль лежит между (ax,ay) и (bx,by) - ищем как расстояние до прямой
            return DistanceToLine(ax, ay, bx, by, x, y);
        }
    }
}