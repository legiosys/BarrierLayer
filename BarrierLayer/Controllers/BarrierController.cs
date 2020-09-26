using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarrierLayer.Models;
using BarrierLayer.Services;
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
        private readonly BarrierService _barrierService;

        public BarrierController(BarrierService barrierService)
        {
           /* _user = new User()
            {
                Number = "+79851590409",
                Token = Guid.Parse("1b92654f6afcd96f801eac91b0ea8b52")
            };*/
            _barrierService = barrierService;
        }
        string url = "https://api.privratnik.net:44590/app/api.php";
        [HttpGet]
        public async Task<string> Get()
        {
            //await url.PostUrlEncodedAsync()
            return Request.Path;
        }
        //key 1b92654f6afcd96f801eac91b0ea8b52
        //classes3 api


        [HttpPost("open")]
        public async Task Open(Guid userKey, int barrierId)
        {
            await _barrierService.Open(userKey, barrierId);
        }

        
    }
}
