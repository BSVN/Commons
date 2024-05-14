using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BSN.Commons.AutoMapper.Tests
{
    public class IServiceCollectionExtensionsTests : AutoMapperTestBase
    {
        [Test]
        public void AddAutoMapper_AddsMapperToServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configure = new Action<IMapperConfigurationExpression>(config => { });

            // Act
            services.AddAutoMapper(configure);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var mapper = serviceProvider.GetService<IMapper>();
            Assert.NotNull(mapper);
        }
    }
}
