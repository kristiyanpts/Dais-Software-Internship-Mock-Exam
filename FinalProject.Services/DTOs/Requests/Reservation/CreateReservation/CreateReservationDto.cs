namespace FinalProject.Services.DTOs.Requests.Reservation.CreateReservation
{
    public class CreateReservationDto
    {
        public int WorkspaceId { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}