using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MOH.Common;
using MOH.Common.Helpers;
using MOH.Common.Interfaces;
using MOH.Common.ViewModels;
using MOH.CoreTemplate.Configuration;
using Newtonsoft.Json.Linq;

namespace MOH.ServicesAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Lists")]
    public class ListsController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IHttpClientService _httpClientService;
        public ListsController(IOptions<DataProvider> dataProviderSettings,IHttpClientService httpClientService)
        {
            _dataProvider = dataProviderSettings.Value;
            _httpClientService = httpClientService;
        }

        // GET: api/Lists
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Lists/{listName}
        [HttpGet("{listName}")]
        public async Task<ApiResponse> Get(string listName)
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl,
                 _dataProvider.GeneralListsApi,
                 listName);

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            return new ApiOkResponse(result); 

        }

        // GET: api/Lists/{siteId}/{listCode}
        [HttpGet("{siteId}/{listCode}")]
        public async Task<ApiResponse> Get(int siteId, string listCode)
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);
            try {
                JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl,
                 _dataProvider.GeneralListsApi,
                 new Dictionary<string, string>() { { "siteId", siteId.ToString() }, { "listCode", listCode } });
                LogManager.LogInfo(MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    MethodBase.GetCurrentMethod().Name,new Dictionary<string,object>() { { "siteId",siteId }, { "listCode",listCode },
                        { "result",result} },"Success get list from dataProvider");

                return new ApiOkResponse(result);
            }
            catch (Exception ex) {
                LogManager.LogError(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name, ex, 
                    new Dictionary<string, object>() { { "siteId", siteId }, { "listCode", listCode } }, "error get list from dataProvider");
            }

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

             return new ApiOkResponse(null);

        }

        // GET: api/Lists/edm/{listName}
        [HttpGet("edm/{listName}")]
        public async Task<ApiResponse> Edm(string listName)
        {
            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl,
                _dataProvider.EdmListsApi,
                 listName);

            return new ApiOkResponse(result);
           
        }

        // POST: api/Lists
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Lists/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}