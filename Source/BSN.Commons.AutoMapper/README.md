# BSN.Commons.AutoMapper

ITNOA

This package contains some facilities for using AutoMapper in Enterprise Way

AutoMapper is a popular library for mapping objects from one type to another. It simplifies the process of mapping complex objects and reduces the amount of code needed to perform these mappings.

To use this package, you need to first install it in your client service. You can do this by running the following command:
- Install-Package BSN.Commons.AutoMapper

Once you've installed the package, you need to add the following code to your Startup.cs file:
- services.AddAutoMapper(config => config.AddProfile<AppServiceViewMapperProfile>());

1.AppServiceViewMapperProfile: This profile maps objects related to App Services, including:
- AppService to AppServiceViewModel
- AppServiceDto to AppServiceViewModel
- AppServiceViewModel to AppService
- AppServiceViewModel to AppServiceDto

2.UserViewMapperProfile: This profile maps objects related to Users, including:
- User to UserViewModel
- UserDto to UserViewModel
- UserViewModel to User
- UserViewModel to UserDto

3.RoleViewMapperProfile: This profile maps objects related to Roles, including:
- Role to RoleViewModel
- RoleDto to RoleViewModel
- RoleViewModel to Role
- RoleViewModel to RoleDto

These profiles cover some of the most common use cases for mapping objects in enterprise applications. However, if you need to add support for additional complex types, you can create your own profile and add it to the configuration in the Startup.cs file.