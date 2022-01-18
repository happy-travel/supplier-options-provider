using HappyTravel.SunpuClient.ConfigurationProvider;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Add all the required data here to test this configuration provider
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