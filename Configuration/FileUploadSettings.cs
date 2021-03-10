using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOH.ServicesAPI.Configuration
{
    public class FileProviderSettings
    {
        public string FileProvider { get; set; }
    }
    public class FileUploadSettings
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string Domain { get; set; }
        public string NetworkPath { get; set; }
        public string ShareFolder { get; set; }

    }
    public class SignedFilesSettings : FileUploadSettings { }
    public class UnsignedFilesSettings : FileUploadSettings { }
}
