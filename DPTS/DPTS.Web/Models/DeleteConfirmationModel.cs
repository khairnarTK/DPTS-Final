using DPTS.Domain.Entities;

namespace DPTS.Web.Models
{
    public class DeleteConfirmationModel : BaseEntity
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
    }
}