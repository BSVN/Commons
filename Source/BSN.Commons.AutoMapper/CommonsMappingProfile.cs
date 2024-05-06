using AutoMapper;
using BSN.Commons.Responses;

namespace BSN.Commons.AutoMapper
{
    public class CommonMapperProfile : Profile
    {
        public CommonMapperProfile()
        {
            CreateMap(typeof(PagedEntityCollection<>), typeof(PaginationMetadata)).ConvertUsing(typeof(PagedEntityCollectionToMetaDataConverter<>));

            CreateMap(typeof(IEnumerable<>), typeof(CollectionViewModel<>)).ConvertUsing(typeof(GenericIEnumerableToCollectionViewModelConverter<,>));
        }

        private class PagedEntityCollectionToMetaDataConverter<TDomain> : ITypeConverter<PagedEntityCollection<TDomain>, PaginationMetadata>
        {
            public PaginationMetadata Convert(PagedEntityCollection<TDomain> source, PaginationMetadata destination, ResolutionContext context)
            {
                return new PaginationMetadata()
                {
                    Page = source.CurrentPage,
                    PageCount = source.PageSize,
                    PageSize = source.PageSize,
                    RecordCount = source.RecordCount
                };
            }
        }

        private class GenericIEnumerableToCollectionViewModelConverter<TDomain, TViewModel> : ITypeConverter<IEnumerable<TDomain>, CollectionViewModel<TViewModel>>
        {
            public CollectionViewModel<TViewModel> Convert(IEnumerable<TDomain> source, CollectionViewModel<TViewModel> destination, ResolutionContext context)
            {
                return new CollectionViewModel<TViewModel>
                {
                    Items = context.Mapper.Map<IEnumerable<TViewModel>>(source)
                };
            }
        }
    }
}
