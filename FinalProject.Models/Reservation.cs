using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Workspace ID is required")]
        public int WorkspaceId { get; set; }

        [Required(ErrorMessage = "Reservation date is required")]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Is quick reservation is required")]
        public bool IsQuickReservation { get; set; }

        [Required(ErrorMessage = "Created at is required")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}