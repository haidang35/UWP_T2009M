using System;
using System.Collections.Generic;
using System.Text;

namespace CloudinaryService
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
            Debug.WriteLine(filePath);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filePath),
            };
            Debug.WriteLine(456);
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult;
        }
    }
}
