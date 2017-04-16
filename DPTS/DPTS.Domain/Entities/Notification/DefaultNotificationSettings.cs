namespace DPTS.Domain.Entities.Notification
{
    public class DoctorNotificationSettings : BaseEntityWithDateTime
    {
        public DoctorNotificationSettings()
        {
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Message { get; set; }

        public string DoctorId { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public virtual EmailCategory EmailCategory { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
