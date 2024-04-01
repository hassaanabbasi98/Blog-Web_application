using Bloggie.Web.Respositories;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogpostRepository;

        public BlogsController(IBlogPostRepository blogpostRepository)
        {
            this.blogpostRepository = blogpostRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string UrlHandle) // the read more url handle
        {
         var blogPost = await blogpostRepository.GetByUrlHandleAsync(UrlHandle);

            return View(blogPost);
        }
    }
}
