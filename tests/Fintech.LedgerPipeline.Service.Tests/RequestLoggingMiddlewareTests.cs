using Fintech.LedgerPipeline.Service.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fintech.LedgerPipeline.Service.Tests;

[TestClass]
public class RequestLoggingMiddlewareTests
{
    [TestMethod]
    public async Task InvokeAsync_WithCorrelationIdHeader_SetsResponseHeaderAndContextItem()
    {
        var middleware = new RequestLoggingMiddleware(
            _ => Task.CompletedTask,
            NullLogger<RequestLoggingMiddleware>.Instance);

        var context = new DefaultHttpContext();
        context.Request.Headers["X-Correlation-Id"] = "corr-123";

        await middleware.InvokeAsync(context);

        Assert.AreEqual("corr-123", context.Response.Headers["X-Correlation-Id"].ToString());
        Assert.AreEqual("corr-123", context.Items["CorrelationId"]);
    }
}
