
using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.FavoriteWorkspace;
using FinalProject.Repository.Interfaces.Reservation;
using FinalProject.Repository.Interfaces.Workspace;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.AddFavoriteWorkspace;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.DeleteFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.AddFavoriteWorkspace;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.DeleteFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.GetFavoriteWorkspaceById;
using FinalProject.Services.DTOs.Responses.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;
using FinalProject.Services.DTOs.Responses.Workspace;
using FinalProject.Services.Interfaces.FavoriteWorkspace;

namespace FinalProject.Services.Implementations.FavoriteWorkspace
{
    public class FavoriteWorkspaceService : IFavoriteWorkspaceService
    {
        private readonly IFavoriteWorkspaceRepository _favoriteWorkspaceRepository;
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IReservationRepository _reservationRepository;

        public FavoriteWorkspaceService(IFavoriteWorkspaceRepository favoriteWorkspaceRepository, IWorkspaceRepository workspaceRepository, IReservationRepository reservationRepository)
        {
            _favoriteWorkspaceRepository = favoriteWorkspaceRepository;
            _workspaceRepository = workspaceRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<GetFavoriteWorkspaceByIdResponse> GetFavoriteWorkspaceByIdAsync(GetFavoriteWorkspaceByIdRequest request)
        {
            if (request == null)
            {
                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can fetch favorite workspaces!"
                };
            }

            if (request.Data == null)
            {
                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.Data.Id <= 0)
            {
                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Invalid favorite workspace id!"
                };
            }

            try
            {
                var favoriteWorkspace = await _favoriteWorkspaceRepository.Retrieve(request.Data.Id);

                var mappedFavoriteWorkspace = await MapFavoriteWorkspaceToFavoriteWorkspaceResponseDto(favoriteWorkspace);

                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = true,
                    Message = "Favorite workspace fetched successfully!",
                    Data = mappedFavoriteWorkspace
                };
            }
            catch (Exception ex)
            {
                return new GetFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GetFavoriteWorkspacesForAuthUserResponse> GetFavoriteWorkspacesForAuthUserAsync(GetFavoriteWorkspacesForAuthUserRequest request)
        {
            if (request == null)
            {
                return new GetFavoriteWorkspacesForAuthUserResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new GetFavoriteWorkspacesForAuthUserResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can fetch favorite workspaces!"
                };
            }

            try
            {
                var queryParameters = new QueryParameters();
                queryParameters.AddWhere("user_id", request.User.Id);

                var favoriteWorkspaces = await _favoriteWorkspaceRepository.RetrieveAll(queryParameters).ToListAsync();

                var mappedFavoriteWorkspaces = await Task.WhenAll(favoriteWorkspaces.Select(async favoriteWorkspace => await MapFavoriteWorkspaceToFavoriteWorkspaceResponseDto(favoriteWorkspace)));

                return new GetFavoriteWorkspacesForAuthUserResponse()
                {
                    Status = true,
                    Message = "Favorite workspaces fetched successfully!",
                    Data = mappedFavoriteWorkspaces
                };
            }
            catch (Exception ex)
            {
                return new GetFavoriteWorkspacesForAuthUserResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<AddFavoriteWorkspaceResponse> AddFavoriteWorkspaceAsync(AddFavoriteWorkspaceRequest request)
        {
            if (request == null)
            {
                return new AddFavoriteWorkspaceResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new AddFavoriteWorkspaceResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can add favorite workspaces!"
                };
            }

            if (request.Data == null)
            {
                return new AddFavoriteWorkspaceResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.Data.WorkspaceId <= 0)
            {
                return new AddFavoriteWorkspaceResponse()
                {
                    Status = false,
                    Message = "Invalid workspace id!"
                };
            }

            try
            {
                var queryParameters = new QueryParameters();
                queryParameters.AddWhere("user_id", request.User.Id);

                var favoriteWorkspaces = await _favoriteWorkspaceRepository.RetrieveAll(queryParameters).ToListAsync();

                if (favoriteWorkspaces.Any(favoriteWorkspace => favoriteWorkspace.WorkspaceId == request.Data.WorkspaceId))
                {
                    return new AddFavoriteWorkspaceResponse()
                    {
                        Status = false,
                        Message = "You have already added this workspace to your favorites!"
                    };
                }

                if (favoriteWorkspaces.Count >= 3)
                {
                    return new AddFavoriteWorkspaceResponse()
                    {
                        Status = false,
                        Message = "You can only have up to 3 favorite workspaces!"
                    };
                }

                var favoriteWorkspace = new Models.FavoriteWorkspace()
                {
                    WorkspaceId = request.Data.WorkspaceId,
                    UserId = request.User.Id,
                    CreatedAt = DateTime.Now
                };

                var id = await _favoriteWorkspaceRepository.Create(favoriteWorkspace);

                favoriteWorkspace.Id = id;

                return new AddFavoriteWorkspaceResponse()
                {
                    Status = true,
                    Message = "Favorite workspace added successfully!",
                    Data = await MapFavoriteWorkspaceToFavoriteWorkspaceResponseDto(favoriteWorkspace)
                };
            }
            catch (Exception ex)
            {
                return new AddFavoriteWorkspaceResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DeleteFavoriteWorkspaceByIdResponse> DeleteFavoriteWorkspaceByIdAsync(DeleteFavoriteWorkspaceByIdRequest request)
        {
            if (request == null)
            {
                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can delete favorite workspaces!"
                };
            }

            if (request.Data == null)
            {
                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.Data.Id <= 0)
            {
                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Invalid favorite workspace id!"
                };
            }

            try
            {
                var favoriteWorkspace = await _favoriteWorkspaceRepository.Retrieve(request.Data.Id);

                if (favoriteWorkspace == null)
                {
                    return new DeleteFavoriteWorkspaceByIdResponse()
                    {
                        Status = false,
                        Message = "Favorite workspace not found!"
                    };
                }

                if (favoriteWorkspace.UserId != request.User.Id)
                {
                    return new DeleteFavoriteWorkspaceByIdResponse()
                    {
                        Status = false,
                        Message = "You are not authorized to delete this favorite workspace!"
                    };
                }

                var deleted = await _favoriteWorkspaceRepository.Delete(request.Data.Id);

                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = true,
                    Message = null,
                    Data = deleted
                };
            }
            catch (Exception ex)
            {
                return new DeleteFavoriteWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        private async Task<WorkspaceResponseDto> MapWorkspaceToWorkspaceResponseDto(Models.Workspace workspace)
        {
            var queryParameters = new QueryParameters();
            queryParameters.AddWhere("workspace_id", workspace.Id);
            queryParameters.AddWhere("reservation_date", DateTime.Now, ">=");
            queryParameters.AddWhere("reservation_date", DateTime.Now.AddDays(14), "<=");

            var reservations = await _reservationRepository.RetrieveAll(queryParameters).ToListAsync();

            return new WorkspaceResponseDto()
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Floor = workspace.Floor,
                Zone = workspace.Zone,
                HasMonitor = workspace.HasMonitor,
                HasDockingStation = workspace.HasDockingStation,
                IsNearWindow = workspace.IsNearWindow,
                IsNearPrinter = workspace.IsNearPrinter,
                ReservedDates = reservations.Select(reservation => reservation.ReservationDate).ToList()
            };
        }

        public async Task<FavoriteWorkspaceResponseDto> MapFavoriteWorkspaceToFavoriteWorkspaceResponseDto(Models.FavoriteWorkspace favoriteWorkspace)
        {
            var workspace = await _workspaceRepository.Retrieve(favoriteWorkspace.WorkspaceId);

            var mappedWorkspace = await MapWorkspaceToWorkspaceResponseDto(workspace);

            return new FavoriteWorkspaceResponseDto()
            {
                Id = favoriteWorkspace.Id,
                WorkspaceId = favoriteWorkspace.WorkspaceId,
                Workspace = mappedWorkspace,
                UserId = favoriteWorkspace.UserId,
                CreatedAt = favoriteWorkspace.CreatedAt
            };
        }
    }
}