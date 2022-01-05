using System;
using System.Collections.Generic;

namespace datasyncservice.Models
{
    public partial class Unnaturaldeaths
    {
        public DateTimeOffset? TimeOfPostmortemExamination { get; set; }
        public string Sex { get; set; }
        public string SceneOfDeath { get; set; }
        public string PoliceStation { get; set; }
        public int? PoliceCaseNo { get; set; }
        public string PlaceOfExamination { get; set; }
        public string Nationality { get; set; }
        public string InformantRelationToDeceased { get; set; }
        public string InformantName { get; set; }
        public string ImformantCidNo { get; set; }
        public string History { get; set; }
        public string GeneralExternalInformation { get; set; }
        public string Dzongkhag { get; set; }
        public string DeceasedName { get; set; }
        public DateTime? DateOfPostmortemExamination { get; set; }
        public string CidOrPassport { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public bool? Isactive { get; set; }
        public string Transactedby { get; set; }
        public DateTime Transacteddate { get; set; }
        public string Remark { get; set; }
        public DateTime? Lastchanged { get; set; }
        public int Version { get; set; }
        public Guid Id { get; set; }
    }
}
