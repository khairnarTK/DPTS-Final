using System.Web.Mvc;

namespace DPTS.Web.Models
{
    public class SearchModel
    {
        //public SearchModel()
        //{
        //    AvailableSpeciality = new List<SelectListItem>();
        //}
       // public IList<SelectListItem> AvailableSpeciality { get; set; }

        public string Specialitie { get; set; }

        public string Warning { get; set; }

        public bool NoResults { get; set; }

        public string geo_location { get; set; }

       // public string keyword { get; set; }

       // public string directory_type { get; set; }

        //public int geo_distance { get; set; }

        public decimal minfee { get; set; }

        public decimal maxfee { get; set; }

        //public string lat { get; set; }
        //public string lng { get; set; }

        [AllowHtml]
        public string q { get; set; }

    }
}