# Sunpu client
Provider easiest way to add suppliers configuration to project

## Usage
Add configuration provider in configuration section in Program.cs
```c#
.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddEnvironmentVariables();
    config.AddSuppliersConfiguration(endpoint);
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