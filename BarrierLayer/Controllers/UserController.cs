using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarrierLayer.Domain.Dto;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(UserService userService) : ControllerBase
    {
        [HttpPost("RegisterApp")]
        public async Task<Guid> RegisterApp(string number, Guid password)
        {
            Console.WriteLine($"{number} {password}");
            return await userService.RegisterApp(number, password);
        }

        [HttpGet("GetBarriers")]
        public async Task<List<BarrierForUserDto>> GetBarriers(Guid userKey)
        {
            return await userService.GetBarrierList(userKey);
        }
    }
}