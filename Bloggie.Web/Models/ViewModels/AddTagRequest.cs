using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Bloggie.Web.Models.ViewModels
{
    public class AddTagRequest
    {
        public String Name { get; set; }
        public String DisplayName { get; set; }

    }
}
