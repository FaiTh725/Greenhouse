using Application.Shared.Exceptions;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Ingest.Elasticsearch;
using Elastic.Serilog.Sinks;
using Serilog;
using Elastic.Transport;
using Greenhouse.API.Configuration;

namespace Greenhouse.API.Extentions
{
    public static class AppExtention
    {
        public static IServiceCollection ConfigureAppServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSerilogProvider(configuration)
                .ConfigureCors(configuration);

            return services;
        }

        private static IServiceCollection ConfigureCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var clientUrl = configuration
                .GetValue<string>("ClientUrl") ??
                throw new AppConfigurationException("Client url");

            services.AddCors(options =>
            {
                options.AddPolicy("Client", policy =>
                {
                    policy
                        .WithOrigins(clientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        private static IServiceCollection AddSerilogProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var elasticConf = configuration
                .GetSection("ElasticSearchConf")
                .Get<ElasticConf>() ??
                throw new AppConfigurationException("ElasticSearch Configuration");

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new[] { new Uri(elasticConf.Uri) },
                opts =>
                {
                    opts.DataStream = new DataStreamName("logs", "greenhouse-api-greenhouse", "greenhouse");
                    opts.BootstrapMethod = BootstrapMethod.Failure;
                }, transport =>
                {
                    transport.Authentication(new BasicAuthentication(
                        elasticConf.User, elasticConf.Password));
                    transport.MaxRetryTimeout(TimeSpan.FromSeconds(120));
                    transport.MaximumRetries(3);
                })
                .CreateLogger();

            return services;
        }
    }
}
