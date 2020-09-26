using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarrierLayer.Dto;
using BarrierLayer.Models;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfController : ControllerBase
    {
        private readonly ConfigService _config;
        private readonly BarrierService _barrierService;
        private readonly UserService _userService;

        public ConfController(ConfigService config, BarrierService barrierService, UserService userService)
        {
            _config = config;
            _barrierService = barrierService;
            _userService = userService;
        }
        [HttpPost("SetMasterPassword")]
        public async Task SetMasterPassword(string newPassword, string oldPassword = null)
        {
            await _config.SetMasterPassword(newPassword, oldPassword);
        }
        [HttpPost("AddBarrier")]
        public async Task<BarrierAddResult> AddBarrier(string userNumber, string barrierNumber, BarrierType type)
        {
            return await _barrierService.Register(userNumber, barrierNumber, type);
        }
        [HttpPost("ConfirmBarrier")]
        public async Task<BarrierAddResult> ConfirmBarrier(int barrierId, string smsCode)
        {
            return await _barrierService.Confirm(barrierId, smsCode);
        }
        [HttpPost("SetProperty")]
        public async Task SetProperty(string key, string value, string format, string password)
        {
            await _config.SetValue(key, value, format, password);
        }
        [HttpPost("ManualBarrier")]
        public async Task<BarrierAddResult> ManualAdd(string userNumber, string barrierNumber, BarrierType type, string token)
        {
            return await _barrierService.AddManual(userNumber, barrierNumber, type, token);
        }
        [HttpPost("CreateUser")]
        public async Task<Guid> CreateUser(string number, string password)
        {
            return await _userService.CreateUser(number, password);
        }
        [HttpPost("AddBarrierToUser")]
        public async Task AddBarrierToUser(int barrierId, string number)
        {
            await _userService.AddBarrierToUser(barrierId, number);
        }
        [HttpGet("GetBarriers")]
        public async Task<List<BarrierDto>> GetBarriers(string password)
        {
            return await _userService.GetBarrierList(password);
        }
    }
}
