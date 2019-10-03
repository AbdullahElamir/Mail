
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.LibyanObjects
{
    public partial class UsersObj
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public short? UserType { get; set; }
        public string Email { get; set; }
        public short Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public string Photo { get; set; }

        public DateTime CreatedOn { get; set; }
        public short Status { get; set; }
        public int? BranchId { get; set; }
        public long CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Phone { get; set; }


    }

}
