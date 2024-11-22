using System;
using System.Threading.Tasks;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarrierController(BarrierService barrierService) : ControllerBase
    {
        /* _user = new User()
            {
                Number = "+79851590409",
                Token = Guid.Parse("1b92654f6afcd96f801eac91b0ea8b52")
            };*/

        string url = "https://api.privratnik.net:44590/app/api.php";

        [HttpGet]
        public async Task<string> Get()
        {
            //await url.PostUrlEncodedAsync()
            return Request.Path;
        }
        //key 1b92654f6afcd96f801eac91b0ea8b52
        //classes3 api 74999950866
        //4849 65637c20-c978-4db0-8134-523e2759e832

        [HttpPost("OpenBarrier")]
        public async Task<bool> Open(Guid userKey, int barrierId)
        {
            Console.WriteLine($"{userKey} trying open barrier {barrierId}");
            try
            {
                await barrierService.Open(userKey, barrierId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}