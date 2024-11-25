using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Model
{
    public class DatabaseSettings
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string MongoServer { get; set; }
        public required int MongoPort { get; set; }
        public required string MongoCertificatePath { get; set; }
        public required string MongoCertificatePassword { get; set; }
    }
}
