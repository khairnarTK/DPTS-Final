using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    /// <summary>
    /// Represents a doctor picture mapping
    /// </summary>
    public partial class PictureMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        /// [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        [Required]
        public int PictureId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the Doctor
        /// </summary>
        public virtual Doctor Doctor { get; set; }
    }
}
