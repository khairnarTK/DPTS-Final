using DPTS.EmailSmsNotifications.IServices;
using DPTS.EmailSmsNotifications.ServiceModels;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace DPTS.EmailSmsNotifications.Services
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private NotificationServiceConfig _config;

        public SmsNotificationService()
        {
            _config = new NotificationServiceConfig();
        }

        public string SendSms(SmsNotificationModel sms)
        {
            //Your authentication key
            string authKey = _config.SmsAuthKey;
            //Multiple mobiles numbers separated by comma
            string mobileNumber = sms.numbers;
            //Your route to send, promotional=1/transactional=4
            int route = sms.route;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = sms.senderId;
            //Your message to send, Add URL encoding here.
            //string message = HttpUtility.UrlEncode(sms.message);
            string message = System.Uri.EscapeDataString(sms.message);

            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&route={0}", route);

            try
            {
                //Call Send SMS API
                string sendSMSUri = _config.SmsUrl;
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
                return responseString;
            }
            catch (SystemException ex)
            {
                return ex.Message.ToString();
            }
        }

        public string GenerateOTP()
        {
            int length = 6;
            string numbers = "0123456789";
            Random objrandom = new Random();
            string strrandom = string.Empty;
            int noofnumbers = length;
            for (int i = 0; i < noofnumbers; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                strrandom += temp;
            }
            return strrandom;
        }
    }
}