using HappyTravel.SunpuClient.ConfigurationProvider;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Testing SunpuClient as a configuration provider
var config = new ConfigurationBuilder()
    .AddSuppliersConfiguration(
        "",
        "",
        "",
        "",
        ""
        )
    .Build();

var debug = config.GetDebugView();

app.Run();