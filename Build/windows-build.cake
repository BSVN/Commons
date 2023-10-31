// ITNOA

// See https://cakebuild.net/docs/writing-builds/preprocessor-directives/define
#define _WINDOWS_

#tool "nuget:?package=NUnit.ConsoleRunner&version=3.11.1"
#tool "nuget:?package=vswhere&version=2.8.4"

#addin nuget:?package=Cake.EntityFramework6&version=1.0.1

// https://github.com/cake-build/cake/issues/1860#issuecomment-453080685
#load "build.cake"

// To exclusively build system telephony modules, comment top line and uncomment:
// #load "build-call-control-system.cake"