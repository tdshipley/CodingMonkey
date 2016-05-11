namespace CodingMonkey.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string LowercaseFirstLetter(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
