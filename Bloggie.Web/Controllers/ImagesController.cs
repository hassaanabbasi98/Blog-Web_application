using Bloggie.Web.Respositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bloggie.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagesController : ControllerBase
    {
        private readonly  IIamgeRepository imageRepository;

        public ImagesController(IIamgeRepository iamgeRepository)
        {
            this.imageRepository = iamgeRepository; 
        }
        [HttpPost]
        public async Task <IActionResult> UploadAsync(IFormFile file)
        {
            //call the repository
         var imageURL =    await imageRepository.UploadAsync(file);

            if(imageURL == null)
            {
                return Problem("Image wasn't Uploaded", null , (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult( new {link = imageURL});
        }
    }
}
