using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class AdTypes
    {
        public AdTypes()
        {
            Conversations = new HashSet<Conversations>();
        }

        public long AdTypeId { get; set; }
        public string AdTypeName { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }

        public Users CreatedByNavigation { get; set; }
        public Users ModifiedByNavigation { get; set; }
        public ICollection<Conversations> Conversations { get; set; }
    }
}
