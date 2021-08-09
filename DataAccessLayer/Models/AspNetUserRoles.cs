using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class AspNetUserRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
