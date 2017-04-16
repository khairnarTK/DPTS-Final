namespace DPTS.Domain.Entities.Notification
{
    public class EmailCategory : BaseEntityWithDateTime
    {
        public EmailCategory()
        {
        }

        public string Name { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

    }
}
