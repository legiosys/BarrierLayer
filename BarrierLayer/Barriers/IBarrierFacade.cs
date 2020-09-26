using BarrierLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarrierLayer.Barriers
{
    public class BaseBarrierFacade : IBarrierFacade
    {
        public async Task<BarrierResponse> Open()
        {
            await Task.Delay(1);
            return new BarrierResponse();
        }
        public async Task<BarrierResponse> Register(string userNumber)
        {
            await Task.Delay(1);
            return new BarrierResponse();
        }
        public async Task<BarrierResponse> Confirm(string userNumber, string code)
        {
            await Task.Delay(1);
            return new BarrierResponse();
        }
    }
    public interface IBarrierFacade
    {
        public Task<BarrierResponse> Open();
        public Task<BarrierResponse> Register(string userNumber);
        public Task<BarrierResponse> Confirm(string userNumber, string code);
    }
}

