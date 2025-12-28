using Core.Exceptions;
using Models.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Database
{
    public class Database<T> : IDatabase<T>
    {
        private readonly IOptions<DatabseOptions> _database;
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _mongodatabase;
        
        public Database(IOptions<DatabseOptions> database)
        {
            _database = database;

            _mongoClient = new MongoClient(MongoClientSettings);
            _mongodatabase = _mongoClient.GetDatabase(_database.Value.DatabaseName);
        }

        #region Public Methods
        public SslSettings SslSettings
        {
            get
            {
                return new SslSettings()
                {
                    ClientCertificates = [ new X509Certificate2(_database.Value.MongoCertificatePath, _database.Value.MongoCertificatePassword) ],
                    CheckCertificateRevocation = true
                };
            }
        }

        public IMongoCollection<T> GetCollection(string name)
        {
            ArgumentsValidator.ThrowIfNull(name, nameof(name));

            return _mongodatabase.GetCollection<T>(name);
        }

        #endregion

        #region Private Methods

        private MongoClientSettings MongoClientSettings
        {
            get
            {
                return new MongoClientSettings()
                {
                    Scheme = ConnectionStringScheme.MongoDB,
                    Server = new MongoServerAddress(_database.Value.MongoServer, _database.Value.MongoPort),
                    SslSettings = SslSettings,
                };
            }
        }

        #endregion
    }
}
