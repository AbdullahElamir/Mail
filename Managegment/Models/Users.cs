using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Users
    {
        public long UserId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public short UserType { get; set; }
        public string Email { get; set; }
        public short Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public short LoginTryAttempts { get; set; }
        public DateTime? LoginTryAttemptDate { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public byte[] Photo { get; set; }
        public long? NationalId { get; set; }
        public int? PersonId { get; set; }
        public short Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
    }
}
