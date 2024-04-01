using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Respositories
{
    public class CloudinaryImageRepositoryl : IIamgeRepository

    {
        private readonly IConfiguration configuration;
        private readonly Account account;

        public CloudinaryImageRepositoryl(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
            configuration.GetSection("Cloudinary")["CloudinaryName"],
            configuration.GetSection("Cloudinary")["ApiKey"],
            configuration.GetSection("Cloudinary")["ApiSecret"]
                );
        }

        [Obsolete]
        public async Task<string?> UploadAsync(IFormFile file)
        {
            var client = new Cloudinary(account);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await client.UploadAsync(uploadParams);

            if(uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUri.ToString();
            }
            return null;
        }
    }
}
