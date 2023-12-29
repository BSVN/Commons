using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSN.Commons.PresentationInfrastructure
{
    /// <summary>
    /// Represents a single response of a command/query service.
    /// </summary>
    public interface IResponse
    {
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

    /// <summary>
    /// Represents a single response of a command/query service with additional informations about invalid items.
    /// </summary>
    /// <typeparam name="ValidationResultType"></typeparam>
    public interface IResponse<ValidationResultType> : IResponse
    {
        /// <summary>
        /// Invalid items of the request object.
        /// </summary>
        IList<ValidationResultType> InvalidItems { get; set; }
    }
}
