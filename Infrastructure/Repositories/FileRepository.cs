using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Application.Interfaces;

namespace Infrastructure.Repositories
{
  public class FileRepo : IFileRepo
  {

    private readonly IHostEnvironment _hostEnvironment;
    public FileRepo(IHostEnvironment hostEnvironment)
    {
      _hostEnvironment = hostEnvironment;
    }

    public async Task<string> CreateFileAsync(IFormFile file, string path)
    {
      try
      {
        if (file == null) return null;

        var fileName = file.FileName;

        var extention = "." + fileName.Split('.')[fileName.Split('.').Length - 1];

        var lastFileName = Guid.NewGuid() + extention;

        var pathDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Upload", path);

        if (!Directory.Exists(pathDirectory))
        {
          Directory.CreateDirectory(pathDirectory);
        }

        var pathFile = Path.Combine(pathDirectory, lastFileName);

        using (var stream = new System.IO.FileStream(pathFile, System.IO.FileMode.Create))
        {
          await file.CopyToAsync(stream);
        }

        return "Upload/" + path + "/" + lastFileName;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }

    }

    public async Task<string> CreateFileFromBase64Async(string base64String, string fileName, string path)
    {
      try
      {
        if (base64String == null) return null;

        var bytes = Convert.FromBase64String(base64String);
        var pathDirectory = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Upload", path);

        MemoryStream stream = new MemoryStream(bytes);

        // var newFileName = Guid.NewGuid() + GetFileExtension(base64String);
        var newFileName = fileName + GetFileExtension(base64String);

        IFormFile file = new FormFile(stream, 0, bytes.Length, newFileName, newFileName);

        if (!Directory.Exists(pathDirectory))
        {
          Directory.CreateDirectory(pathDirectory);
        }

        var pathFile = Path.Combine(pathDirectory, newFileName);

        using (var st = File.Create(pathFile))
        {
          await file.CopyToAsync(st);
        }

        return "Upload/" + path + "/" + newFileName;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }

    }



    public static string GetFileExtension(string base64String)
    {
      var data = base64String.Substring(0, 5);

      switch (data.ToUpper())
      {
        case "UESDB":
          return ".zip";
        case "IVBOR":
          return ".png";
        case "/9J/4":
          return ".jpg";
        case "AAAAF":
          return ".mp4";
        case "JVBER":
          return ".pdf";
        case "AAABA":
          return ".ico";
        case "UMFYI":
          return ".rar";
        case "E1XYD":
          return ".rtf";
        case "U1PKC":
          return ".txt";
        case "MQOWM":
        case "77U/M":
          return ".srt";
        default:
          return ".png";
      }
    }
    public bool DeleteFile(string path)
    {
      try
      {
        // File.Delete(Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/" + path));

        string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot/" + path);

        if (System.IO.File.Exists(filePath))
        {
          System.IO.File.Delete(filePath);
          return true;
        }

        return false;
      }
      catch (Exception)
      {
        return false;
      }

    }

  }
}