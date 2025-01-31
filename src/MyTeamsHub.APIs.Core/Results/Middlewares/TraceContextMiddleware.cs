using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsHub.APIs.Core.Results.Middleware;

public class TraceContextMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, Func<Task> next)
    {
        await next();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.Headers.Add("X-Trace-Id", "Hello World");
        await next.Invoke(context);
    }
}