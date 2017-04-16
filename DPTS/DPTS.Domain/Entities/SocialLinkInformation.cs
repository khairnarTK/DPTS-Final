using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
   public partial class SocialLinkInformation
    {
        public int  Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DoctorId { get; set; }

        /// <summary>
        /// social type - facebook,instagram,twitter,..etc
        /// </summary>
        [Required]
        public string SocialType { get; set; }

        /// <summary>
        /// social link
        /// </summary>
        public string SocialLink { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
