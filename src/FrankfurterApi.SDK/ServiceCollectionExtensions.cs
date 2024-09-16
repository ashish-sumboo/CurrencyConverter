namespace FrankfurterApi.SDK
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Polly;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Extensions.Http;
    using Polly.Timeout;

    public static class ServiceCollectionExtensions
    {
        private const int DefaultTimeoutMillisecond = 3000;

        public static IServiceCollection AddCurrencyConverterApi(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var settings = configuration.GetSection("FrankfurterAPI").Get<ClientSettings>();

            if (settings == null)
            {
                throw new ArgumentException("Configuration not found");
            }

            if (string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                throw new ArgumentException($"{nameof(settings.BaseUrl)}should contain a valid URL");
            }

            if (settings.RetryCount <= 0)
            {
                throw new ArgumentException($"{nameof(settings.RetryCount)} should be greater than zero");
            }

            if (settings.RetryIntervalMilliseconds <= 0)
            {
                throw new ArgumentException($"{nameof(settings.RetryIntervalMilliseconds)} should be greater than zero");
            }

            services
                .AddSingleton(settings)
                .AddHttpClient<ICurrencyConverterClient, CurrencyConverterClient>(client => client.BaseAddress = new Uri(settings.BaseUrl))
                .AddPolicyHandler(GetRetryPolicy(
                                      settings.RetryIntervalMilliseconds,
                                      settings.RetryCount,
                                      settings.TimeoutMilliseconds == 0 ? DefaultTimeoutMillisecond : settings.TimeoutMilliseconds));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryInterval, int retryCount, int timeout)
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(retryInterval), retryCount);
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(timeout));
            return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .Or<TimeoutRejectedException>()
                   .WaitAndRetryAsync(delay)
                   .WrapAsync(timeoutPolicy);
        }
    }
}