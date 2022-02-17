using System;

namespace HappyTravel.SupplierOptionsProvider
{
    public class SupplierOptionsProviderConfiguration
    {
        public string Endpoint { get; set; } = string.Empty;
        public TimeSpan StorageTimeout { get; set; }
        public TimeSpan UpdaterInterval { get; set; }
        public string IdentityClientName { get; set; } = string.Empty;
    }
}