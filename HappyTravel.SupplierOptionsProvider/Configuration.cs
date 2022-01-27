namespace HappyTravel.SunpuClient
{
    public class Configuration
    {
        public string HttpClientName { get; set; }
        public string EndPoint { get; set; }
        public int StorageTimeoutInSeconds { get; set; }
        public int UpdaterIntervalInSeconds { get; set; }
    }
}