using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Web.Models.ViewModels.Error;
using FinalProject.Web.Controllers.Base;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.Interfaces.Reservation;
using FinalProject.Services.DTOs.Responses.Reservation;
using FinalProject.Web.Models.ViewModels.Reservation;
using FinalProject.Web.Models.ViewModels.Workspace;
using FinalProject.Web.Models.ViewModels.Home;

namespace FinalProject.Web.Controllers.Home
{
    public class HomeController : BaseController
    {
        private readonly IReservationService _reservationService;

        public HomeController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            if (GetUserId() == 0)
            {
                return RedirectToAction("Login", "Authentication");
            }

            var getReservationsForAuthUserRequest = new GetReservationsForAuthUserRequest()
            {
                User = GetUserSessionData()
            };

            var getReservationsForAuthUserResponse = await _reservationService.GetCurrentReservationsForAuthUserAsync(getReservationsForAuthUserRequest);

            if (getReservationsForAuthUserResponse.Status == false)
            {
                return View("Error", new ErrorViewModel()
                {
                    RequestId = getReservationsForAuthUserResponse.Message
                });
            }

            var reservations = getReservationsForAuthUserResponse.Data.ToList().Select(reservation => new ReservationViewModel()
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
                    IsNearPrinter = reservation.Workspace.IsNearPrinter
                },
                UserId = reservation.UserId,
                UserName = reservation.UserName,
                ReservationDate = reservation.ReservationDate,
                IsQuickReservation = reservation.IsQuickReservation,
                CreatedAt = reservation.CreatedAt
            }).ToList();

            var viewModel = new HomeViewModel()
            {
                Reservations = reservations
            };

            return View(viewModel);
        }
    }
}
