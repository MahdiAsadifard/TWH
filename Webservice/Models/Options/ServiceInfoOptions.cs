namespace Models.Options
{
    public class ServiceInfoOptions
    {
        public const string OptionName = "ServiceInfo";
        public string DatabaseName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string MongoServer { get; set; } = string.Empty;
        public string MongoPort { get; set; } = string.Empty;
        public string MongoCertificatePath { get; set; } = string.Empty;
        public string MongoCertificatePassword { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
    }
}
