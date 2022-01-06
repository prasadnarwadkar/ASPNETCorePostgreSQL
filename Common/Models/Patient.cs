using System;
using System.Collections.Generic;

namespace Common.Models
{
    public partial class Patient
    {
        public string[] Name { get; set; }
        public long Id { get; set; }
        public string[] Address { get; set; }
        public Guid? Uhid { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
