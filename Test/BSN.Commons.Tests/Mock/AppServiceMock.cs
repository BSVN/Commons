using BSN.Commons.AppServiceInfrastructure;
using BSN.Commons.PresentationInfrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace BSN.Commons.Test.Mock
{
    public class AppServiceMock
    {
        public class ResponseMessage : ResponseBase { }
        public class RequestMessage : IRequestBase { }

        public class RequestMessageWithValidationRequired : IRequestBase
        {
            [Required]
            public string strProperty { get; set; }

            [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
            public int intProperty { get; set; }
        }

        [ValidationAdvice]
        public ResponseMessage ValidMethod(RequestMessage requestBase)
        {
            return new ResponseMessage() { StatusCode = ResponseStatusCode.OK };
        }


        [ValidationAdvice]
        public ResponseMessage ValidMethodWithValidationRequired(RequestMessageWithValidationRequired requestBase)
        {
            return new ResponseMessage() { StatusCode = ResponseStatusCode.OK };
        }


        [ValidationAdvice]
        public void InvalidMethodWithReturnVoid(RequestMessage requestBase)
        {

        }


        [ValidationAdvice]
        public ResponseMessage InvalidMethodWithoutInputArgumant()
        {
            return new ResponseMessage();
        }


        [ValidationAdvice]
        public ResponseMessage InvalidMethodWithMultipleInputArgumant(RequestMessage requestBase, string strArgumant)
        {
            return new ResponseMessage();
        }
    }
}
