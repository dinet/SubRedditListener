using Microsoft.Extensions.Options;
using SubRedditListner;
using SubRedditListner.Services;
using System.Net.Http.Headers;
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
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddHostedService<Worker2>();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IRedditAuthClient, RedditAuthClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                            client.BaseAddress = new Uri(apiSettings.AccessUrl);
                            var byteArray = Encoding.ASCII.GetBytes($"{apiSettings.ClientId}:{apiSettings.ClientSecret}");
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        });

        builder.Services.AddHttpClient<IRedditPostClient, RedditPostClient>()
                        .ConfigureHttpClient((serviceProvider, client) =>
                        {
                            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
                            client.BaseAddress = new Uri(apiSettings.AccessUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", apiSettings.UserAgent);
                        });


        IHost host = builder.Build();

        host.Run();
    }
}