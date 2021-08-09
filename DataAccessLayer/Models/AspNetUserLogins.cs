using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class AspNetUserLogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public Guid UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
