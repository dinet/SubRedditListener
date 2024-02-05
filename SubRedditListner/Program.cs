using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using SubRedditListner;
using SubRedditListner.DataAccess;
using SubRedditListner.Services;
using SubRedditListner.Configurations;
using System.Text;
using SubRedditListner.Services.Services;
using Microsoft.Extensions.Configuration;

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
        configuration.GetSection(nameof(ApiConfig)).Bind(ApiConfig);
        builder.Services.Configure<ApiConfig>(configuration.GetSection(nameof(ApiConfig)));
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

        builder.Services.AddHttpClient<IRedditGetClient, RedditGetClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(ApiConfig.BaseUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", ApiConfig.AgentName);
                        });
        //Remove additional logging from HttpClient
        builder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
        builder.Services.AddTransient<ISubRedditService, SubRedditService>();
        builder.Services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddSingleton<ISubredditRepository, SubredditRepository>();

        builder.Services.AddHostedService<SubRedditPollerBackgroundService>();
        builder.Services.AddHostedService<StatRetriverBackgroundService>();

        IHost host = builder.Build();

        host.Run();

    }
}