using BarrierLayer.Domain.Models;
using BarrierLayer.Services;

namespace BarrierLayer.Barriers
{
    public class BarrierFacadeFactory
    {
        private readonly ConfigService _config;

        public BarrierFacadeFactory(ConfigService config)
        {
            _config = config;
        }

        public IBarrierFacade Create(Barrier barrier)
        {
            switch (barrier?.BarrierType)
            {
                case BarrierType.Privratnik:
                    return new PrivratnikBarrierFacade(_config, barrier);
                default:
                    return new BaseBarrierFacade();
            }
        }
    }
}