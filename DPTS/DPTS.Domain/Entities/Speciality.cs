using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class Speciality : BaseEntityWithDateTime
    {
        public Speciality()
        {
            SpecialityMapping = new HashSet<SpecialityMapping>();
            SubSpecialities = new HashSet<SubSpeciality>();
        }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<SpecialityMapping> SpecialityMapping { get; set; }

        public virtual ICollection<SubSpeciality> SubSpecialities { get; set; }
    }
}
