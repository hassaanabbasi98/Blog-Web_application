using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitTag(AddTagRequest addTagRequest) {
            
            // Mapping AddtagRequest to Tag Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
        };

          bloggieDbContext.Tags.Add(tag);
          bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            // use DbContext to read the data
          var tags =  bloggieDbContext.Tags.ToList();

            return View(tags);
        }
        [HttpGet]
        public IActionResult Edit(Guid id )
        {

            var tag = bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);
            if(tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var Tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag = bloggieDbContext.Tags.Find(Tag.Id);

            if(Tag.Id != null)
            {
                existingTag.Name = Tag.Name;
                existingTag.DisplayName = Tag.DisplayName;
                bloggieDbContext.SaveChanges();
                // show succes notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
    
            }

            // shpw failure notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
           var tag = bloggieDbContext.Tags.Find(editTagRequest.Id);

            if(tag !=null)
            {
                bloggieDbContext.Tags.Remove(tag);
                bloggieDbContext.SaveChanges();

                // show succes notification
                return RedirectToAction("List");
            }
            // non - succes notification
            return RedirectToAction("Edit" , new {id = editTagRequest.Id});
        }
    }
}
