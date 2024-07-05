public class RequestTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timeout;

    public RequestTimeoutMiddleware(RequestDelegate next, TimeSpan timeout)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _timeout = timeout;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var cts = new CancellationTokenSource())
        {
            var timeoutTask = Task.Delay(_timeout, cts.Token);
            var requestTask = _next(context);

            var completedTask = await Task.WhenAny(requestTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
            }
            else
            {
                cts.Cancel(); // Cancel the timeout task if the request completed in time
                await requestTask; // Complete the request
            }
        }
    }
}
