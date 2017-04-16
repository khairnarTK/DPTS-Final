using System;
using System.Configuration;

namespace DPTS.EmailSmsNotifications.Services
{
    public class NotificationServiceConfig
    {
        /// <summary>
        /// Get the SMS gateway url
        /// </summary>
        public string SmsUrl => GetAppSetting(typeof(string), "SmsUrl").ToString();

        /// <summary>
        /// Get the SMS Auth key
        /// </summary>
        public string SmsAuthKey => GetAppSetting(typeof(string), "SmsAuthKey").ToString();

        /// <summary>
        /// Get the SMS Auth key
        /// </summary>
        public string SendGridApiKey => GetAppSetting(typeof(string), "SendGridApiKey").ToString();

        private static object GetAppSetting(Type expectedType, string key)
        {
            string value = ConfigurationManager.AppSettings[key]; //.Get(key);

            if (value == null)
            {
                throw new Exception(
                    $"The config file does not have the key '{key}' defined in the AppSetting section.");
            }

            if (expectedType == typeof(int))
            {
                return int.Parse(value);
            }

            if (expectedType == typeof(string))
            {
                return value;
            }
            throw new Exception("Type not supported.");
        }
    }
}