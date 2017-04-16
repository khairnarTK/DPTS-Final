using System;

namespace DPTS.Domain.Entities.Notification
{
    public class SentSmsHistory : BaseEntityWithDateTime
    {
        public SentSmsHistory()
        {
        }

        public string SenderId { get; set; } 
        public string ReceiverId { get; set; }
        public string SenderType { get; set; }
        public string ReceiverPhone { get; set; }
        public string Text { get; set; } 
        public virtual Doctor Doctor { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
