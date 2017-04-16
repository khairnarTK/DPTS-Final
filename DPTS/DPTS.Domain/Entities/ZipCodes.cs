namespace DPTS.Domain.Entities
{
    public partial class ZipCodes
    {
        public int Id { get; set; }
        /// <summary>
        /// Get or set the zipcode
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Get or set latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Get or set longitude
        /// </summary>
        public double Longitude { get; set; }
    }
}
