using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SubRedditListner;
using SubRedditListner.Services;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context,services) =>
//    {
//        services.AddHostedService<Worker>();
//        services.AddHostedService<Worker2>();
//    })
//    .UseConsoleLifetime()
//    .Build();

//host.Run();



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

        builder.Services.AddHostedService<Worker>();
        builder.Services.AddHostedService<Worker2>();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IRedditAuthClient, RedditAuthClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(ApiConfig.TokenUrl);
                            var byteArray = Encoding.ASCII.GetBytes($"{ApiConfig.ClientId}:{ApiConfig.ClientSecret}");
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                            client.DefaultRequestHeaders.Add("User-Agent", ApiConfig.AgentName);
                        });

        builder.Services.AddHttpClient<IRedditPostClient, RedditPostClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            client.BaseAddress = new Uri(ApiConfig.TokenUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", ApiConfig.AgentName);
                        });
        builder.Services.AddTransient<IRateLimitedHttpClient, RateLimitedHttpClient>();

        IHost host = builder.Build();

        host.Run();
    }
}