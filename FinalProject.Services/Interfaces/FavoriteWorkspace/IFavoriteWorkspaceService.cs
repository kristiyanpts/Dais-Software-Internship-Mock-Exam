using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.AddFavoriteWorkspace;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.DeleteFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.AddFavoriteWorkspace;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.DeleteFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.GetFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;

namespace FinalProject.Services.Interfaces.FavoriteWorkspace
{
    public interface IFavoriteWorkspaceService
    {
        Task<GetFavoriteWorkspaceByIdResponse> GetFavoriteWorkspaceByIdAsync(GetFavoriteWorkspaceByIdRequest request);
        Task<GetFavoriteWorkspacesForAuthUserResponse> GetFavoriteWorkspacesForAuthUserAsync(GetFavoriteWorkspacesForAuthUserRequest request);
        Task<AddFavoriteWorkspaceResponse> AddFavoriteWorkspaceAsync(AddFavoriteWorkspaceRequest request);
        Task<DeleteFavoriteWorkspaceByIdResponse> DeleteFavoriteWorkspaceByIdAsync(DeleteFavoriteWorkspaceByIdRequest request);
    }
}