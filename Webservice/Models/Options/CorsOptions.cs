using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Options
{
    public class CorsOptions
    {
        public const string OptionName = "CORS";
        public string[] AllowedOrigins { get; set; } = [];
        public string[] AllowedHeaders  { get; set; } = [];
        public string[] AllowedMethods   { get; set; } = [];
    }
}
