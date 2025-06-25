using AutoMapper;
using Coworking.Aplication.Profiles;

namespace Coworking.Integration.Common
{
    public static class TestMapperFactory
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReservaProfile>();
            });
            return config.CreateMapper();
        });

        public static IMapper Create() => _mapper.Value;
    }
}
