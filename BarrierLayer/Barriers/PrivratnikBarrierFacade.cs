using BarrierLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;
using BarrierLayer.Models;

namespace BarrierLayer.Barriers
{
    public class PrivratnikBarrierFacade : IBarrierFacade
    {
        private readonly string _url;
        private readonly Barrier _barrier;

        public PrivratnikBarrierFacade(ConfigService config, Barrier barrier)
        {
            _url = config.GetValue("PrivratnikUrl", "url").Result; 
            _barrier = barrier;

        }

        public async Task<BarrierResponse> Confirm(string userNumber, string code)
        {
            var response = await _url.AppendPathSegment("checkCode").PostUrlEncodedAsync(new { number = userNumber, smsCode = code }).ReceiveJson<StateResponse>();
            return response.ToBarrierResponse();
        }

        public async Task<BarrierResponse> Open()
        {
            var response = await _url.AppendPathSegment("openAddedBarrier")
                .PostUrlEncodedAsync(
                new { 
                    login = _barrier.UserNumber, 
                    key = _barrier.Token, 
                    from = _barrier.UserNumber, 
                    to = _barrier.BarrierNumber 
                }).ReceiveJson<StateResponse>();
            return response.ToBarrierResponse();
        }

        public async Task<BarrierResponse> Register(string userNumber)
        {
            var response = await _url.AppendPathSegment("sendSms").PostUrlEncodedAsync(new { number = userNumber }).ReceiveJson<StateResponse>();
            return response.ToBarrierResponse();
        }

        public class StateResponse
        {
            public string Key { get; set; }
            public string Login { get; set; }
            public int State { get; set; }

            public BarrierResponse ToBarrierResponse()
            {
                return new BarrierResponse()
                {
                    Key = this.Key,
                    Login = this.Login,
                    State = this.State
                };
            }
        }
    }
}
