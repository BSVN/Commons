using BSN.Commons.AppServiceInfrastructure;
using BSN.Commons.PresentationInfrastructure;
using BSN.Commons.Test.Mock;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Tests.AppServiceInfrastructure
{
    public class ValidationAdviceTest
    {
        [Test]
        public void Validate_ValidServiceMethod_ShouldSuccess()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            ResponseBase validResponse = serviceMock.ValidMethod(new AppServiceMock.RequestMessage());

            Assert.True(validResponse.IsSuccess);
        }


        [Test]
        public void Validate_InvalidMethodWithReturnVoid_ShouldThrowException()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            Assert.Throws<Exception>(() => serviceMock.InvalidMethodWithReturnVoid(new AppServiceMock.RequestMessage()));
        }


        [Test]
        public void Validate_InvalidMethodWithoutInputArgumant_ShouldThrowException()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            Assert.Throws<Exception>(() => serviceMock.InvalidMethodWithoutInputArgumant());
        }


        [Test]
        public void Validate_InvalidMethodWithMultipleInputArgumant_ShouldThrowException()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            Assert.Throws<Exception>(() => serviceMock.InvalidMethodWithMultipleInputArgumant(new AppServiceMock.RequestMessage(), ""));
        }


        [Test]
        public void Validate_ValidServiceMethodWithValidRequestMessage_ShouldSuccess()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            ResponseBase validResponse = serviceMock.ValidMethodWithValidationRequired(new AppServiceMock.RequestMessageWithValidationRequired()
            {
                intProperty = 1,
                strProperty = "test"
            });

            Assert.True(validResponse.IsSuccess);
        }


        [Test]
        public void Validate_ValidServiceMethodWithValidRequestMessage_ShouldNotSuccess()
        {
            AppServiceMock serviceMock = new AppServiceMock();

            ResponseBase validResponse = serviceMock.ValidMethodWithValidationRequired(new AppServiceMock.RequestMessageWithValidationRequired());

            Assert.False(validResponse.IsSuccess);
        }
    }


}
