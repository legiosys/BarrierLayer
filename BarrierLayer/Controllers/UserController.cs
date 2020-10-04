using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using BarrierLayer.Dto;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }   

        [HttpPost("RegisterApp")]
        public async Task<Guid> RegisterApp(string number, Guid password)
        {
            Console.WriteLine($"{number} {password}");
            return await _userService.RegisterApp(number, password);
        }

        [HttpGet("GetBarriers")]
        public async Task<List<BarrierForUserDto>> GetBarriers(Guid userKey)
        {
            return await _userService.GetBarrierList(userKey);
        }
    }
}
