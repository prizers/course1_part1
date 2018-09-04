namespace Pluralize
{
    public static class PluralizeTask
    {
        public static string PluralizeRubles(int count)
        {
            var ones = count % 10;
            var tens = (count / 10) % 10;
            if ((0 == ones) || (4 < ones) || (1 == tens)) return "рублей"; // 0, 4..9, 10..19
            else if (1 < ones) return "рубля"; // 2,3,4
            else return "рубль"; // 1
        }
    }
}