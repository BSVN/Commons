using AutoMapper;
using BSN.Commons.Responses;

namespace BSN.Commons.AutoMapper.Tests
{
    [TestFixture]
    public class CommonMapperProfileTests
    {
        [Test]
        public void PagedEntityCollectionToMetaDataConverter_ConvertsCorrectly()
        {
            // Arrange
            var profile = new CommonMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var pagedEntityCollection = new PagedEntityCollection<int>
            {
                CurrentPage = 1,
                PageSize = 10,
                RecordCount = 100
            };

            // Act
            var result = mapper.Map<PaginationMetadata>(pagedEntityCollection);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pagedEntityCollection.CurrentPage, result.Page);
            Assert.AreEqual(pagedEntityCollection.PageSize, result.PageSize);
            Assert.AreEqual(pagedEntityCollection.RecordCount, result.RecordCount);
        }

        [Test]
        public void GenericIEnumerableToCollectionViewModelConverter_ConvertsCorrectly()
        {
            // Arrange
            var profile = new CommonMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var items = new List<int> { 1, 2, 3 };

            // Act
            var result = mapper.Map<CollectionViewModel<int>>(items);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(items.Count, result.Items.Count());
        }

        [Test]
        public void CustomProfileConverter_ConvertsCorrectly()
        {
            // Arrange
            var customProfile = new CustomMapperProfile();
            var profile = new CommonMapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(customProfile);
            });
            var mapper = new Mapper(configuration);
            var customEntity = new CustomEntity { Id = 1, Name = "Custom Entity" };

            // Act
            var result = mapper.Map<CustomViewModel>(customEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customEntity.Id, result.Id);
            Assert.AreEqual(customEntity.Name, result.Name);
        }

        public class CustomMapperProfile : Profile
        {
            public CustomMapperProfile()
            {
                CreateMap<CustomEntity, CustomViewModel>();
            }
        }

        public class CustomEntity
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CustomViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}