using FinalProject.Services.DTOs.Requests.FavoriteWorkspace.GetFavoriteWorkspacesForAuthUser;
using FinalProject.Services.DTOs.Requests.Reservation.CancelReservationById;
using FinalProject.Services.DTOs.Requests.Reservation.CreateReservation;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.DTOs.Requests.Reservation.QuickReserve;
using FinalProject.Services.DTOs.Requests.Workspace.GetAllWorkspaces;
using FinalProject.Services.Interfaces.FavoriteWorkspace;
using FinalProject.Services.Interfaces.Reservation;
using FinalProject.Services.Interfaces.Workspace;
using FinalProject.Web.Controllers.Base;
using FinalProject.Web.Models.ViewModels.Error;
using FinalProject.Web.Models.ViewModels.Reservation;
using FinalProject.Web.Models.ViewModels.Reservation.Create;
using FinalProject.Web.Models.ViewModels.Reservation.Index;
using FinalProject.Web.Models.ViewModels.Workspace;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Web.Controllers.Reservation
{
    [Route("[controller]")]
    public class ReservationController : BaseController
    {
        private readonly IReservationService _reservationService;
        private readonly IFavoriteWorkspaceService _favoriteWorkspaceService;
        private readonly IWorkspaceService _workspaceService;

        public ReservationController(IReservationService reservationService, IFavoriteWorkspaceService favoriteWorkspaceService, IWorkspaceService workspaceService)
        {
            _reservationService = reservationService;
            _favoriteWorkspaceService = favoriteWorkspaceService;
            _workspaceService = workspaceService;
        }

        public async Task<IActionResult> Index()
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var reservationsRequest = new GetReservationsForAuthUserRequest()
            {
                User = GetUserSessionData()
            };

            var reservationsResponse = await _reservationService.GetCurrentReservationsForAuthUserAsync(reservationsRequest);

            if (reservationsResponse.Status == false)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = reservationsResponse.Message
                });
            }

            var favoriteWorkspacesRequest = new GetFavoriteWorkspacesForAuthUserRequest()
            {
                User = GetUserSessionData()
            };

            var favoriteWorkspacesResponse = await _favoriteWorkspaceService.GetFavoriteWorkspacesForAuthUserAsync(favoriteWorkspacesRequest);

            if (favoriteWorkspacesResponse.Status == false)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = favoriteWorkspacesResponse.Message
                });
            }

            var reservations = reservationsResponse.Data.ToList().Select(reservation => new ReservationViewModel()
            {
                Id = reservation.Id,
                WorkspaceId = reservation.WorkspaceId,
                Workspace = new WorkspaceViewModel()
                {
                    Id = reservation.Workspace.Id,
                    Name = reservation.Workspace.Name,
                    Floor = reservation.Workspace.Floor,
                    Zone = reservation.Workspace.Zone,
                    HasMonitor = reservation.Workspace.HasMonitor,
                    HasDockingStation = reservation.Workspace.HasDockingStation,
                    IsNearWindow = reservation.Workspace.IsNearWindow,
                    IsNearPrinter = reservation.Workspace.IsNearPrinter,
                    ReservedDates = reservation.Workspace.ReservedDates,
                },
                UserId = reservation.UserId,
                UserName = reservation.UserName,
                ReservationDate = reservation.ReservationDate,
                IsQuickReservation = reservation.IsQuickReservation,
                CreatedAt = reservation.CreatedAt
            }).ToList();

            var favoriteWorkspaces = favoriteWorkspacesResponse.Data.ToList().Select(favoriteWorkspace => new WorkspaceViewModel()
            {
                Id = favoriteWorkspace.Workspace.Id,
                Name = favoriteWorkspace.Workspace.Name,
                Floor = favoriteWorkspace.Workspace.Floor,
                Zone = favoriteWorkspace.Workspace.Zone,
                HasMonitor = favoriteWorkspace.Workspace.HasMonitor,
                HasDockingStation = favoriteWorkspace.Workspace.HasDockingStation,
                IsNearWindow = favoriteWorkspace.Workspace.IsNearWindow,
                IsNearPrinter = favoriteWorkspace.Workspace.IsNearPrinter,
                ReservedDates = favoriteWorkspace.Workspace.ReservedDates
            }).ToList();

            var viewModel = new IndexReservationViewModel()
            {
                Reservations = reservations,
                FavoriteWorkspaces = favoriteWorkspaces
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("create/{workspaceId?}")]
        public async Task<IActionResult> Create(int? workspaceId)
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var workspacesRequest = new GetAllWorkspacesRequest()
            {
                User = GetUserSessionData()
            };

            var workspacesResponse = await _workspaceService.GetAllWorkspacesAsync(workspacesRequest);

            if (workspacesResponse.Status == false)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = workspacesResponse.Message
                });
            }

            var favoriteWorkspacesRequest = new GetFavoriteWorkspacesForAuthUserRequest()
            {
                User = GetUserSessionData()
            };

            var favoriteWorkspacesResponse = await _favoriteWorkspaceService.GetFavoriteWorkspacesForAuthUserAsync(favoriteWorkspacesRequest);

            if (favoriteWorkspacesResponse.Status == false)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = favoriteWorkspacesResponse.Message
                });
            }


            var favoriteWorkspaces = favoriteWorkspacesResponse.Data.ToList().Select(favoriteWorkspace => new SimpleFavoriteWorkspaceViewModel()
            {
                Id = favoriteWorkspace.Id,
                WorkspaceId = favoriteWorkspace.WorkspaceId,
                UserId = favoriteWorkspace.UserId
            }).ToList();

            var workspaces = workspacesResponse.Data.ToList().Select(workspace => new WorkspaceViewModel()
            {
                Id = workspace.Id,
                Name = workspace.Name,
                Floor = workspace.Floor,
                Zone = workspace.Zone,
                HasMonitor = workspace.HasMonitor,
                HasDockingStation = workspace.HasDockingStation,
                IsNearWindow = workspace.IsNearWindow,
                IsNearPrinter = workspace.IsNearPrinter,
                ReservedDates = workspace.ReservedDates
            }).OrderByDescending(workspace => favoriteWorkspaces.Any(favoriteWorkspace => favoriteWorkspace.WorkspaceId == workspace.Id)).ToList();

            var viewModel = new CreateReservationViewModel()
            {
                Workspaces = workspaces,
                FavoriteWorkspaces = favoriteWorkspaces,
                WorkspaceId = workspaceId ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("create/{workspaceId?}")]
        public async Task<IActionResult> Create(CreateReservationViewModel viewModel)
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (viewModel.WorkspaceId == 0 || viewModel.ReservationDate == null)
            {
                ModelState.AddModelError(string.Empty, "Workspace and reservation date are required");

                var workspacesRequest = new GetAllWorkspacesRequest()
                {
                    User = GetUserSessionData()
                };

                var workspacesResponse = await _workspaceService.GetAllWorkspacesAsync(workspacesRequest);

                if (workspacesResponse.Status == false)
                {
                    return View("Error", new ErrorViewModel() { RequestId = workspacesResponse.Message });
                }

                var workspaces = workspacesResponse.Data.ToList().Select(workspace => new WorkspaceViewModel()
                {
                    Id = workspace.Id,
                    Name = workspace.Name,
                    Floor = workspace.Floor,
                    Zone = workspace.Zone,
                    HasMonitor = workspace.HasMonitor,
                    HasDockingStation = workspace.HasDockingStation,
                    IsNearWindow = workspace.IsNearWindow,
                    IsNearPrinter = workspace.IsNearPrinter,
                    ReservedDates = workspace.ReservedDates
                }).ToList();

                viewModel.Workspaces = workspaces;

                return View(viewModel);
            }

            var createReservationRequest = new CreateReservationRequest()
            {
                User = GetUserSessionData(),
                Data = new CreateReservationDto()
                {
                    WorkspaceId = viewModel.WorkspaceId,
                    ReservationDate = viewModel.ReservationDate
                }
            };

            var createReservationResponse = await _reservationService.CreateReservationAsync(createReservationRequest);

            if (createReservationResponse.Status == false)
            {
                ModelState.AddModelError(string.Empty, createReservationResponse.Message);

                var workspacesRequest = new GetAllWorkspacesRequest()
                {
                    User = GetUserSessionData()
                };

                var workspacesResponse = await _workspaceService.GetAllWorkspacesAsync(workspacesRequest);

                if (workspacesResponse.Status == false)
                {
                    return View("Error", new ErrorViewModel() { RequestId = workspacesResponse.Message });
                }

                var workspaces = workspacesResponse.Data.ToList().Select(workspace => new WorkspaceViewModel()
                {
                    Id = workspace.Id,
                    Name = workspace.Name,
                    Floor = workspace.Floor,
                    Zone = workspace.Zone,
                    HasMonitor = workspace.HasMonitor,
                    HasDockingStation = workspace.HasDockingStation,
                    IsNearWindow = workspace.IsNearWindow,
                    IsNearPrinter = workspace.IsNearPrinter,
                    ReservedDates = workspace.ReservedDates
                }).ToList();

                viewModel.Workspaces = workspaces;

                return View(viewModel);
            }

            return RedirectToAction("Index", "Reservation");
        }

        [HttpPost]
        [Route("quick-reserve")]
        public async Task<IActionResult> QuickReserve(int workspaceId)
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var quickReserveRequest = new QuickReserveRequest()
            {
                User = GetUserSessionData(),
                Data = new QuickReserveDto()
                {
                    WorkspaceId = workspaceId
                }
            };

            var quickReserveResponse = await _reservationService.QuickReserveAsync(quickReserveRequest);

            if (quickReserveResponse.Status == false)
            {
                return View("Error", new ErrorViewModel() { RequestId = quickReserveResponse.Message });
            }

            return RedirectToAction("Index", "Reservation");
        }


        [HttpGet]
        [Route("cancel/{reservationId}")]
        public async Task<IActionResult> Cancel(int reservationId)
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var cancelReservationRequest = new CancelReservationByIdRequest()
            {
                User = GetUserSessionData(),
                Data = new CancelReservationByIdDto()
                {
                    ReservationId = reservationId
                }
            };

            var cancelReservationResponse = await _reservationService.CancelReservationByIdAsync(cancelReservationRequest);

            if (cancelReservationResponse.Status == false)
            {
                return View("Error", new ErrorViewModel() { RequestId = cancelReservationResponse.Message });
            }

            return RedirectToAction("Index", "Reservation");
        }
    }
}