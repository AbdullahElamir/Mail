using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class MessageDTO
    {
        public long MessageID { get; set; }
        public string UserName { get; set; }
        public string DateTime { get; set; }
        public string Subject { get; set; }

        public string ImageUser { get; set; }
    }
}
