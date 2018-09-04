using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Autocomplete
{
    public class LeftBorderTask
    {
        /*
		начальные ограничения: -1 <= left <= right < phrases.Count
		возвращаемое значение лежит в диапазоне: [-1..phrases.Count)
		*/
        public static int GetLeftBorderIndex(IReadOnlyList<string> phrases, string prefix,
                                             int left, int right)
        {
            // откорректируем входные значения в случае их выхода за диапазон ограничений
            // в противном случае можем получить middle вне диапазона [0..phrases.Count)
            left = Math.Max(left, -1);
            right = Math.Min(right, phrases.Count - 1);
            if (right <= left) return left; // пустой диапазон
            var middle = left + (right - left + 1) / 2;
            if (string.Compare(phrases[middle], prefix, StringComparison.OrdinalIgnoreCase) < 0)
            { // pick < prefix -- уточним индекс в правом диапазоне
                return GetLeftBorderIndex(phrases, prefix, middle, right);
            }
            else
            { // prefix <= pick -- будем искать слева и без middle
                return GetLeftBorderIndex(phrases, prefix, left, middle - 1);
            }
        }
    }
}
