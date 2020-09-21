using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarrierLayer.Models;
using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarrierController : ControllerBase
    {
        string url = "https://api.privratnik.net:44590/app/api.php";
        [HttpGet]
        public async Task<string> Get()
        {
            //await url.PostUrlEncodedAsync()
            return Request.Path;
        }
        //key 1b92654f6afcd96f801eac91b0ea8b52

        [HttpPost("number")]
        public async Task<bool> SendSms(string number)
        {
            var response = await url.AppendPathSegment("sendSms").PostUrlEncodedAsync(new { number = number });
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        [HttpPost("checkCode")]
        public async Task<StateResponse> CheckCode(string number, string code)
        {
            var response = await url.AppendPathSegment("checkCode").PostUrlEncodedAsync(new { number = number, smsCode = code }).ReceiveJson<StateResponse>();
            return response;
        }
    }
}
