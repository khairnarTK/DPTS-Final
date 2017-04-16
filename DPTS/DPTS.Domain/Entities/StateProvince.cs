using System.Collections.Generic;

namespace DPTS.Domain.Entities
{
    public partial class StateProvince :BaseEntityWithDateTime
    {
        public StateProvince()
        {
            Addresses = new HashSet<Address>();
        }

        public int CountryId { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual Country Country { get; set; }
    }
}
