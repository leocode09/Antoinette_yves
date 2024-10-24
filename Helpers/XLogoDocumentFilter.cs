using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;

namespace WeCareWebApp.Helpers
{
    public class XLogoDocumentFilter : IDocumentFilter
    {
        readonly IWebHostEnvironment _env;
        public XLogoDocumentFilter(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!swaggerDoc.Info.Extensions.ContainsKey("x-logo"))
            {
                swaggerDoc.Info.Extensions.Add("x-logo", new OpenApiObject
                {
                    { "url", new OpenApiString(Path.Combine(_env.WebRootPath, "/img/tiger-soft-logo.png"))},
                    { "backgroundColor", new OpenApiString("#FFFFFF") },
                    { "altText", new OpenApiString("PetStore Logo") }
                });
            }
        }
    }
}
