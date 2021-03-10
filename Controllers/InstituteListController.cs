using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MOH.Common;
using MOH.Common.Interfaces;
using MOH.Common.ViewModels;
using MOH.CoreTemplate.Configuration;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MOH.ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    public class InstituteListController : Controller
    {

        private readonly DataProvider _dataProvider;
        private readonly IHttpClientService _httpClientService;
        public InstituteListController(IOptions<DataProvider> dataProviderSettings, IHttpClientService httpClientService)
        {
            _dataProvider = dataProviderSettings.Value;
            _httpClientService = httpClientService;
        }
        // GET: api/InstituteList/InstituteHospitalList
        [HttpGet]
        public async Task<ApiResponse> InstituteHospitalList()
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl,_dataProvider.InstituteListApi + "getInstituteHospitalList");

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            return new ApiOkResponse(result);

        }

        // GET: api/InstituteList/InstituteHMOList
        [HttpGet("InstituteHMOList")]
        public async Task<ApiResponse> InstituteHMOList()
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl, _dataProvider.InstituteListApi + "getInstituteHMOList");

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            return new ApiOkResponse(result);

        }

        // GET: api/InstituteList/InstituteHMOList
        [HttpGet("HospitalsAndHMOList")]
        public async Task<ApiResponse> HospitalsAndHMOList()
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl, _dataProvider.InstituteListApi + "getHospitalsAndHMOList");

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            return new ApiOkResponse(result);

        }

        [HttpGet("InstitutesList")]
        public async Task<ApiResponse> InstitutesList()
        {
            LogManager.LogStartFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            JArray result = await _httpClientService.Get<JArray>(_dataProvider.dataProviderApiUrl, _dataProvider.InstituteListApi + "getInstitutesList");

            LogManager.LogEndFunction(MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().Name);

            return new ApiOkResponse(result);

        }



        //// GET api/<controller>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
