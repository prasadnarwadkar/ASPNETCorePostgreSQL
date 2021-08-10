using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common
{
    
    public  class UnnaturalDeathsDto {

        [Required]
        public DateTimeOffset? TimeOfPostmortemExamination { get; set; }
        public string Sex { get; set; }

        [StringLength(200, MinimumLength = 20)]
        public string SceneOfDeath { get; set; }
        public string PoliceStation { get; set; }

        [Required]
        
        public int? PoliceCaseNo { get; set; }
        [Required]
        public string PlaceOfExamination { get; set; }
        [Required]
        public string Nationality { get; set; }
        public string InformantRelationToDeceased { get; set; }
        public string InformantName { get; set; }
        public string ImformantCidNo { get; set; }
        public string History { get; set; }
        public string GeneralExternalInformation { get; set; }
        [Required]
        public string Dzongkhag { get; set; }
        public string DeceasedName { get; set; }
        public DateTime? DateOfPostmortemExamination { get; set; }
        [Required]
        public string CidOrPassport { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public bool? Isactive { get; set; }
        public string Transactedby { get; set; }
        public DateTime Transacteddate { get; set; }
        [Required]
        public string Remark { get; set; }
        public DateTime? Lastchanged { get; set; }
        public int Version { get; set; }

        [Key]
        public Guid Id { get; set; }
    }
}
