namespace Recognizer
{
    public class Line
    {
        public readonly int X0;
        public readonly int X1;
        public readonly int Y0;
        public readonly int Y1;

        public Line(int x0, int y0, int x1, int y1)
        {
            X1 = x1;
            X0 = x0;
            Y1 = y1;
            Y0 = y0;
        }
    }
}