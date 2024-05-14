////using AutoMapper;
////using BSN.Commons.Responses;
////using Moq;

////namespace BSN.Commons.AutoMapper.Tests
////{
////    [TestFixture]
////    public class CommonMapperProfileTests : AutoMapperTestBase
////    {
////        private ResolutionContext context; 
////        private Mock<IMapper> mapperMock;
////        private CommonMapperProfile sut;

////        public CommonMapperProfileTests()
////        {
////            mapperMock = new Mock<IMapper>();
////            sut = new CommonMapperProfile();
////            context = new ResolutionContext { Mapper = mapperMock.Object };
////        }

////        [Test]
////        public void PagedEntityCollectionToMetaDataConverter_MapsPagedEntityCollectionToPaginationMetadata()
////        {
////            // Arrange
////            var pagedEntityCollection = new PagedEntityCollection<Foo>
////            {
////                CurrentPage = 2,
////                PageSize = 10,
////                RecordCount = 50
////            };

////            var expectedMetadata = new PaginationMetadata
////            {
////                Page = 2,
////                PageCount = 10,
////                PageSize = 10,
////                RecordCount = 50
////            };

////            mapperMock.Setup(m => m.Map<IEnumerable<FooViewModel>>(pagedEntityCollection))
////                .Returns(new FooViewModel[0]);

////            var converter = sut.GetType()
////                .GetField("PagedEntityCollectionToMetaDataConverter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
////                .GetValue(sut) as ITypeConverter<PagedEntityCollection<Foo>, PaginationMetadata>;

////            // Act
////            var result = converter.Convert(pagedEntityCollection, null, context);

////            // Assert
////            Assert.AreEqual(expectedMetadata.Page, result.Page);
////            Assert.AreEqual(expectedMetadata.PageCount, result.PageCount);
////            Assert.AreEqual(expectedMetadata.PageSize, result.PageSize);
////            Assert.AreEqual(expectedMetadata.RecordCount, result.RecordCount);
////        }

////        [Test]
////        public void GenericIEnumerableToCollectionViewModelConverter_MapsIEnumerableToCollectionViewModel()
////        {
////            // Arrange
////            var foos = new List<Foo>
////        {
////            new Foo { Id = 1, Name = "Foo 1" },
////            new Foo { Id = 2, Name = "Foo 2" },
////            new Foo { Id = 3, Name = "Foo 3" }
////        };

////            var expectedViewModels = new List<FooViewModel>
////        {
////            new FooViewModel { Id = 1, Name = "Foo 1" },
////            new FooViewModel { Id = 2, Name = "Foo 2" },
////            new FooViewModel { Id = 3, Name = "Foo 3" }
////        };

////            mapperMock.Setup(m => m.Map<IEnumerable<FooViewModel>>(foos))
////                .Returns(expectedViewModels);

////            var converter = sut.GetType()
////                .GetField("GenericIEnumerableToCollectionViewModelConverter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
////                .GetValue(sut) as ITypeConverter<IEnumerable<Foo>, CollectionViewModel<FooViewModel>>;

////            // Act
////            var result = converter.Convert(foos, null, new ResolutionContext { Services = mapperMock.Object });

////            // Assert
////            Assert.AreEqual(expectedViewModels.Count, result.Items.Count());
////            Assert.IsTrue(result.Items, vm => Assert.Contains(vm, expectedViewModels));
////        }

////        private class Foo
////        {
////            public int Id { get; set; }
////            public string Name { get; set; }
////        }

////        private class FooViewModel
////        {
////            public int Id { get; set; }
////            public string Name { get; set; }
////        }
////    }
////}

////// C# code to solve the given problem
////using AutoMapper;
////using BSN.Commons.AutoMapper.Tests;
////using BSN.Commons.AutoMapper;
////using BSN.Commons.Responses;
////using BSN.Commons;
////using Moq;
////using NUnit.Framework;
////using System.Collections.Generic;
////using System.Linq;

////[TestFixture]
////public class CommonMapperProfileTests : AutoMapperTestBase
////{
////    private Mock<IMapper> mapperMock;
////    private CommonMapperProfile sut;

////    public CommonMapperProfileTests()
////    {
////        mapperMock = new Mock<IMapper>();
////        sut = new CommonMapperProfile();
////    }

////    [Test]
////    public void PagedEntityCollectionToMetaDataConverter_MapsPagedEntityCollectionToPaginationMetadata()
////    {
////        // Arrange
////        var pagedEntityCollection = new PagedEntityCollection<Foo>
////        {
////            CurrentPage = 2,
////            PageSize = 10,
////            RecordCount = 50
////        };

////        var expectedMetadata = new PaginationMetadata
////        {
////            Page = 2,
////            PageCount = 10,
////            PageSize = 10,
////            RecordCount = 50
////        };

////        mapperMock.Setup(m => m.Map<IEnumerable<FooViewModel>>(pagedEntityCollection))
////            .Returns(new FooViewModel[0]);

////        var converter = sut.GetType()
////            .GetField("PagedEntityCollectionToMetaDataConverter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
////            .GetValue(sut) as ITypeConverter<PagedEntityCollection<Foo>, PaginationMetadata>;

////        // Act
////        var result = converter.Convert(pagedEntityCollection, null, null);

////        // Assert
////        Assert.AreEqual(expectedMetadata.Page, result.Page);
////        Assert.AreEqual(expectedMetadata.PageCount, result.PageCount);
////        Assert.AreEqual(expectedMetadata.PageSize, result.PageSize);
////        Assert.AreEqual(expectedMetadata.RecordCount, result.RecordCount);
////    }

////    [Test]
////    public void GenericIEnumerableToCollectionViewModelConverter_MapsIEnumerableToCollectionViewModel()
////    {
////        // Arrange
////        var foos = new List<Foo>
////    {
////        new Foo { Id = 1, Name = "Foo 1" },
////        new Foo { Id = 2, Name = "Foo 2" },
////        new Foo { Id = 3, Name = "Foo 3" }
////    };

////        var expectedViewModels = new List<FooViewModel>
////    {
////        new FooViewModel { Id = 1, Name = "Foo 1" },
////        new FooViewModel { Id = 2, Name = "Foo 2" },
////        new FooViewModel { Id = 3, Name = "Foo 3" }
////    };

////        mapperMock.Setup(m => m.Map<IEnumerable<FooViewModel>>(foos))
////            .Returns(expectedViewModels);

////        var converter = sut.GetType()
////            .GetField("GenericIEnumerableToCollectionViewModelConverter", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
////            .GetValue(sut) as ITypeConverter<IEnumerable<Foo>, CollectionViewModel<FooViewModel>>;

////        // Act
////        var result = converter.Convert(foos, null, null);

////        // Assert
////        Assert.AreEqual(expectedViewModels.Count, result.Items.Count());
////        Assert.IsTrue(expectedViewModels.All(vm => result.Items.Contains(vm)));
////    }

////    private class Foo
////    {
////        public int Id { get; set; }
////        public string Name { get; set; }
////    }

////    private class FooViewModel
////    {
////        public int Id { get; set; }
////        public string Name { get; set; }
////    }
////}

//using AutoMapper;
//using NUnit.Framework;
//using BSN.Commons.AutoMapper;
//using BSN.Commons.Responses;

//namespace BSN.Commons.Tests.AutoMapper
//{
//    [TestFixture]
//    public class CommonMapperProfileTests
//    {
//        [Test]
//        public void PagedEntityCollectionToMetaDataConverter_ConvertsCorrectly()
//        {
//            // Arrange
//            var profile = new CommonMapperProfile();
//            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
//            var mapper = new Mapper(configuration);
//            var pagedEntityCollection = new PagedEntityCollection<int>
//            {
//                CurrentPage = 1,
//                PageSize = 10,
//                RecordCount = 100
//            };

//            // Act
//            var result = mapper.Map<PaginationMetadata>(pagedEntityCollection);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(pagedEntityCollection.CurrentPage, result.Page);
//            Assert.AreEqual(pagedEntityCollection.PageSize, result.PageSize);
//            Assert.AreEqual(pagedEntityCollection.RecordCount, result.RecordCount);
//        }

//        [Test]
//        public void GenericIEnumerableToCollectionViewModelConverter_ConvertsCorrectly()
//        {
//            // Arrange
//            var profile = new CommonMapperProfile();
//            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
//            var mapper = new Mapper(configuration);
//            var items = new List<int> { 1, 2, 3 };

//            // Act
//            var result = mapper.Map<CollectionViewModel<int>>(items);

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(items.Count, result.Items.Count);
//        }
//    }

//}

using AutoMapper;
using NUnit.Framework;
using BSN.Commons.AutoMapper;
using BSN.Commons.Responses;
using System.Collections.Generic;

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