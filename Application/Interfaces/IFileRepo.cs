using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IFileRepo
    {
        Task<string> CreateFileAsync(IFormFile file, string path);
        Task<string> CreateFileFromBase64Async(string base64String, string fileName, string path);
        public bool DeleteFile(string path);

    }
}