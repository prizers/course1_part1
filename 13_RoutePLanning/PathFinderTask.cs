using System;
using System.Drawing;

namespace RoutePlanning
{
    public static class PathFinderTask
    {
        public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
        {
            var size = checkpoints.Length;
            var weights = new double[size, size];
            for (var i = 0; i < size; ++i)
            {
                for (var j = 0; j < i; ++j)
                {
                    weights[i, j] = weights[j, i] = checkpoints[i].DistanceTo(checkpoints[j]);
                }
            }
            var bestPath = new int[size];
            var bestWeight = double.MaxValue;
            var path = new int[size];
            path[0] = 0;
            Walk(weights, path, 1, 0.0, bestPath, ref bestWeight);
            return bestPath;
        }

        private static void Walk(double[,] weights,
                                 int[] path, int currentStep, double currentWeight,
                                 int[] bestPath, ref double bestWeight)
        {
            var numSteps = path.Length;
            if (numSteps <= currentStep)
            { // раз мы добрались до конца - мы победители!
                Array.Copy(path, bestPath, numSteps);
                bestWeight = currentWeight;
            }
            else
            {
                for (var nextCheckpoint = 0; nextCheckpoint < numSteps; ++nextCheckpoint)
                {
                    if (Array.IndexOf(path, nextCheckpoint, 0, currentStep) == -1)
                    {
                        var currentCheckpoint = path[currentStep - 1];
                        path[currentStep] = nextCheckpoint;
                        var nextWeight = currentWeight + weights[currentCheckpoint, nextCheckpoint];
                        if (bestWeight <= nextWeight) continue; // уже проиграли. нет смысла углубляться
                        Walk(weights, path, currentStep + 1, nextWeight, bestPath, ref bestWeight);
                    }
                }
            }
        }
    }
}