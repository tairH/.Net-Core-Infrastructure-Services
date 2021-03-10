using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MOH.Common.Helpers;
using MOH.Common.Interfaces;
using MOH.Common.ViewModels.MohPackageEntities;
using MOH.CoreTemplate.Configuration;
using Newtonsoft.Json.Linq;

namespace MOH.ServicesAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Menu")]
    public class MenuController : Controller
    {
        private readonly DataProvider _dataProvider;
        private readonly IHttpClientService _httpClientService;
        public MenuController(IOptions<DataProvider> dataProviderSettings, IHttpClientService httpClientService)
        {
            _dataProvider = dataProviderSettings.Value;
            _httpClientService = httpClientService;
        }

        // GET: api/Menu/{menuId}
        [HttpGet("{menuId}")]
        public async Task<MenuItem[]> Get(int menuId)
        {
            var result = await _httpClientService.Get<MenuItem[]>(_dataProvider.dataProviderApiUrl,
                _dataProvider.MenuApi,
                menuId.ToString());

            return result;
           
        }
    }
}