using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Services
{
    public class CloudinaryService
    {
        private static string MyCloudName = "dadqmakce";
        private static string APIKey = "691133837286492";
        private static string APISecret = "ydOAsaZ9-iqnS5W5LpJgWiRdTpI";
        private static Account account = new Account(MyCloudName, APIKey, APISecret);
        private static Cloudinary cloudinary = new Cloudinary(account);
        
        public ImageUploadResult UploadImage(string filePath)
        {
            cloudinary.Api.Secure = true;
            var uploadParams = new ImageUploadParams() {
                File = new FileDescription(filePath),
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult;
        }
         
        public string UploadFile(string filePath)
        {
            cloudinary.Api.Secure = true;
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(filePath),
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Uri.ToString();
        }

    }
}
