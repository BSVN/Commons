using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BSN.Commons.PresentationInfrastructure
{
    public abstract class ResponseBase : IResponse<ValidationResult>
    {
        public virtual bool IsSuccess => StatusCode == ResponseStatusCode.OK;

        public string Message { get; set; }

        public ResponseStatusCode StatusCode { get; set; }

        public IList<ValidationResult> InvalidItems { get; set; }
    }
}
