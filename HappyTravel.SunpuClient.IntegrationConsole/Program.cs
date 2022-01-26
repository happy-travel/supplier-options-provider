using HappyTravel.SunpuClient.ConfigurationProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSunpuClient(o =>
{
    o.EndPoint = "http://localhost:5990/api/1.0/suppliers";
    o.HttpClientName = "Edo";
});