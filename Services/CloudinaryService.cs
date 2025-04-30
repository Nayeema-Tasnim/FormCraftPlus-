
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;


namespace finalproject.Services
{
    public class CloudinaryService
    {
         private readonly Cloudinary _cloudinary;
         public CloudinaryService(IConfiguration configuration)
         {
              var cloudName = configuration["CloudinarySettings:CloudName"];
              var apiKey = configuration["CloudinarySettings:ApiKey"];
              var apiSecret = configuration["CloudinarySettings:ApiSecret"];
              if (string.IsNullOrWhiteSpace(cloudName))
                  throw new ArgumentNullException("Cloud name must be specified in CloudinarySettings");
              var account = new Account(cloudName, apiKey, apiSecret);
              _cloudinary = new Cloudinary(account);
         }

         public async Task<string> UploadImageAsync(IFormFile file)
         {
              using (var stream = file.OpenReadStream())
              {
                  var uploadParams = new ImageUploadParams
                  {
                       File = new FileDescription(file.FileName, stream)
                  };
                  var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                  return uploadResult.SecureUrl?.ToString() ?? string.Empty;
              }
         }
    }
}
