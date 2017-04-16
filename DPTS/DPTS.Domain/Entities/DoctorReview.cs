using System;
using System.Collections.Generic;

namespace DPTS.Domain.Entities
{
    public partial class DoctorReview : BaseEntity
    {
        private ICollection<DoctorReviewHelpfulness> _doctorReviewHelpfulnessEntries;

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public string DoctorId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is approved
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the review text
        /// </summary>
        public string ReviewText { get; set; }

        /// <summary>
        /// Gets or sets the reply text
        /// </summary>
        public string ReplyText { get; set; }

        /// <summary>
        /// Review rating
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Review helpful votes total
        /// </summary>
        public int HelpfulYesTotal { get; set; }

        /// <summary>
        /// Review not helpful votes total
        /// </summary>
        public int HelpfulNoTotal { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the visitor
        /// </summary>
        public virtual AspNetUser AspNetUser { get; set; }

        /// <summary>
        /// Gets the doctor
        /// </summary>
        public virtual Doctor Doctor { get; set; }


        /// <summary>
        /// Gets the entries of product review helpfulness
        /// </summary>
        public virtual ICollection<DoctorReviewHelpfulness> DoctorReviewHelpfulnessEntries
        {
            get { return _doctorReviewHelpfulnessEntries ?? (_doctorReviewHelpfulnessEntries = new List<DoctorReviewHelpfulness>()); }
            protected set { _doctorReviewHelpfulnessEntries = value; }
        }
    }
}
