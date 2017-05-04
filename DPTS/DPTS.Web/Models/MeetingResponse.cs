using System;

namespace DPTS.Web.Models
{
    public class MeetingResponse
    {
        public string uuid { get; set; }
        public int id { get; set; }
        public string start_url { get; set; }
        public string join_url { get; set; }

        public DateTime created_at { get; set; }
        public string host_id { get; set; }
        public string topic { get; set; }
        public int type { get; set; }
        public bool option_jbh { get; set; }
        public bool option_host_video { get; set; }
        public bool option_participants_video { get; set; }
        public string option_audio { get; set; }
        public bool option_enforce_login { get; set; }
    }
}