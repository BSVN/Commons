using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

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
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            
            return services;
        }
    }
}
