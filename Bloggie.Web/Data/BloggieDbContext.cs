using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class BloggieDbContext : DbContext

    {
        public BloggieDbContext(DbContextOptions options) : base(options)  // why are we using this learn about this
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; } // we are creating a table with the name "BlogPosts"
        public DbSet<Tag> Tags { get; set; } // it will create a table in the DB with the name of "Tags"

    }
}
