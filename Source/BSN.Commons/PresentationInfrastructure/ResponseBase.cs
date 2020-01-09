using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BSN.Commons.PresentationInfrastructure
{
    public abstract class ResponseBase
    {
        public virtual bool IsSuccess => StatusCode == ResponseStatusCode.OK;

        public string Message { get; set; }

        public ResponseStatusCode StatusCode { get; set; }

        public IList<ValidationResult> InvalidItems { get; set; }
    }
}
