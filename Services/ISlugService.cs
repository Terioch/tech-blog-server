namespace TechBlog.Services
{
    public interface ISlugService
    {
        public string Slugify(string value);
        public bool IsUnique(string slug);      
    }
}
