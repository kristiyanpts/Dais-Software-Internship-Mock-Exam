namespace FinalProject.Services.DTOs.Responses.Workspace
{
    public class WorkspaceResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public string Zone { get; set; }
        public bool HasMonitor { get; set; }
        public bool HasDockingStation { get; set; }
        public bool IsNearWindow { get; set; }
        public bool IsNearPrinter { get; set; }

        public List<DateTime> ReservedDates { get; set; }
    }
}