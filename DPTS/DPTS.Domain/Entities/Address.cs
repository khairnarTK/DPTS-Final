using System.Collections.Generic;

namespace DPTS.Domain.Entities
{
    public partial class Address :BaseEntityWithDateTime
    {
        public Address()
        {
            AddressMappings = new HashSet<AddressMapping>();
        }

        public string Hospital { get; set; }

        public int? CountryId { get; set; }

        public int? StateProvinceId { get; set; }

        public string City { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string ZipPostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string FaxNumber { get; set; }

        public int? Doctor_Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public virtual ICollection<AddressMapping> AddressMappings { get; set; }

        public virtual Country Country { get; set; }

        public virtual StateProvince StateProvince { get; set; }
    }
}
