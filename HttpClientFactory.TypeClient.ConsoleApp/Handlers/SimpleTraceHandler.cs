using System.Diagnostics;
using Microsoft.Extensions.Logging;


namespace HttpClientFactory.TypeClient.ConsoleApp.Handlers;

/// <summary>
/// A custom HTTP delegating handler that intercepts outgoing HTTP requests,
/// attaches a unique X-Request-Id header, and logs request lifecycle metrics (elapsed time and status code).
/// </summary>
public sealed class SimpleTraceHandler(ILogger<SimpleTraceHandler> logger) : DelegatingHandler
{
    private const string RequestIdHeaderName = "X-Request-Id";

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains(RequestIdHeaderName))
        {
            var newRequestId = Guid.NewGuid().ToString("N");
            request.Headers.Add(RequestIdHeaderName, newRequestId);
        }

        var requestId = request.Headers.GetValues(RequestIdHeaderName).FirstOrDefault() ?? "unknown";
        var requestUri = request.RequestUri?.ToString() ?? "unknown";

        // 2. Log when the HTTP request starts
        logger.LogInformation(
            "[SimpleTraceHandler] Outgoing HTTP {Method} request to '{Uri}' [RequestId: {RequestId}]",
            request.Method,
            requestUri,
            requestId);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            stopwatch.Stop();

            logger.LogInformation(
                "[SimpleTraceHandler] Received HTTP {StatusCode} ({(int)StatusCode}) from '{Uri}' in {ElapsedMs:F2} ms [RequestId: {RequestId}]",
                response.StatusCode,
                (int)response.StatusCode,
                requestUri,
                stopwatch.Elapsed.TotalMilliseconds,
                requestId);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(
                ex,
                "[SimpleTraceHandler] HTTP {Method} request to '{Uri}' failed after {ElapsedMs:F2} ms [RequestId: {RequestId}]: {ErrorMessage}",
                request.Method,
                requestUri,
                stopwatch.Elapsed.TotalMilliseconds,
                requestId,
                ex.Message);

            throw;
        }
    }
}
