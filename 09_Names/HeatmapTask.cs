using System;

namespace Names
{
    internal static class HeatmapTask
    {
        // генерация массива цифровых меток
        static string[] CreateLabels(int start, int count)
        {
            var labels = new string[count];
            for (int i = 0; i < count; i++) labels[i] = (i + start).ToString();
            return labels;
        }

        /*
		Подготовьте данные для построения карты интенсивностей, 
		у которой по оси X — число месяца, по Y — номер месяца, 
		а интенсивность точки (она отображается цветом и размером) 
		обозначает количество рожденных людей в это число этого месяца.
		Не учитывайте людей, родившихся первого числа любого месяца.

		В качестве подписей (label) по X используйте число месяца. 
		Поскольку данные за первые числа месяца учитывать не нужно,
		то начинайте подписи со второго числа.
		В качестве подписей по Y используйте номер месяца (январь — 1, февраль — 2, ...).

		Таким образом, данные для карты интенсивностей должны быть 
		в виде двумерного массива 30 на 12 —  от 2 до 31 числа и от января до декабря.
		*/
        public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
        {
            // месяцы по колонкам, дни по строкам
            const int numRows = 30; // [2..31]
            const int numCols = 12; // [1..12]
            const int firstDay = 2;
            var heatmap = new double[numRows, numCols];
            foreach (var item in names)
            {
                if (item.BirthDate.Day == 1) continue; // родившихся первого - не считаем
                ++heatmap[item.BirthDate.Day - firstDay, item.BirthDate.Month - 1];
            }
            return new HeatmapData("Пример карты интенсивностей",
                heatmap, CreateLabels(firstDay, numRows), CreateLabels(1, numCols));
        }
    }
}