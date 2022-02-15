using System.Collections.Generic;

namespace HappyTravel.SupplierOptionsProvider
{
    public class SlimSupplier
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
        public bool IsEnabled { get; init; }
        public string ConnectorUrl { get; init; } = string.Empty;
        public string? ConnectorGrpcEndpoint { get; set; }
        public bool IsMultiRoomFlowSupported { get; init; }
        public Dictionary<string, string>? CustomHeaders { get; init; }
    }
}