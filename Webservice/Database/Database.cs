using Core.Exceptions;
using Database.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Database
{
    public class Database<T> : IDatabase<T>
    {
        private readonly IOptions<DatabaseSettings> _database;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongodatabase;

        public SslSettings sslSettings
        {
            get
            {
                return new SslSettings()
                {
                    ClientCertificates = new[] { new X509Certificate2(_database.Value.MongoCertificatePath, _database.Value.MongoCertificatePassword) },
                    CheckCertificateRevocation = true
                };
            }
        }

        private MongoClientSettings mongoClientSettings
        {
            get
            {
                return new MongoClientSettings()
                {
                    Scheme = ConnectionStringScheme.MongoDB,
                    Server = new MongoServerAddress(_database.Value.MongoServer, _database.Value.MongoPort),
                    SslSettings = sslSettings,
                };
            }
        }
        public Database(IOptions<DatabaseSettings> database)
        {
            _database = database;

            _mongoClient = new MongoClient(mongoClientSettings);
            _mongodatabase = _mongoClient.GetDatabase(_database.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection(string name)
        {
            ArgumentsValidator.ThrowIfNull(name, nameof(name));

            return _mongodatabase.GetCollection<T>(name);
        }
    }
}
