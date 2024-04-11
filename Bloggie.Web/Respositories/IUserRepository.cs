using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Respositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
