using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace TechBlog.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class PostCommentController : ControllerBase
    {
        public ActionResult<int> Get()
        {
            return 0;
        }
    }
}
