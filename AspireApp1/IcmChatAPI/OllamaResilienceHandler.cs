
 public static class OllamaResilienceHandlersExtensions
{
    public static IServiceCollection AddOllamaResilienceHandlers(this IServiceCollection services)
    {
        services.ConfigureHttpClientDefaults(httpClientBuilder =>
        {
            httpClientBuilder.AddStandardResilienceHandler(config =>
            {
                config.AttemptTimeout.Timeout = TimeSpan.FromMinutes(5); // Timeout for each attempt
                config.CircuitBreaker.SamplingDuration = TimeSpan.FromMinutes(10);
                config.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(10);
            });
        });

        return services;
    }
}