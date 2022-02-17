# Supplier options provider
Provides easier way to add suppliers configuration to the project.


## Usage
Add options provider in `Program.cs` or `Startup.cs` alongside with other services configuration
```c#
services.AddSupplierOptionsProvider(options =>
            {
                options.IdentityClientName = HttpClientNames.AccessTokenClient; // Identity client name (must be configured additionally)
                options.Endpoint = endpoint; // Endpoint for sunpu service in form: https://sunpu-url/api/1.0/suppliers
                options.StorageTimeout = TimeSpan.FromSeconds(60);
                options.UpdaterInterval = TimeSpan.FromSeconds(60);
            });
```

Inject to your service
```c#
public class SomeService
{
    public SomeService(ISupplierOptionsStorage supplierStorage)
    {}
}
```

