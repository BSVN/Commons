# BSN.Commons.AutoMapper

ITNOA

This package contains some facilities for using AutoMapper in Enterprise Way

AutoMapper is a popular library for mapping objects from one type to another. It simplifies the process of mapping complex objects and reduces the amount of code needed to perform these mappings.

To use this package, you need to first install it in your client service. You can do this by running the following command:
- Install-Package BSN.Commons.AutoMapper

Once you've installed the package, you need to add the following code to your Startup.cs file:
- services.AddAutoMapper(config => config.AddProfile<AppServiceViewMapperProfile>());