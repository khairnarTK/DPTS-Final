namespace DPTS.Domain.Entities
{
    public partial class Qualifiaction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
