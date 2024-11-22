using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarrierLayer.Domain.Dto;
using BarrierLayer.Domain.Models;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfController(ConfigService config, BarrierService barrierService, UserService userService)
        : ControllerBase
    {
        [HttpPost("SetMasterPassword")]
        public async Task SetMasterPassword(string newPassword, string oldPassword = null)
        {
            await config.SetMasterPassword(newPassword, oldPassword);
        }

        [HttpPost("AddBarrier")]
        public async Task<BarrierAddResult> AddBarrier(string userNumber, string barrierNumber, BarrierType type)
        {
            return await barrierService.Register(userNumber, barrierNumber, type);
        }

        [HttpPost("ConfirmBarrier")]
        public async Task<BarrierAddResult> ConfirmBarrier(int barrierId, string smsCode)
        {
            return await barrierService.Confirm(barrierId, smsCode);
        }

        [HttpPost("SetProperty")]
        public async Task SetProperty(string key, string value, string format, string password)
        {
            await config.SetValue(key, value, format, password);
        }

        [HttpPost("ManualBarrier")]
        public async Task<BarrierAddResult> ManualAdd(string userNumber, string barrierNumber, BarrierType type,
            string token)
        {
            return await barrierService.AddManual(userNumber, barrierNumber, type, token);
        }

        [HttpPost("CreateUser")]
        public async Task<Guid> CreateUser(string number, string password)
        {
            return await userService.CreateUser(number, password);
        }

        [HttpPost("AddBarrierToUser")]
        public async Task AddBarrierToUser(int barrierId, string number)
        {
            await userService.AddBarrierToUser(barrierId, number);
        }

        [HttpGet("GetBarriers")]
        public async Task<List<BarrierDto>> GetBarriers(string password)
        {
            return await userService.GetBarrierList(password);
        }
    }
}