using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MOH.Common;
using MOH.ServicesAPI.Configuration;
using MOH.ServicesAPI.Interfaces;

namespace MOH.ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    public class FileUploadController : Controller
    {
        private readonly ILogger<FileUploadController> _logger;

      
        private readonly IFileStorage _fileProvider;


     /*   public FileUploadController(ILogger<FileUploadController> logger)
        {
            _logger = logger;
        }*/
        public FileUploadController( IFileStorage fileProvider )
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

          
            _fileProvider = fileProvider;
        }


        [HttpPost("[action]")]
        [EnableCors("MyPolicy")]
        public async Task<string> Upload()
        {
           
                var files = Request.Form.Files;
               
                var uniqueFileName = String.Empty;
              

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(uniqueFileName))
                            uniqueFileName += ";" + $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        else
                            uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                        await _fileProvider.SaveFile(uniqueFileName, file);

                    }

                }
                return uniqueFileName;
           
        }

        [HttpPost("[action]")]
        [EnableCors("MyPolicy")]
        public async Task<string> UploadSignedFile()
        {
            var files = Request.Form.Files;

            var uniqueFileName = String.Empty;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    if (!string.IsNullOrEmpty(uniqueFileName))
                        uniqueFileName += ";" + $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    else
                        uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    await _fileProvider.SaveSignedFile(uniqueFileName, file);
                }
            }
            return uniqueFileName;
        }

        [HttpGet("Download/{guid}")]
        public JsonResult Download(string guid)
        {
            try
            {
                string base64 = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAAHCAYAAADebrddAAABS2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDAgNzkuMTYwNDUxLCAyMDE3LzA1LzA2LTAxOjA4OjIxICAgICAgICAiPgogPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIi8+CiA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgo8P3hwYWNrZXQgZW5kPSJyIj8+LUNEtwAAAIFJREFUGJVlzrsNwkAQBcDxVQESIjCcBDEN2KIOIKUn5Ig+QBQC/kAfkJylk9lkg52nt0WMsUSDI97+Z44rzgEXVLhjMYEz3LBHE3DCE6tJYIQbtDgEfFDjhXUK7BLcJlhhKGKMY+UywRJfFOgS7CFk/w2poc1gPcIpzgOPtLv8+APH8Rz6cWj6rwAAAABJRU5ErkJggg==";
                string fileName = "chevron.png";
                return new JsonResult(new { fileContent = base64,  fileName });

                //var filePath = "E:\\UploadedData\\1b2707f2-d279-4124-92a7-65198f92ee3f.png";//Path.Combine(@"\\azrshare03.file.core.windows.net\dgtinput", fileName);//"E:\\UploadedData"

                //var dataBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                ////adding bytes to memory stream   
                //var dataStream = new MemoryStream(dataBytes);
                //var response = File(dataStream, "application/octet-stream"); // FileStreamResult
                //return response;
                //return new eBookResult(dataStream, Request, bookName);

                //using (var client = new HttpClient())
                //{
                //    var res = await client.GetAsync(filePath);
                //    //response.Content = new StreamContent(await res.Content.ReadAsStreamAsync());
                //    var stream = await res.Content.ReadAsStreamAsync(); //await { { __get_stream_here__} }
                //    var response = File(stream, "application/octet-stream"); // FileStreamResult

                //    return response;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
