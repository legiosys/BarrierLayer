using System;
using System.Threading.Tasks;
using BarrierLayer.Models;
using BarrierLayer.Services;
using Flurl.Http;
using Newtonsoft.Json;

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
            var response = await _url
                .PostMultipartAsync(mp => mp
                    .AddString("number", userNumber)
                    .AddString("smsCode", code))
                .ReceiveJson<ConfirmResponse>();
            return new BarrierResponse()
            {
                Key = response.Key
            };
        }

        private async Task<BarrierResponse> OpenWithRefused()
        {
            var response = await _url
                .PostMultipartAsync(mp => mp
                    .AddString("barrier_id", _barrier.BarrierNumber)
                    .AddString("command", "open")
                    .AddString("key", _barrier.Token)
                    .AddString("login", _barrier.UserNumber));
            var responseString = await response.GetStringAsync();
            Console.WriteLine($"Open result {response.StatusCode}: {responseString}");
            var responseJson = JsonConvert.DeserializeObject<StateResponse>(responseString);
            return responseJson.ToBarrierResponse();
        }

        public async Task<BarrierResponse> Open()
        {
            try
            {
                return await OpenWithRefused();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Message.Contains("Connection refused"))
                {
                    return await OpenWithRefused();
                }
                else
                {
                    Console.WriteLine($"Error open: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<BarrierResponse> Register(string userNumber)
        {
            var response = await _url
                .PostMultipartAsync(mp => mp
                    .AddString("number", userNumber))
                .ReceiveJson<StateResponse>();
            return response.ToBarrierResponse();
        }

        public class StateResponse
        {
            public int State { get; set; }

            public BarrierResponse ToBarrierResponse()
            {
                return new BarrierResponse()
                {
                    State = this.State
                };
            }
        }

        public record ConfirmResponse(string Key);
    }
}