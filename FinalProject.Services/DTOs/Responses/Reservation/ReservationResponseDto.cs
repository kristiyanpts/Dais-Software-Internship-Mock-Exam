using FinalProject.Services.DTOs.Responses.Workspace;

namespace FinalProject.Services.DTOs.Responses.Reservation
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public WorkspaceResponseDto Workspace { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsQuickReservation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}