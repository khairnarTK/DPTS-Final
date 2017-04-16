namespace DPTS.Domain.Entities
{
    public partial class SubSpeciality : BaseEntityWithDateTime
    {
        public int SpecialityId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public virtual Speciality Speciality { get; set; }
    }
}
