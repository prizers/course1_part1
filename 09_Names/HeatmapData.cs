using System.Linq;

namespace Names
{
    public class HeatmapData
    {
        public HeatmapData(string title, double[,] heat, string[] xLabels, string[] yLabels)
        {
            XLabels = xLabels;
            YLabels = yLabels;
            Title = title;
            Heat = heat;
        }

        public string[] XLabels { get; }
        public string[] YLabels { get; }
        public string Title { get; }
        public double[,] Heat { get; }

        public bool Equals(HeatmapData other)
        {
            return Enumerable.Range(0, 2)
                       .All(dimension =>
                           Heat.GetLength(dimension) == other.Heat.GetLength(dimension))
                   && Heat
                       .Cast<double>()
                       .SequenceEqual(other.Heat
                           .Cast<double>());
        }
    }
}