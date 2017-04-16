namespace DPTS.Domain.Entities
{
    public partial class DoctorReviewHelpfulness : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product review identifier
        /// </summary>
        public int DoctorReviewId { get; set; }

        /// <summary>
        /// A value indicating whether a review a helpful
        /// </summary>
        public bool WasHelpful { get; set; }

        /// <summary>
        /// Gets or sets the visitor identifier
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual DoctorReview DoctorReview { get; set; }
    }
}
