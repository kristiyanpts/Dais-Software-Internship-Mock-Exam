using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class FavoriteWorkspace
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Workspace ID is required")]
        public int WorkspaceId { get; set; }

        [Required(ErrorMessage = "Created at is required")]
        public DateTime CreatedAt { get; set; }
    }
}