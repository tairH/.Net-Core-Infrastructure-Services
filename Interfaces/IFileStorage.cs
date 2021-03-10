using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOH.ServicesAPI.Interfaces
{
    public interface IFileStorage
    {
        Task SaveFile(string uniqueFileName, IFormFile file);
        Task SaveSignedFile(string uniqueFileName, IFormFile file);
    }
}
