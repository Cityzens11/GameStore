using FluentValidation.Results;
using GameStore.Common.Exceptions;
using GameStore.Common.Responses;

namespace GameStore.Common.Extensions;

public static class ErrorResponseExtensions
{
    /// <summary>
    /// Make error response from ValidationResult
    /// </summary>
    public static ErrorResponse ToErrorResponse(this ValidationResult data)
    {
        var res = new ErrorResponse()
        {
            Message = "",
            FieldErrors = data.Errors.Select(x =>
            {
                var elems = x.ErrorMessage.Split('&');
                var errorName = elems[0];
                var errorMessage = elems.Length > 0 ? elems[1] : errorName;
                return new ErrorResponseFieldInfo()
                {
                    FieldName = x.PropertyName,
                    Message = errorMessage,
                };
            })
        };

        return res;
    }

    /// <summary>
    /// Convert process exception to ErrorResponse
    /// </summary>
    public static ErrorResponse ToErrorResponse(this ProcessException data)
    {
        var res = new ErrorResponse()
        {
            Message = data.Message
        };

        return res;
    }

    /// <summary>
    /// Convert exception to ErrorResponse
    /// </summary>
    public static ErrorResponse ToErrorResponse(this Exception data)
    {
        var res = new ErrorResponse()
        {
            ErrorCode = -1,
            Message = data.Message
        };

        return res;
    }
}

