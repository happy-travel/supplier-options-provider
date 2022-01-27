using System;

namespace HappyTravel.SupplierOptionsProvider
{
    public class Configuration
    {
        public string HttpClientName { get; set; }
        public string Endpoint { get; set; }
        public TimeSpan StorageTimeout { get; set; }
        public TimeSpan UpdaterInterval { get; set; }
    }
}