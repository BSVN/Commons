using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
            services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
            // Act
            services.AddAutoMapper(configure);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var mapper = serviceProvider.GetService<IMapper>();
            Assert.NotNull(mapper);
        }
    }
}
