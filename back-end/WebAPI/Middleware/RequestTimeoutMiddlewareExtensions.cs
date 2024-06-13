public static class RequestTimeoutMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTimeout(this IApplicationBuilder builder, TimeSpan timeout)
    {
        return builder.UseMiddleware<RequestTimeoutMiddleware>(timeout);
    }
}
