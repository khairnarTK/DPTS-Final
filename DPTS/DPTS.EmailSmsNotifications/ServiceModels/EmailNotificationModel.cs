namespace DPTS.EmailSmsNotifications.ServiceModels
{
    public class EmailNotificationModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string subject { get; set; }
        public string content { get; set; }
    }
}