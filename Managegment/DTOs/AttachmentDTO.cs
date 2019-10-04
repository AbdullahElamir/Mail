using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class AttachmentDTO
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] ContentFile { get; set; }
        public double? FileSize { get; set; }
        public string Hash { get; set; }
    }
}
