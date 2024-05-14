using AutoMapper;
using BSN.Commons.AutoMapper;
using BSN.Commons.Responses;

namespace BSN.Commons.Tests
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
    }
}