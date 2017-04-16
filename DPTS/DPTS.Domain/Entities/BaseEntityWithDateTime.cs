using System;

namespace DPTS.Domain.Entities
{
    public class BaseEntityWithDateTime : BaseEntity
    {
        /// <summary>
        /// Get Or Set Date Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Get Or Set Date Updated
        /// </summary>
        public DateTime DateUpdated { get; set; }
    }
}
