using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Respositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }



        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        async Task<BlogPost?> IBlogPostRepository.DeleteAsync(Guid id)
        {
         var existingBlog =     await bloggieDbContext.BlogPosts.FindAsync(id);
            
            if(existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog); // checking if there's a blog and if it's there remove the blog
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return existingBlog;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }


        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id != blogPost.Id); // finding the Tag if it's there by Blog Id using lamda function 

            if (existingBlog != null) // updating it into the DataBase
            {


                existingBlog.Id = blogPost.Id;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageURL = blogPost.FeaturedImageURL;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
          return  await bloggieDbContext.BlogPosts.Include( x => x.Tags).FirstOrDefaultAsync( x => x.UrlHandle == urlHandle );
        }
    }
}

