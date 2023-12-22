using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSN.Commons.PresentationInfrastructure
{
    public interface IResponse<ValidationResultType>
    {
        /// <summary>
        /// Invalid items of the request object.
        /// </summary>
        IList<ValidationResultType> InvalidItems { get; set; }

        /// <summary>
        /// Distinction between successful and unsuccessful result.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Human-readable message for the End-User.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Corresponding HttpStatusCode.
        /// </summary>
        ResponseStatusCode StatusCode { get; set; }
    }
}
