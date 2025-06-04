using FinalProject.Services.DTOs.Requests.Reservation.CancelReservationById;
using FinalProject.Services.DTOs.Requests.Reservation.CreateReservation;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationById;
using FinalProject.Services.DTOs.Requests.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.DTOs.Requests.Reservation.QuickReserve;
using FinalProject.Services.DTOs.Responses.Reservation.CancelReservationById;
using FinalProject.Services.DTOs.Responses.Reservation.CreateReservation;
using FinalProject.Services.DTOs.Responses.Reservation.GetReservationById;
using FinalProject.Services.DTOs.Responses.Reservation.GetReservationsForAuthUser;
using FinalProject.Services.DTOs.Responses.Reservation.QuickReserve;

namespace FinalProject.Services.Interfaces.Reservation
{
    public interface IReservationService
    {
        Task<GetReservationByIdResponse> GetReservationByIdAsync(GetReservationByIdRequest request);
        Task<GetReservationsForAuthUserResponse> GetCurrentReservationsForAuthUserAsync(GetReservationsForAuthUserRequest request);
        Task<CreateReservationResponse> CreateReservationAsync(CreateReservationRequest request);
        Task<QuickReserveResponse> QuickReserveAsync(QuickReserveRequest request);
        Task<CancelReservationByIdResponse> CancelReservationByIdAsync(CancelReservationByIdRequest request);
    }
}