using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MOH.ServicesAPI.Configuration;
using MOH.ServicesAPI.Interfaces;
using MOH.ServicesAPI.StorageWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOH.ServicesAPI.Extenstions
{
    public static class FileServiceExtensions
    {
        
        public static IServiceCollection AddFileProvider(this IServiceCollection services, IHostingEnvironment env, IConfiguration config)
        {
           
            services.Configure<FileProviderSettings>(config.GetSection("FileProviderSettings"));
            var fileProviderConfig = config.GetSection("FileProviderSettings").Get<FileProviderSettings>();


            if (fileProviderConfig.FileProvider == "smb")
            {
                services.AddTransient<IFileStorage, SMBFileProvider>();
            }
            else if(fileProviderConfig.FileProvider == "azure")
            {
                services.AddTransient<IFileStorage, AzureFileProvider>();
            }
            return services;
        }
    }
}
