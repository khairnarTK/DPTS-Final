namespace DPTS.Web.Models
{
    public class DoctorSpecialityViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool IsChecked { get; set; }
    }
}