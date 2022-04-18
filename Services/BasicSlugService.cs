using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TechBlog.Services
{
    public class BasicSlugService : ISlugService
    {
        private readonly IPostDataService postRepo;

        public BasicSlugService(IPostDataService postRepo)
        {
            this.postRepo = postRepo;
        }

        public bool IsUnique(string slug)
        {
            var posts = postRepo.GetAllPosts();
            return posts.All(p => p.Slug != slug);
        }

        public string Slugify(string value)
        {
            // Remove all accents, convert to lowercase and trim
            value = RemoveAccents(value).ToLowerInvariant().Trim();           

            // Replace spaces with dashes
            value = string.Join("-", value.Split(" "));

            // Remove invalid characters
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            // Replace multiple consecutive occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }

        public static string RemoveAccents(string value)
        {          
            value = value.Normalize(NormalizationForm.FormD);
            char[] chars = value
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }
    }
}
