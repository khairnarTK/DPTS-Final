using DPTS.EmailSmsNotifications.ServiceModels;

namespace DPTS.EmailSmsNotifications.IServices
{
    public interface ISmsNotificationService
    {
        string SendSms(SmsNotificationModel sms);

        string GenerateOTP();
    }
}