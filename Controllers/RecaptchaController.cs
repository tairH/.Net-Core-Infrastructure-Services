using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MOH.ServicesAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecaptchaController : Controller
    {
        [HttpGet("[action]")]
        public async Task<IDictionary<string, object>> ValidateRecaptcha(string response, string visible)
        {
            string secretKey = visible == "false" ? "6LevXzUUAAAAAIMa9aId7Ktotb74oACQfwIaYNRB" : "6LcufC0UAAAAABW4V3gMgX_LOZD_rHJdEMNajP1P";
            string isValid = string.Empty;
            var client = new HttpClient();
            string result = await client.GetStringAsync(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));

            var Recaptcha_set = JsonConvert.DeserializeObject<IDictionary<string, object>>(result);
            isValid = Recaptcha_set["success"].ToString();

            //return isValid;
            return Recaptcha_set;
        }
    }
}
