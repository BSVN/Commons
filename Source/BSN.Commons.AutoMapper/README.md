# BSN.Commons.AutoMapper

ITNOA

This package contains some facilities for using AutoMapper in Enterprise way

AutoMapper is a popular library for mapping objects from one type to another. It simplifies the process of mapping complex objects and reduces the amount of code needed to perform these mappings.

### 1. Install required Package
To use this package, you need to first add it in your client service. You can do this by running the following command:
```
- Install-Package BSN.Commons.AutoMapper
```

### 2. Add required services
Once you've installed the package, you need to add the following code to your Startup.cs file:
```
- services.AddAutoMapper(config => config.AddProfile<AppServiceViewMapperProfile>());
```

### 3.How to use in your service?
in your application code, execute the mappings:
```
var productDto = mapper.Map<ProductDto>(product);
var orderDto = mapper.Map<OrderDto>(order);
```

### 4.Where CommonsMappingProfile for example is:

The CommonMapperProfile class is a profile for AutoMapper that defines custom type conversions. It contains the following:

- #### PagedEntityCollectionToMetaDataConverter:
A custom type converter that converts PagedEntityCollection<TDomain> to PaginationMetadata.
- #### GenericIEnumerableToCollectionViewModelConverter: 
A generic custom type converter that converts IEnumerable<TDomain> to CollectionViewModel<TViewModel>

The PagedEntityCollectionToMetaDataConverter is a custom type converter that converts PagedEntityCollection<TDomain> to PaginationMetadata. It has a generic type parameter TDomain that represents the type of the domain entities in the PagedEntityCollection
Here's an example of how to use this custom type converter: 

#### PagedEntityCollectionToMetaDataConverter
```
var pagedEntities = new PagedEntityCollection<Product>(products, 1, 10, 100);
var paginationMetadata = mapper.Map<PaginationMetadata>(pagedEntities);
```

The GenericIEnumerableToCollectionViewModelConverter is a generic custom type converter that converts IEnumerable<TDomain> to CollectionViewModel<TViewModel>. It has two generic type parameters: TDomain represents the type of the domain entities in the IEnumerable, and TViewModel represents the type of the view models that will be created from the domain entities.
Here's an example of how to use this custom type converter: 

#### GenericIEnumerableToCollectionViewModelConverter
```
var products = new List<Product>
{
    new Product { Id = 1, Name = "Product 1" },
    new Product { Id = 2, Name = "Product 2" },
    new Product { Id = 3, Name = "Product 3" }
};

var collectionViewModel = mapper.Map<CollectionViewModel<ProductViewModel>>(products);
```

These profiles cover some of the most common use cases for mapping objects in enterprise applications. However, if you need to add support for additional complex types, you can create your own profile and add it to the configuration in the Startup.cs file.

BSN.Commons.AutoMapper is Copyright © 2024 Bsn and other contributors under the Bsn license.