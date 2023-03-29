using GameStore.Common;
using GameStore.Common.Exceptions;
using GameStore.Common.Extensions;
using GameStore.Common.Responses;
using System.Text.Json;

namespace GameStore.Web.Middlewares;

public class ExceptionsMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionsMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string error = null;
        try
        {
            await next.Invoke(context);
        }
        catch (ProcessException pe)
        {
            error = pe.Message;
        }
        catch (Exception pe)
        {
            error = pe.Message;
        }
        finally
        {
            if (error is not null)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}