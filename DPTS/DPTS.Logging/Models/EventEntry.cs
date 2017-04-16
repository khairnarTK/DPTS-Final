using System;
using System.Net;

namespace DPTS.Logging.Models
{
    public class EventEntry
    {
        public string Id => Guid.NewGuid().ToString(); //model init used using c#6.0 feature
        public string SessionId => Guid.NewGuid().ToString();
        public string MachineName => Dns.GetHostName();
        public string IpAddress => "127.0.0.1";
        public string Status { get; set; }
        public decimal? ResponseTime { get; set; }
        public string Title { get; set; }
        public string CallType { get; set; }
        public object Request { get; set; }
        public object Response { get; set; }
        public DateTime DateTime => DateTime.Now;
    }
}