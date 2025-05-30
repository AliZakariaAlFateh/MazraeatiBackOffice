using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MazraeatiBackOffice.Configuration
{
    public static class GenericFunction
    {
        public static string UploadedFile(IFormFile formFile, IWebHostEnvironment webHostEnvironment ,string imagePath)
        {
            string uniqueFileName = null;

            if (formFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath + "/images" + (string.IsNullOrEmpty(imagePath) ? "" : $"/{imagePath}"));
                uniqueFileName = Guid.NewGuid().ToString() + "." + formFile.ContentType.Replace("image/", string.Empty);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public static string UploadedVideo(IFormFile formFile, IWebHostEnvironment webHostEnvironment)
        {
            string uniqueFileName = null;

            if (formFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath + "/Videos");
                uniqueFileName = Guid.NewGuid().ToString() + "." + formFile.ContentType.Replace("video/", string.Empty);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        
    }
}
