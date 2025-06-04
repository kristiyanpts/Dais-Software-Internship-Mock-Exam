using FinalProject.Web.Models.ViewModels.Workspace;

namespace FinalProject.Web.Models.ViewModels.Reservation
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public WorkspaceViewModel Workspace { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsQuickReservation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}