using System;
using System.ComponentModel.DataAnnotations;

namespace DPTS.Data.IdentityEntities
{
    public class User 
    {
        [Required, MaxLength(256)]
        public string FirstName { get; set; }

        [Required, MaxLength(256)]
        public string LastName { get; set; }

        public string LastIpAddress { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? LastLoginDateUtc { get; set; }
    }
}
