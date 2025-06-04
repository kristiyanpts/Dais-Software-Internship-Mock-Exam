using FinalProject.Services.DTOs.Requests.Workspace.GetAllWorkspaces;
using FinalProject.Services.DTOs.Requests.Workspace.GetWorkspaceById;
using FinalProject.Services.DTOs.Responses.Workspace.GetAllWorkspaces;
using FinalProject.Services.DTOs.Responses.Workspace.GetWorkspaceById;

namespace FinalProject.Services.Interfaces.Workspace
{
    public interface IWorkspaceService
    {
        Task<GetAllWorkspacesResponse> GetAllWorkspacesAsync(GetAllWorkspacesRequest request);
        Task<GetWorkspaceByIdResponse> GetWorkspaceByIdAsync(GetWorkspaceByIdRequest request);
    }
}