using ArxOne.MrAdvice.Advice;
using BSN.Commons.PresentationInfrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Linq;

namespace BSN.Commons.AppServiceInfrastructure
{
    public class ValidationAdvice : Attribute, IMethodAdvice
    {
        readonly bool validateAllProperties;

        public ValidationAdvice(bool validateAllProperties = true)
        {
            this.validateAllProperties = validateAllProperties;
        }

        public void Advise(MethodAdviceContext context)
        {
            Type returnType = (context.TargetMethod as MethodInfo).ReturnType;

            if (!returnType.IsSubclassOf(typeof(ResponseBase)))
                throw new Exception("ValidationAdvice should only use at service methods with subclass of ResponseBase return type.");

            if (context.Arguments.Count != 1)
                throw new Exception("Service methods should have one and only one argument as subclass of RequestBase");

            object requestObject = context.Arguments[0];

            if (!requestObject.GetType().GetInterfaces().Contains(typeof(IRequestBase)))
                throw new Exception("Service methods should have one and only one argument as subclass of RequestBase");

            IList<ValidationResult> invalidItems = new List<ValidationResult>();

            bool objectIsvalid = Validator.TryValidateObject(requestObject, new ValidationContext(requestObject, null, null), invalidItems, validateAllProperties);

            if (!objectIsvalid)
            {
                object instance = Activator.CreateInstance(returnType);
                (instance as ResponseBase).StatusCode = ResponseStatusCode.BadRequest;
                (instance as ResponseBase).InvalidItems = invalidItems;
                context.ReturnValue = instance;
            }
            else
                context.Proceed();
        }
    }
}
