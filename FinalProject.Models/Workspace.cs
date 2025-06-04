using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Workspace
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 255 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Floor is required")]
        [Range(1, 10, ErrorMessage = "Floor must be between 1 and 10!")]
        public int Floor { get; set; }

        [Required(ErrorMessage = "Zone is required")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Zone must be between 3 and 255 characters!")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "HasMonitor is required")]
        public bool HasMonitor { get; set; }

        [Required(ErrorMessage = "HasDockingStation is required")]
        public bool HasDockingStation { get; set; }

        [Required(ErrorMessage = "IsNearWindow is required")]
        public bool IsNearWindow { get; set; }

        [Required(ErrorMessage = "IsNearPrinter is required")]
        public bool IsNearPrinter { get; set; }
    }
}