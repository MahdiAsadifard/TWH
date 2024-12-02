using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common;
using Models.Framework;
using Models.Common;

namespace Models.Models
{
    public class UserRecord : IMongoRecord
    {
        public string CollectionName { get => ModelConstants.CollectionNames.User.ToString(); }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _Id { get; set; }

        [BsonRequired]
        public string Uri
        {
            get
            {
                return CoreUtility.GenerateHashAndExtractFiveChars(this._Id.ToString());
            }
        }

        [BsonRequired]
        public required string FirstName { get; set; }

        [BsonRequired]
        public required string LastName { get; set; }

        [Length(10, 10, ErrorMessage = "phone number must be at leat 10 charachters")]
        [BsonIgnoreIfNull]
        public string? Phone { get; set; }

        private string _email;
        [BsonRequired]
        public required string Email 
        {
            get
            {
                return _email.ToLower();
            }
            set
            {
                _email = value.ToLower();
            }
        }

        [BsonRequired]
        public required byte[] HashPassword { get; set; }

        [BsonRequired]
        public required string Salt { get; set; }

        [BsonIgnoreIfNull]
        public bool Disabled { get; set; } = false;

    }
}
