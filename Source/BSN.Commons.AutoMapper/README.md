# BSN.Commons.AutoMapper

This package contains some facilities for using AutoMapper in Enterprise Applications.

AutoMapper is a popular library for mapping objects from one Model to Another. It simplifies the process of mapping complex objects and reduces the amount of code needed to perform these mappings stuffs.

**BSN.Commons.Automapper** is a package that provides some predefined mappings for **BSN.Commons.PresentationInfrastructure** Models.
This helps **BSN.Commons** users to skip writing required mapping to dealing with these Models.

### 1. Installation
To use this package, you need to first install it on your web api or presentation layer. You can do this by running the following command (in package manager console):
```
Install-Package BSN.Commons.AutoMapper
```

### 2. Add Your required mapping profiles
To use these predefined mapping profiles and injecting your preferred profiles you just need to add following line in the `ServiceCollection`:
```
services.AddCommonsAutoMapper(config => config.AddProfile<YourMappingProfile>());
```
or 
```
services.AddCommonsAutoMapper(config => 
{
    config.AddProfile<YourFirstMappingProfile>();
    config.AddProfile<YourSecondMappingProfile>();
});
```

### 3. Predefined mapping profile:

Provided built in mapping profile contains following mappings:

#### PagedEntityCollectionToMetaDataConverter:
A default converter which converts `PagedEntityCollection<TDomain>` to `PaginationMetadata`.
#### GenericIEnumerableToCollectionViewModelConverter: 
A generic default converter which converts `IEnumerable<TDomain>` to `CollectionViewModel<TViewModel>`

### 4. Example Usage

#### PagedEntityCollectionToMetaDataConverter
```
var pagedEntities = new PagedEntityCollection<Product>(products, 1, 10, 100);
var paginationMetadata = mapper.Map<PaginationMetadata>(pagedEntities);
```

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

`BSN.Commons.AutoMapper` is Copyright © 2024 BSN and other contributors under the BSN license.