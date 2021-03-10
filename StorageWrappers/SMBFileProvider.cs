using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MOH.ServicesAPI.Configuration;
using MOH.ServicesAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MOH.ServicesAPI.StorageWrappers
{
    public class SMBFileProvider : IFileStorage
    {
        public readonly NetworkCredential _networkCredentials;
        public readonly NetworkCredential _signedFilesNetworkCredentials;
    
        private readonly UnsignedFilesSettings _FUSettings;
        private readonly SignedFilesSettings _SignedFUSettings;

        public SMBFileProvider(IOptions<UnsignedFilesSettings> fuSettings, IOptions<SignedFilesSettings> sigendFuSettings)
        {
            _FUSettings = fuSettings.Value;
            _SignedFUSettings = sigendFuSettings.Value;

            _networkCredentials = new NetworkCredential(_FUSettings.AccountName, _FUSettings.AccountKey, _FUSettings.Domain);
            _signedFilesNetworkCredentials = new NetworkCredential(_SignedFUSettings.AccountName, _SignedFUSettings.AccountKey, _SignedFUSettings.Domain);
        }
        public async Task SaveFile(string uniqueFileName, IFormFile file)
        {
            string networkPath = _FUSettings.NetworkPath + _FUSettings.ShareFolder;

            var savePath = Path.Combine(networkPath, uniqueFileName);
            

            using (new Helpers.Impersonation(networkPath,_networkCredentials))
            {
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public async Task SaveSignedFile(string uniqueFileName, IFormFile file)
        {
            string networkPath = _SignedFUSettings.NetworkPath + _SignedFUSettings.ShareFolder;

            var savePath = Path.Combine(networkPath, uniqueFileName);


            using (new Helpers.Impersonation(networkPath, _signedFilesNetworkCredentials))
            {
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
