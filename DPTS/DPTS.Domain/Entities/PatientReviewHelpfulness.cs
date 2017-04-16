namespace DPTS.Domain.Entities
{
    /// <summary>
    /// Represents a patient review helpfulness
    /// </summary>
    public partial class PatientReviewHelpfulness : BaseEntity
    {
        /// <summary>
        /// Gets or sets the doctor review identifier
        /// </summary>
        public int DoctorReviewId { get; set; }

        /// <summary>
        /// A value indicating whether a review a helpful
        /// </summary>
        public bool WasHelpful { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier
        /// </summary>
        public string PatientId { get; set; }

        /// <summary>
        /// Gets the doctor
        /// </summary>
        public virtual DoctorReview DoctorReview { get; set; }
    }
}
