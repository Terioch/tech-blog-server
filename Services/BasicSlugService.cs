using System.Text;
using System.Text.RegularExpressions;

namespace TechBlog.Services
{
    public class BasicSlugService : ISlugService
    {
        public string Slugify(string value)
        {
            value = value.ToLowerInvariant().Trim();

            // Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            // Replace spaces with dashes
            value = string.Join("-", value.Split(" "));

            // Remove invalid characters
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Replace multiple consecutive occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
