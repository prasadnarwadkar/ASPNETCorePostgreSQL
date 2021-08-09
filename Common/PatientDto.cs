using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
    /// <summary>
    /// Dto class for Patient entity. Never use EF Core entities directly
    /// in BLL or DAL.
    /// </summary>
    public class PatientDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 10)]
        public string Name { get; set; }

        [Key]
        public long ID { get; set; }
        public string Address { get; set; }
    }
}
