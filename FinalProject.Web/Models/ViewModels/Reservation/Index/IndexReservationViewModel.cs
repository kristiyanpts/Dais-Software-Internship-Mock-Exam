using FinalProject.Web.Models.ViewModels.Workspace;

namespace FinalProject.Web.Models.ViewModels.Reservation.Index
{
    public class IndexReservationViewModel
    {
        public List<ReservationViewModel> Reservations { get; set; }
        public List<WorkspaceViewModel> FavoriteWorkspaces { get; set; }
    }
}