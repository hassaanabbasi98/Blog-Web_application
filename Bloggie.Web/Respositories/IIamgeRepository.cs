namespace Bloggie.Web.Respositories
{
    public interface IIamgeRepository
    {
        Task<string> UploadAsync(IFormFile file);

    }
}
