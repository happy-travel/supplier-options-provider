# Sunpu client
Provides easier way to add suppliers configuration to project.


## Usage
Add configuration provider in configuration section in Program.cs alongside with identity server information
```c#
.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddEnvironmentVariables();
    config.AddSuppliersConfiguration(sunpuEndpoint, identityUrl, clientId, clientSecret, clientScope);
})
```
Add configuration in Startup.cs
```c#
services.ConfigureSuppliers(configuration);
```
Inject to your service
```c#
public class SomeService
{
    public SomeService(IOptions<SupplierOptions> options)
    {}
}
```

Or use IOptionsMonitor<T> for up-to-date information about suppliers
    
```c#
public class SomeService
{
    public SomeService(IOptionsMonitor<SupplierOptions> options)
    {}
}
```
