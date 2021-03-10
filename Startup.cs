using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using MOH.Common.CoreMiddlewares;
using MOH.Common.Helpers;
using MOH.Common.Interfaces;
using MOH.CoreTemplate.Configuration;
using MOH.ServicesAPI.Configuration;
using MOH.ServicesAPI.Extenstions;
using MOH.ServicesAPI.Interfaces;
using MOH.ServicesAPI.StorageWrappers;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;

namespace MOH.ServicesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            env.ConfigureNLog("nlog.config");
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            NLog.LogManager.Configuration.Variables["appName"] = PlatformServices.Default.Application.ApplicationName;
            //services.AddCors();
            services.Configure<DataProvider>(Configuration.GetSection("DataProvider"));
                       
            services.Configure<UnsignedFilesSettings>(Configuration.GetSection("UnsignedFileUploadSettings"));

            services.Configure<SignedFilesSettings>(Configuration.GetSection("SignedFileUploadSettings"));

            services.AddFileProvider(HostingEnvironment, Configuration);

            services.AddHttpClient<IHttpClientService,HttpClientService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                /*builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>())
                 .AllowAnyMethod()
                       .AllowCredentials()
                       .AllowAnyHeader();//"http://localhost:50258"*/
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .AllowAnyHeader();/**/
            }));
            
            services.AddMvc();
            
            if (Convert.ToBoolean(Configuration.GetSection("Swagger").GetSection("EnableSwagger").Value) == true)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Services API", Version = "v1" });
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

            loggerFactory.AddNLog();
            
            //add NLog.Web
            app.AddNLogWeb();
            
            if (Convert.ToBoolean(Configuration.GetSection("Swagger").GetSection("EnableSwagger").Value) == true)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Services API V1");
                });
            }
        }
    }
}
