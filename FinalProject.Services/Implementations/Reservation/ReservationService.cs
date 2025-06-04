using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.Reservation;
using FinalProject.Repository.Interfaces.User;
using FinalProject.Repository.Interfaces.Workspace;
using FinalProject.Services.DTOs.Requests.Reservation.CancelReservationById;
using FinalProject.Services.DTOs.Requests.Reservation.CreateReservation;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationById;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.DTOs.Requests.Reservation.QuickReserve;
using FinalProject.Services.DTOs.Responses.Reservation;
using FinalProject.Services.DTOs.Responses.Reservation.CancelReservationById;
using FinalProject.Services.DTOs.Responses.Reservation.CreateReservation;
using FinalProject.Services.DTOs.Responses.Reservation.GetReservationById;
using FinalProject.Services.DTOs.Responses.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.DTOs.Responses.Reservation.QuickReserve;
using FinalProject.Services.DTOs.Responses.Workspace;
using FinalProject.Services.Interfaces.Reservation;

namespace FinalProject.Services.Implementations.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkspaceRepository _workspaceRepository;

        public ReservationService(IReservationRepository reservationRepository, IUserRepository userRepository, IWorkspaceRepository workspaceRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _workspaceRepository = workspaceRepository;
        }

        public async Task<GetReservationByIdResponse> GetReservationByIdAsync(GetReservationByIdRequest request)
        {
            if (request == null)
            {
                return new GetReservationByIdResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.Data == null)
            {
                return new GetReservationByIdResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.User == null)
            {
                return new GetReservationByIdResponse()
                {
                    Status = false,
                    Message = "Only authenticated users fetch reservations!"
                };
            }

            if (request.Data.Id <= 0)
            {
                return new GetReservationByIdResponse()
                {
                    Status = false,
                    Message = "Invalid reservation id!"
                };
            }

            try
            {
                var reservation = await _reservationRepository.Retrieve(request.Data.Id);

                if (reservation.UserId != request.User.Id)
                {
                    return new GetReservationByIdResponse()
                    {
                        Status = false,
                        Message = "You are not authorized to access this reservation!"
                    };
                }

                var mappedReservation = await MapReservationToReservationResponseDto(reservation);

                return new GetReservationByIdResponse()
                {
                    Status = true,
                    Message = "Reservation fetched successfully!",
                    Data = mappedReservation
                };
            }
            catch (Exception ex)
            {
                return new GetReservationByIdResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<GetReservationsForAuthUserResponse> GetCurrentReservationsForAuthUserAsync(GetReservationsForAuthUserRequest request)
        {
            if (request == null)
            {
                return new GetReservationsForAuthUserResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.User == null)
            {
                return new GetReservationsForAuthUserResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can fetch reservations!"
                };
            }

            try
            {
                var queryParameters = new QueryParameters();
                queryParameters.AddWhere("user_id", request.User.Id);
                queryParameters.AddWhere("reservation_date", DateTime.Now, ">=");
                queryParameters.AddWhere("reservation_date", DateTime.Now.AddDays(14), "<=");

                var reservations = await _reservationRepository.RetrieveAll(queryParameters).ToListAsync();

                var mappedReservations = await Task.WhenAll(reservations.Select(async reservation => await MapReservationToReservationResponseDto(reservation)));

                return new GetReservationsForAuthUserResponse()
                {
                    Status = true,
                    Message = "Reservations fetched successfully!",
                    Data = mappedReservations
                };
            }
            catch (Exception ex)
            {
                return new GetReservationsForAuthUserResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request)
        {
            if (request == null)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.Data == null)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.User == null)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can create reservations!"
                };
            }

            if (request.Data.WorkspaceId <= 0)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = "Invalid workspace id!"
                };
            }

            if (request.Data.ReservationDate <= DateTime.Now.Date || request.Data.ReservationDate > DateTime.Now.AddDays(14).Date)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = "Invalid reservation date. Reservation date must be between now and 14 days from now."
                };
            }

            try
            {
                var queryParameters = new QueryParameters();
                queryParameters.AddWhere("user_id", request.User.Id);
                queryParameters.AddWhere("reservation_date", request.Data.ReservationDate);

                var alreadyReserved = await _reservationRepository.RetrieveAll(queryParameters).AnyAsync();

                if (alreadyReserved)
                {
                    return new CreateReservationResponse()
                    {
                        Status = false,
                        Message = "You have already reserved a workspace for this date!"
                    };
                }

                queryParameters = new QueryParameters();
                queryParameters.AddWhere("workspace_id", request.Data.WorkspaceId);
                queryParameters.AddWhere("reservation_date", request.Data.ReservationDate);

                var reservations = await _reservationRepository.RetrieveAll(queryParameters).ToListAsync();

                if (reservations.Any())
                {
                    return new CreateReservationResponse()
                    {
                        Status = false,
                        Message = "Workspace is already reserved for this date!"
                    };
                }

                var reservation = new Models.Reservation()
                {
                    WorkspaceId = request.Data.WorkspaceId,
                    UserId = request.User.Id,
                    ReservationDate = request.Data.ReservationDate,
                    IsQuickReservation = false,
                    CreatedAt = DateTime.Now
                };

                var id = await _reservationRepository.Create(reservation);

                reservation.Id = id;

                return new CreateReservationResponse()
                {
                    Status = true,
                    Message = "Reservation created successfully!",
                    Data = await MapReservationToReservationResponseDto(reservation)
                };
            }
            catch (Exception ex)
            {
                return new CreateReservationResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<QuickReserveResponse> QuickReserveAsync(QuickReserveRequest request)
        {
            if (request == null)
            {
                return new QuickReserveResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.Data == null)
            {
                return new QuickReserveResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.User == null)
            {
                return new QuickReserveResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can quick reserve!"
                };
            }

            if (request.Data.WorkspaceId <= 0)
            {
                return new QuickReserveResponse()
                {
                    Status = false,
                    Message = "Invalid workspace id!"
                };
            }

            try
            {
                var tomorrow = DateTime.Now.AddDays(1);

                var workspace = await _workspaceRepository.Retrieve(request.Data.WorkspaceId);

                if (workspace == null)
                {
                    return new QuickReserveResponse()
                    {
                        Status = false,
                        Message = "Workspace not found!"
                    };
                }

                var queryParameters = new QueryParameters();
                queryParameters.AddWhere("user_id", request.User.Id);
                queryParameters.AddWhere("reservation_date", tomorrow.Date);

                var alreadyHasReservation = await _reservationRepository.RetrieveAll(queryParameters).AnyAsync();

                if (alreadyHasReservation)
                {
                    return new QuickReserveResponse()
                    {
                        Status = false,
                        Message = "You have already reserved a workspace for tomorrow!"
                    };
                }

                queryParameters = new QueryParameters();
                queryParameters.AddWhere("workspace_id", request.Data.WorkspaceId);
                queryParameters.AddWhere("reservation_date", tomorrow.Date);

                var alreadyReserved = await _reservationRepository.RetrieveAll(queryParameters).AnyAsync();

                if (alreadyReserved)
                {
                    return new QuickReserveResponse()
                    {
                        Status = false,
                        Message = "Workspace is already reserved for tomorrow!"
                    };
                }

                var reservation = new Models.Reservation()
                {
                    WorkspaceId = request.Data.WorkspaceId,
                    UserId = request.User.Id,
                    ReservationDate = tomorrow.Date,
                    IsQuickReservation = true,
                    CreatedAt = DateTime.Now
                };

                var id = await _reservationRepository.Create(reservation);

                reservation.Id = id;

                var mappedReservation = await MapReservationToReservationResponseDto(reservation);

                return new QuickReserveResponse()
                {
                    Status = true,
                    Message = "Workspace reserved for tomorrow!",
                    Data = mappedReservation
                };
            }
            catch (Exception ex)
            {
                return new QuickReserveResponse()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<CancelReservationByIdResponse> CancelReservationByIdAsync(CancelReservationByIdRequest request)
        {
            if (request == null)
            {
                return new CancelReservationByIdResponse()
                {
                    Status = false,
                    Message = "Request is null!"
                };
            }

            if (request.Data == null)
            {
                return new CancelReservationByIdResponse()
                {
                    Status = false,
                    Message = "Request data is null!"
                };
            }

            if (request.User == null)
            {
                return new CancelReservationByIdResponse()
                {
                    Status = false,
                    Message = "Only authenticated users can cancel reservations!"
                };
            }

            if (request.Data.ReservationId <= 0)
            {
                return new CancelReservationByIdResponse()
                {
                    Status = false,
                    Message = "Invalid reservation id!"
                };
            }

            try
            {
                var reservation = await _reservationRepository.Retrieve(request.Data.ReservationId);

                if (reservation.UserId != request.User.Id)
                {
                    return new CancelReservationByIdResponse()
                    {
                        Status = false,
                        Message = "You are not authorized to cancel this reservation!"
                    };
                }

                var deleted = await _reservationRepository.Delete(request.Data.ReservationId);

                return new CancelReservationByIdResponse()
                {
                    Status = true,
                    Message = "Reservation cancelled successfully!",
                    Data = deleted
                };
            }
            catch (Exception ex)
            {
                return new CancelReservationByIdResponse()
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

        public async Task<ReservationResponseDto> MapReservationToReservationResponseDto(Models.Reservation reservation)
        {
            var user = await _userRepository.Retrieve(reservation.UserId);
            var workspace = await _workspaceRepository.Retrieve(reservation.WorkspaceId);

            var mappedWorkspace = await MapWorkspaceToWorkspaceResponseDto(workspace);

            return new ReservationResponseDto()
            {
                Id = reservation.Id,
                WorkspaceId = reservation.WorkspaceId,
                Workspace = mappedWorkspace,
                UserId = reservation.UserId,
                UserName = user.FullName,
                ReservationDate = reservation.ReservationDate,
                IsQuickReservation = reservation.IsQuickReservation,
                CreatedAt = reservation.CreatedAt
            };
        }
    }
}