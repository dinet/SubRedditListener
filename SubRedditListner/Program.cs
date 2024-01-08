using SubRedditListner;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices((context,services) =>
//    {
//        services.AddHostedService<Worker>();
//        services.AddHostedService<Worker2>();
//    })
//    .UseConsoleLifetime()
//    .Build();

//host.Run();



HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<Worker2>();
builder.Services.AddHttpClient();

IHost host = builder.Build();

host.Run();