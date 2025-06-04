using FinalProject.Services.DTOs.Responses.Workspace;

namespace FinalProject.Services.DTOs.Responses.FavoriteWorkspace
{
    public class FavoriteWorkspaceResponseDto
    {
        public int Id { get; set; }
        public int WorkspaceId { get; set; }
        public WorkspaceResponseDto Workspace { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}