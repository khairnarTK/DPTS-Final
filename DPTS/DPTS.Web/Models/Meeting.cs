namespace DPTS.Web.Models
{
    public class Meeting
    {
        public string Topic { get; set; }
        public string Password { get; set; }
        public string MeetingType { get; set; }
        public bool IsVideoHostOn { get; set; }
        public bool IsVideoParticipantsOn{ get; set; }
        public string AudioOption { get; set; }
    }
}

