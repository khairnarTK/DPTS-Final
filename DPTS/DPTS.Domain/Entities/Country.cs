using System.Collections.Generic;

namespace DPTS.Domain.Entities
{
    public partial class Country :BaseEntityWithDateTime
    {
        public Country()
        {
            Addresses = new HashSet<Address>();
            StateProvinces = new HashSet<StateProvince>();
        }

        public string Name { get; set; }

        public string TwoLetterIsoCode { get; set; }

        public string ThreeLetterIsoCode { get; set; }

        public int NumericIsoCode { get; set; }

        public bool SubjectToVat { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<StateProvince> StateProvinces { get; set; }
    }
}
