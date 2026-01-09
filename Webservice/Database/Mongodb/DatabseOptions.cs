namespace Models.Options
{
    public class DatabseOptions
    {
        public const string OptionName = "Database";
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string MongoServer { get; set; }
        public required int MongoPort { get; set; }
        public required string MongoCertificatePath { get; set; }
        public required string MongoCertificatePassword { get; set; }

    }
}
