using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Helpers
{
    public static class Tools
    {
        public static byte[] FileUpload(IFormFile formFile)
        {

            using (var stream = new MemoryStream())
            {
                formFile.CopyTo(stream);

                var fileBytes = stream.ToArray();
                return fileBytes;
            }


        }
    }
}
