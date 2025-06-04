using FinalProject.Web.Models.ViewModels.Workspace;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Web.Models.ViewModels.Reservation.Create
{
    public class CreateReservationViewModel
    {
        public List<WorkspaceViewModel>? Workspaces { get; set; }
        public List<SimpleFavoriteWorkspaceViewModel>? FavoriteWorkspaces { get; set; }

        [Required(ErrorMessage = "Workspace is required")]
        public int WorkspaceId { get; set; }

        [Required(ErrorMessage = "Reservation date is required")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }
    }
}