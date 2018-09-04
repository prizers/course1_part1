using System;
using System.Linq;

namespace Names
{
    internal static class HistogramTask
    {
        /*
        Напишите код, готовящий данные для построения гистограммы 
        — количества людей в выборке c заданным именем в зависимости от числа 
        (то есть номера дня в месяце) их рождения.
        Не учитывайте тех, кто родился 1 числа любого месяца.
        Если вас пугает незнакомое слово гистограмма — вам, как обычно, в википедию! 
        http://ru.wikipedia.org/wiki/%D0%93%D0%B8%D1%81%D1%82%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%B0

        В качестве подписей (label) по оси X используйте число месяца.

        Объясните наблюдаемый результат!

        Пример подготовки данных для гистограммы смотри в файле HistogramSample.cs
        */

        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            // FIXME:
            // автоматический валидатор, похоже, const распознавать не обучен
            // и матерится на заглавные буквы в начале их названий
            const int numDays = 31; // размеры возвращаемых массивов. считаем, что дней 31 
            const int firstDay = 2; // учитываемый диапазон дней месяца: [2..31]
            const int lastDay = 31;

            // метки делаем для всех дней недели. 
            var labels = new string[numDays];
            for (int i = 0; i < numDays; ++i) labels[i] = (i + 1).ToString();
            var values = new double[numDays];
            foreach (var item in names)
            {
                if (item.Name == name &&
                    firstDay <= item.BirthDate.Day &&
                    item.BirthDate.Day <= lastDay)
                {
                    ++values[item.BirthDate.Day - 1];
                }
            }
            return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name),
                                     labels, values);
        }
    }
}