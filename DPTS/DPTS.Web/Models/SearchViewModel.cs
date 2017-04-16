using System.Collections.Generic;

namespace DPTS.Web.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            doctorsModel = new List<DoctorViewModel>();
            SearchModel = new SearchModel();
        }

        public virtual IList<DoctorViewModel> doctorsModel { get; set; }

        public virtual SearchModel SearchModel { get; set; }
    }
}