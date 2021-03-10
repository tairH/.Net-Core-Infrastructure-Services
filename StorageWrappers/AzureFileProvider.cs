using MOH.ServicesAPI.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MOH.ServicesAPI.Configuration;
using System.Reflection;
using MOH.Common;

namespace MOH.ServicesAPI.StorageWrappers
{
    public class AzureFileProvider : IFileStorage
    {
        public readonly CloudStorageAccount _cFileStorageAccount;
        public readonly CloudStorageAccount _cSignedFileStorageAccount;

        //public readonly FileUploadSettings _fileUploadConfig;
        private readonly UnsignedFilesSettings _FUSettings;
        private readonly SignedFilesSettings _SignedFUSettings;

        public AzureFileProvider(IOptions<UnsignedFilesSettings> fuSettings, IOptions<SignedFilesSettings> sigendFuSettings)//IOptions<FileUploadSettings> fileUploadConfig
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name,new Dictionary<string, object> { { "app signed file settings config", sigendFuSettings } });

            _FUSettings = fuSettings.Value;
            _SignedFUSettings = sigendFuSettings.Value;

            _cFileStorageAccount = new CloudStorageAccount(new StorageCredentials(_FUSettings.AccountName, _FUSettings.AccountKey), true);
            _cSignedFileStorageAccount = new CloudStorageAccount(new StorageCredentials(_SignedFUSettings.AccountName, _SignedFUSettings.AccountKey), true);
        }
       
        public async Task SaveFile(string uniqueFileName, IFormFile file)
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            CloudFileClient fileClient = _cFileStorageAccount.CreateCloudFileClient();

            //get the share reference (on the storage)
            CloudFileShare ExistingShare = fileClient.GetShareReference(_FUSettings.ShareFolder);

            try
            {
                await ExistingShare.CreateIfNotExistsAsync();
            }
            catch (StorageException ex)
            {
                //Console.WriteLine("Please make sure your storage account has storage file endpoint enabled and specified correctly in the app.config - then restart the sample.");
                //Console.WriteLine("Press any key to exit");
                //Console.ReadLine();
                LogManager.LogError(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex, "Please make sure your storage account has storage file endpoint enabled and specified correctly in the app.config - then restart the sample.");
                //throw(ex);
            }

            // Get a reference to the root directory of the share.
            CloudFileDirectory shareRoot = ExistingShare.GetRootDirectoryReference();

            await shareRoot.GetFileReference(uniqueFileName).UploadFromStreamAsync(file.OpenReadStream());
        }

        public async Task SaveSignedFile(string uniqueFileName, IFormFile file)
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            CloudFileClient fileClient = _cSignedFileStorageAccount.CreateCloudFileClient();

            CloudFileShare ExistingShare = fileClient.GetShareReference(_SignedFUSettings.ShareFolder);

            await ExistingShare.CreateIfNotExistsAsync();

            CloudFileDirectory shareRoot = ExistingShare.GetRootDirectoryReference();

            await shareRoot.GetFileReference(uniqueFileName).UploadFromStreamAsync(file.OpenReadStream());
        }
    }
}
