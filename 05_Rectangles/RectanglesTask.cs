using System;

namespace Rectangles
{
    public static class RectanglesTask
    {
        static Rectangle Intersect(Rectangle r1, Rectangle r2)
        {
            var left = Math.Max(r1.Left, r2.Left);
            var top = Math.Max(r1.Top, r2.Top);
            var right = Math.Min(r1.Right, r2.Right);
            var bottom = Math.Min(r1.Bottom, r2.Bottom);
            if (right < left || bottom < top) return null;
            return new Rectangle(left, top, right - left, bottom - top);
        }

        static bool Equals(Rectangle r1, Rectangle r2)
        {
            return r1 != null && r2 != null &&
                r1.Left == r2.Left && r1.Top == r2.Top &&
                r1.Right == r2.Right && r1.Bottom == r2.Bottom;
        }

        // Пересекаются ли два прямоугольника 
        // (пересечение только по границе также считается пересечением)
        public static bool AreIntersected(Rectangle r1, Rectangle r2)
        {
            return Intersect(r1, r2) != null;
        }

        // Площадь пересечения прямоугольников
        public static int IntersectionSquare(Rectangle r1, Rectangle r2)
        {
            var intersection = Intersect(r1, r2);
            return intersection == null ? 0 : intersection.Width * intersection.Height;
        }

        // проверяем вложенность одного прямоугольника в другой
        static bool IsNested(Rectangle exterior, Rectangle interior)
        {
            return exterior.Left <= interior.Left &&
                   exterior.Top <= interior.Top &&
                   interior.Right <= exterior.Right &&
                   interior.Bottom <= exterior.Bottom;
        }

        // Если один из прямоугольников целиком находится внутри другого 
        // — вернуть номер (с нуля) внутреннего.
        // Иначе вернуть -1
        // Если прямоугольники совпадают, можно вернуть номер любого из них.
        public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
        {
            return IsNested(r1, r2) ? 1 : IsNested(r2, r1) ? 0 : -1;
        }
    }
}