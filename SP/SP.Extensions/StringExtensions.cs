namespace SP.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNumberOrEmpty(this string value)
        {
            double v;
            return string.IsNullOrEmpty(value) || double.TryParse(value, out v);
        }

        public static bool IsNumber(this string value)
        {
            double v;
            return double.TryParse(value, out v);
        }

        public static double ToNumber(this string value)
        {
            return double.Parse(value);
        }

        public static bool IsNumberAndInRange(this string value, double min, double max)
        {
            if (!IsNumber(value))
            {
                return false;
            }

            var number = value.ToNumber();

            return number >= min && number <= max;
        }
    }
}
