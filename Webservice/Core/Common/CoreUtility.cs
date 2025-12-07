using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Core.Common
{
    public class CoreUtility
    {
        public static byte[] GenerateSHA256HashByte(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                return hashBytes;
            }
        }
        public static string GenerateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert byte array to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Convert each byte to a hex string
                }

                return sb.ToString();
            }
        }

        public static string GenerateHashAndExtractFiveChars(string input)
        {
            string hash = GenerateSHA256Hash(input);
            // only last five chars
            int start = hash.Length - 5;
            int end = hash.Length - start;
            return hash.Substring(start, end);
        }

        public static string GenerateRandomUriFromObjectId(string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId))
            {
                throw new ArgumentNullException(nameof(objectId), "objectId cannot be null.");
            }

            var random = new Random();
            char[] uri = new char[5];
            for (int i = 0; i < uri.Length; i++)
            {
                uri[i] = objectId[random.Next(objectId.ToString().Length)];
            }
            return string.Join(string.Empty, uri);
        }

        public static string SerializeJson(object json, JsonSerializerSettings settings = null)
        {
            if (settings is null)
            {
                settings = new JsonSerializerSettings
                {
                    /*
                        new DefaultContractResolver()

                        new CamelCasePropertyNamesContractResolver()

                        SnakeCaseNamingStrategy: 
                            new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }

                        KebabCaseNamingStrategy:
                            new DefaultContractResolver { NamingStrategy = new KebabCaseNamingStrategy() }

                        new IgnoreNullContractResolver()
                        
                     */
                    ContractResolver = new DefaultContractResolver()
                };
            }
            return JsonConvert.SerializeObject(json, settings);
        }

        public static Task DelayTask(TimeSpan delay)
        {
            return Task.Delay(delay);
        }
    }
}
