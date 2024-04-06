using Azure;
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Bloggie.Web.Controllers
{

    [Authorize (Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> SubmitTag(AddTagRequest addTagRequest) {
            
            // Mapping AddtagRequest to Tag Domain Model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
        };
           await  tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }


        
        [HttpGet]
        [ActionName("List")]
        public async Task <IActionResult> List()
        {
            // use DbContext to read the data
          var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }


        
        [HttpGet]
        public async  Task <IActionResult> Edit(Guid id )
        {

           var tag = await tagRepository.GetAsync(id);

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
        public async Task <IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var Tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

       var updatedTag =  await tagRepository.UpdateAsync(Tag);

            if (updatedTag != null)
            {
                //show success notification
            }
            else
            {
                // shpw failure notification
            }
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }


        
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
        var DeletedTag =   await  tagRepository.DeleteAsync(editTagRequest.Id);
            
            if(DeletedTag != null)
            {
                // show succes notification
                return RedirectToAction("List");
            }
            else
            {
                //failure notification
            }

            return RedirectToAction("Edit" , new {id = editTagRequest.Id});
        }
    }
}
