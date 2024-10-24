using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using WeCareWebApp.EF;
using WeCareWebApp.Helpers;
using WeCareWebApp.Services;
using Swashbuckle.AspNetCore.ReDoc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WeCareWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));




            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(option =>
               {
                   option.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Connection.JwtIssuer,
                       ValidAudience = Connection.JwtIssuer,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Connection.JwtKey))
                   };
               });

            services.AddAuthorization();

            var mapped = new MapperConfiguration(m => { m.AddProfile<MappingHelper>(); });
            IMapper mapper = mapped.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRazorPages();
           
            services.AddMvcCore(o =>
            {
                o.Filters.Add<ExceptionActionFilter>();
            })
                .AddApiExplorer()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                 });
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<XLogoDocumentFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WK API",
                    Description = "We Kare Api 1.0 ",
                    TermsOfService = new Uri("http://www.wkya.rw"),
                    Contact = new OpenApiContact() { Name = "wecare", Email = "rwahamanick@gmail.com", Url = new Uri("http://www.wkya.rw") }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement());
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<FileUploadOperation>();
            });

            services.AddDbContext<WeCareDbContext>(o => o.UseSqlServer(Connection.Production));
            


            services.AddScoped<IWeCareRepository, WeCareRepository>();
            services.AddScoped<HttpClient>();
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                            Log.Error(exceptionHandlerFeature.Error, "An unexpected fault happened. Try again later");
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Try again later");
                    });
                });
            }
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(new
                {
                    Message = !string.IsNullOrEmpty(exception.Message) ? exception.Message : "No Message",
                    Source = !string.IsNullOrEmpty(exception.Source) ? exception.Source : "No Source",
                    InnerException = exception.InnerException != null ? exception.InnerException.Message : "No Inner Exception"
                });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseSwagger(o =>
            {
                o.RouteTemplate = "docs/{documentName}/docs.json";
            });
            app.UseSwagger(o =>
            {
                o.RouteTemplate = "dev/{documentName}/dev.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
                c.DisplayRequestDuration();
                c.DocumentTitle = "We kare Api 1.0";
                c.RoutePrefix = "dev";
                c.SwaggerEndpoint("/dev/v1/dev.json", "WK Api");
                c.InjectStylesheet("/css/custom.css");
                c.InjectJavascript("/js/custom.js");
            });
            app.UseReDoc(c =>
            {
                c.RoutePrefix = "docs";
                c.SpecUrl = "v1/docs.json";
                c.ConfigObject = new ConfigObject
                {
                    HideDownloadButton = true,
                    HideLoading = true
                };
            });
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
