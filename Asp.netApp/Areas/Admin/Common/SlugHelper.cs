using System.Text.RegularExpressions;

namespace Asp.netApp.Areas.Admin.Common
{
    public class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
            {
                return string.Empty;
            }

            // Convert to lowercase
            string str = phrase.ToLowerInvariant();

            // Replace accented characters
            str = Regex.Replace(str, @"[àáâãäå]", "a");
            str = Regex.Replace(str, @"[èéêë]", "e");
            str = Regex.Replace(str, @"[ìíîï]", "i");
            str = Regex.Replace(str, @"[òóôõö]", "o");
            str = Regex.Replace(str, @"[ùúûü]", "u");
            str = Regex.Replace(str, @"[ç]", "c");
            str = Regex.Replace(str, @"[ñ]", "n");

            // Remove invalid characters
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // Replace spaces with hyphens
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }
    }
}
