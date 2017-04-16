using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public partial class HonorsAwards
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DoctorId { get; set; }

        /// <summary>
        /// award title
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// award Description
        /// </summary>
        public string Description { get; set; }

        public DateTime AwardDate { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
