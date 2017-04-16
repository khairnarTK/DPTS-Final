using DPTS.EmailSmsNotifications.ServiceModels;
using System.Threading.Tasks;

namespace DPTS.EmailSmsNotifications.IServices
{
    public interface IEmailNotificationService
    {
        Task SendEmail(EmailNotificationModel model);
    }
}