using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Domain.Entities
{
    public class Picture
    {
        public Picture()
        {
            PictureMapping = new HashSet<PictureMapping>();
        }
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the picture binary
        /// </summary>
        public byte[] PictureBinary { get; set; }

        /// <summary>
        /// Gets or sets the picture mime type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string AltAttribute { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. product name)
        /// </summary>
        public string TitleAttribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the picture is new
        /// </summary>
        public bool IsNew { get; set; }

        public string SeoFilename { get; set; }

        public virtual ICollection<PictureMapping> PictureMapping { get; set; }
    }
}
