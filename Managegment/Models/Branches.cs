using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Branches
    {
        public Branches()
        {
            Users = new HashSet<Users>();
        }

        public long BranchId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }
        public short BranchLevel { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
