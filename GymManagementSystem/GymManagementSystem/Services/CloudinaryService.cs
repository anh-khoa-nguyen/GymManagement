using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService()
    {
        var cloudName = ConfigurationManager.AppSettings["Cloudinary:CloudName"];
        var apiKey = ConfigurationManager.AppSettings["Cloudinary:ApiKey"];
        var apiSecret = ConfigurationManager.AppSettings["Cloudinary:ApiSecret"];

        if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
        {
            throw new ArgumentNullException("Cloudinary configuration is missing in Web.config.");
        }

        Account account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(HttpPostedFileBase file)
    {
        if (file == null || file.ContentLength == 0)
        {
            return null;
        }

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, file.InputStream),
            // Có thể thêm các tùy chọn khác ở đây, ví dụ:
            // Folder = "BaiTap",
            // Transformation = new Transformation().Width(800).Height(600).Crop("limit")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            // Ghi log lỗi
            System.Diagnostics.Debug.WriteLine(uploadResult.Error.Message);
            return null;
        }

        // Trả về URL an toàn (https) của ảnh đã upload
        return uploadResult.SecureUrl.ToString();
    }
}