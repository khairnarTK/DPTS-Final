using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using DPTS.Web.Models;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace DPTS.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
            var port = ConfigurationManager.AppSettings["Port"].ToString();
            var userName = ConfigurationManager.AppSettings["Username"].ToString();
            var password = ConfigurationManager.AppSettings["Password"].ToString();
            var appName = ConfigurationManager.AppSettings["AppTitle"].ToString();
            var enableSSL = ConfigurationManager.AppSettings["EnableSSL"].ToString();
            string text = message.Body;
            string html = message.Body;
            var msg = new MailMessage
            {
                From = new MailAddress(userName, appName)
            };
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            var smtpClient = new SmtpClient(smtpServer, Convert.ToInt32(port));
            smtpClient.UseDefaultCredentials = false;
            var credentials = new NetworkCredential(userName, password);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = Convert.ToBoolean(enableSSL);
            smtpClient.Send(msg);
            return Task.FromResult(0);
        }


    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            //var Twilio = new TwilioRestClient("AC6eec9ab16de06b6555d5823d3d7d009f", "26cbda1255a0952c962af6b27fa216af");
            ////System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
            ////System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"]);
            ////System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"]
            //var result = Twilio.SendMessage("+14703759131", message.Destination, message.Body);
            ////Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            //Trace.TraceInformation(result.Status);

            //ASPSMS Begin
            //var soapSms = new ASPSMSX2SoapClient("ASPSMSX2Soap");
            //soapSms.SendSimpleTextSMS(
            //    System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
            //    System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"],
            //    message.Destination,
            //    System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
            //    message.Body);
            //soapSms.Close();
            //ASPSMS End

            return Task.FromResult(0);
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}