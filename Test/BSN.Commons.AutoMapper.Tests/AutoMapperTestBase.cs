using AutoMapper;

namespace BSN.Commons.AutoMapper.Tests
{
    public abstract class AutoMapperTestBase
    {
        protected readonly IMapper _mapper;

        protected AutoMapperTestBase()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CommonMapperProfile>();
            });

            _mapper = configuration.CreateMapper();
        }
    }
}
