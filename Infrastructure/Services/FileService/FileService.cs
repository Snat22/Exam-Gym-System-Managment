using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.FileService;

public class FileService(IWebHostEnvironment hostEnvironment) : IFileService
{

    public async Task<string> CreateFile(IFormFile file)
    {
        try
        {
            var fileName =
                string.Format($"{Guid.NewGuid() + Path.GetExtension(file.FileName)}");
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "IMG", fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public bool DeleteFile(string file)
    {
        try
        {
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "IMG", file);
            File.Delete(fullPath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}