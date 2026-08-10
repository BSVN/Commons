using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace BSN.Commons.AutoMapper.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonsAutoMapper(this IServiceCollection services, Action<IMapperConfigurationExpression> configure)
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                MapperConfigurationExpression mapperConfigurationExpression = new MapperConfigurationExpression();
                configure(mapperConfigurationExpression);

                configure(config);

                config.AddProfile(new CommonMapperProfile());
            }, NullLoggerFactory.Instance);

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            return services;
        }
    }
}
