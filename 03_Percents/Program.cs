using System;

namespace Percents
{
    class Program
    {
        public static double Calculate(string userInput)
        {
            string[] parts = userInput.Split(' ');
            double amount = double.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
            double interest = double.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
            int months = int.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture);
            return CalculateFinalAmount(amount, interest, months);
        }
        
        // вычисление суммы на счёте при помесячной капитализации сложных процентах
        static double CalculateFinalAmount(double initialAmount, double interest, int months)
        {
            var finalAmount = initialAmount;
            var monthlyRate = 1.0 + interest / 100.0 / 12.0; // коэффициент ежемесячного обогащения =)
            for (int month = 0; month < months; ++month)
            {
                finalAmount *= monthlyRate;
            }
            var fastVal = initialAmount * Math.Pow(monthlyRate, months);
            Console.WriteLine($"iter={finalAmount}, pow={fastVal}");
            return finalAmount;
        }

        static void Test(string userInput)
        {
            Console.WriteLine($"Calculate({userInput}) = {Calculate(userInput)}");
        }

        static void Main(string[] args)
        {
            Test("1000.0 5.0 24");
            Test("1000.0 10.0 24");
            Test("1000.0 15.0 240");
        }
    }
}
