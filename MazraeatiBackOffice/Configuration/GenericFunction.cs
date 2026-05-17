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
        //public static string UploadedFile(IFormFile formFile, IWebHostEnvironment webHostEnvironment ,string imagePath)
        //{
        //    string uniqueFileName = null;

        //    if (formFile != null)
        //    {
        //        if (!Directory.Exists(imagePath))
        //            Directory.CreateDirectory(imagePath);
        //        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath + "/images" + (string.IsNullOrEmpty(imagePath) ? "" : $"/{imagePath}"));
        //        uniqueFileName = Guid.NewGuid().ToString() + "." + formFile.ContentType.Replace("image/", string.Empty);
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            formFile.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}

        public static string UploadedFile(IFormFile formFile, IWebHostEnvironment env, string folderName)
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            // ✅ مسار صحيح 100%
            string uploadsFolder = Path.Combine(env.WebRootPath, "images", folderName);

            // ✅ إنشاء الفولدر لو مش موجود
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // ✅ اسم ملف صحيح
            string extension = Path.GetExtension(formFile.FileName);
            string uniqueFileName = Guid.NewGuid().ToString() + extension;

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
        public static string UploadedVideo(IFormFile formFile, IWebHostEnvironment webHostEnvironment, string videoPath)
        {
            string uniqueFileName = null;

            if (formFile != null)
            {
                if (!Directory.Exists(videoPath))
                    Directory.CreateDirectory(videoPath);
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath + "/Videos" + (string.IsNullOrEmpty(videoPath) ? "" : $"/{videoPath}"));
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
