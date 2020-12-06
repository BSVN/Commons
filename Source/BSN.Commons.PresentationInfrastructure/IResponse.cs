using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSN.Commons.PresentationInfrastructure
{
    public interface IResponse<ValidationResultType>
    {
        IList<ValidationResultType> InvalidItems { get; set; }
        bool IsSuccess { get; }
        string Message { get; set; }
        ResponseStatusCode StatusCode { get; set; }
    }
}
