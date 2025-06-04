using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.Reservation;
using FinalProject.Repository.Interfaces.Workspace;
using FinalProject.Services.DTOs.Requests.Workspace.GetAllWorkspaces;
using FinalProject.Services.DTOs.Requests.Workspace.GetWorkspaceById;
using FinalProject.Services.DTOs.Responses.Workspace;
using FinalProject.Services.DTOs.Responses.Workspace.GetAllWorkspaces;
using FinalProject.Services.DTOs.Responses.Workspace.GetWorkspaceById;
using FinalProject.Services.Interfaces.Workspace;

namespace FinalProject.Services.Implementations.Workspace
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IReservationRepository _reservationRepository;

        public WorkspaceService(IWorkspaceRepository workspaceRepository, IReservationRepository reservationRepository)
        {
            _workspaceRepository = workspaceRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<GetAllWorkspacesResponse> GetAllWorkspacesAsync(GetAllWorkspacesRequest request)
        {
            if (request == null)
            {
                return new GetAllWorkspacesResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new GetAllWorkspacesResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can fetch workspaces!"
                };
            }

            try
            {
                var workspaces = await _workspaceRepository.RetrieveAll().ToListAsync();

                var mappedWorkspaces = await Task.WhenAll(workspaces.Select(async workspace => await MapWorkspaceToWorkspaceResponseDto(workspace)));

                return new GetAllWorkspacesResponse()
                {
                    Status = true,
                    Message = "Workspaces fetched successfully!",
                    Data = mappedWorkspaces
                };
            }
            catch (Exception ex)
            {
                return new GetAllWorkspacesResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GetWorkspaceByIdResponse> GetWorkspaceByIdAsync(GetWorkspaceByIdRequest request)
        {
            if (request == null)
            {
                return new GetWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.Data == null)
            {
                return new GetWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.User == null)
            {
                return new GetWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can fetch workspaces!"
                };
            }

            if (request.Data.Id <= 0)
            {
                return new GetWorkspaceByIdResponse()
                {
                    Status = false,
                    Message = "Invalid workspace id!"
                };
            }

            try
            {
                var workspace = await _workspaceRepository.Retrieve(request.Data.Id);

                var mappedWorkspace = await MapWorkspaceToWorkspaceResponseDto(workspace);

                return new GetWorkspaceByIdResponse()
                {
                    Status = true,
                    Message = "Workspace fetched successfully!",
                    Data = mappedWorkspace
                };
            }
            catch (Exception ex)
            {
                return new GetWorkspaceByIdResponse()
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
    }
}