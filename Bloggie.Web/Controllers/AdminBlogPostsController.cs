using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Respositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogPostsController : Controller
    {
        private ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from reposirtory
         var tags =   await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            // Map view model to domain model
            var blogpost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageURL = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible,

            };
            //Map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            { 
                var selectedTagIdGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdGuid);
                if(existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            // Maping tags to domain model
            blogpost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogpost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List ()
        {
            // Call the repository
           var blogPost = await blogPostRepository.GetAllAsync();

           
            return View(blogPost);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            //Retreive the result from the repository
            var blogpost = await blogPostRepository.GetAsync(Id);
            var tagsDomainModel = await tagRepository.GetAllAsync();


            if (blogpost != null)
            {
                //map the domain modek into the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogpost.Id,
                    Heading = blogpost.Heading,
                    PageTitle = blogpost.PageTitle,
                    Content = blogpost.Content,
                    Author = blogpost.Author,
                    FeaturedImageURL = blogpost.FeaturedImageURL,
                    UrlHandle = blogpost.UrlHandle,
                    ShortDescription = blogpost.ShortDescription,
                    PublishedDate = blogpost.PublishedDate,
                    Visible = blogpost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()

                    }),
                    SelectedTags = blogpost.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                //pass data to view
                return View(model);
            }
            else
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit (EditBlogPostRequest editBlogPostRequest)
        {
            //Mapping view model to domain Model to update the Blog changes made
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageURL = editBlogPostRequest.FeaturedImageURL,
                PublishedDate = editBlogPostRequest.PublishedDate,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible

            };
            var SelectedTags = new List<Tag>();  // updateing the select tags along with the BlogPost
            
                foreach(var selectedTag in editBlogPostRequest.SelectedTags)
                {
                    if(Guid.TryParse(selectedTag, out var tag))
                    {
                        var foundTag = await tagRepository.GetAsync(tag);
                        if(foundTag != null) {
                            SelectedTags.Add(foundTag);
                        }
                    }
                
            }

            blogPostDomainModel.Tags = SelectedTags;

            // submit information to repository to update
         var updatedBlog =   await  blogPostRepository.UpdateAsync(blogPostDomainModel);

            if(updatedBlog != null)
            {
                // show success notification
                return RedirectToAction("Edit");

            }
            //show error notification
            return RedirectToAction("Edit");
            //After Updating it will Redirect to Get
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to repository to delete this blog Post and Tags

            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id); // we are getting the blog id from the repository of database and then assinging it into the varuage
                if (deletedBlogPost != null)
            {
                //show succes notification
                return RedirectToAction("List");
            }

            //show error notification
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id });
        }

    }
}
