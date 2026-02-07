using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs.FileSystem
{
    public record ImageDTO
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
