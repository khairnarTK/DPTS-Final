using System;

namespace DPTS.Domain.Entities.Notification
{
    public class SentEmailHistory : BaseEntityWithDateTime
    {
        public SentEmailHistory()
        {
        }

        public string SenderId { get; set; } 
        public string ReceiverId { get; set; }
        public string SenderType { get; set; }
        public string ReceiverEmail { get; set; }
        public string Email { get; set; } 
        public virtual Doctor Doctor { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
