using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public partial class Patient
    {
        public string[] Name { get; set; }
        public long Id { get; set; }
        public string[] Address { get; set; }
    }
}
