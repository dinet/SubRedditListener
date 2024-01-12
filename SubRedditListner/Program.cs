using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using SubRedditListner;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args)
            .Build();
        var ApiConfig = new ApiConfig();
        configuration.GetSection("ApiConfig").Bind(ApiConfig);
        builder.Services.AddLogging();

        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IRedditAuthClient, RedditAuthClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(ApiConfig.TokenUrl);
                            var byteArray = Encoding.ASCII.GetBytes($"{ApiConfig.ClientId}:{ApiConfig.ClientSecret}");
                            client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(byteArray)}");
                            client.DefaultRequestHeaders.Add("User-Agent", ApiConfig.AgentName);
                        });

        builder.Services.AddHttpClient<IRedditPostClient, RedditPostClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(ApiConfig.BaseUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", ApiConfig.AgentName);
                        });

        builder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
        builder.Services.AddTransient<IRateLimitedHttpClient, RateLimitedHttpClient>();
        builder.Services.AddSingleton<ISubredditRepository, SubredditRepository>();

        builder.Services.AddHostedService<StatRetriverHostedService>();

        IHost host = builder.Build();
        var rateLimiter = host.Services.GetService<IRateLimitedHttpClient>();
        rateLimiter?.SendAsync($"/r/{ApiConfig.SubRedditName}/new");
        host.Run();

    }
}