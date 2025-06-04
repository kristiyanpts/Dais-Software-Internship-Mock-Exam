using FinalProject.Repository.Base;
using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.Reservation;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations.Reservation
{
    public class ReservationRepository : BaseRepository<Models.Reservation>, IReservationRepository
    {
        public ReservationRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }

        private const string _idField = "id";
        protected override string GetTableName() => "reservations";

        protected override IEnumerable<string> GetColumns() => new[] { _idField, "user_id", "workspace_id", "reservation_date", "is_quick_reservation", "created_at" };

        protected override Models.Reservation MapReaderToEntity(SqlDataReader reader)
        {
            return new Models.Reservation
            {
                Id = Convert.ToInt32(reader[_idField]),
                UserId = Convert.ToInt32(reader["user_id"]),
                WorkspaceId = Convert.ToInt32(reader["workspace_id"]),
                ReservationDate = Convert.ToDateTime(reader["reservation_date"]),
                IsQuickReservation = Convert.ToBoolean(reader["is_quick_reservation"]),
                CreatedAt = Convert.ToDateTime(reader["created_at"]),
            };
        }

        protected override Dictionary<string, string> MapPropertiesToColumns()
        {
            return new Dictionary<string, string>
            {
                { "Id", _idField },
                { "UserId", "user_id" },
                { "WorkspaceId", "workspace_id" },
                { "ReservationDate", "reservation_date" },
                { "IsQuickReservation", "is_quick_reservation" },
                { "CreatedAt", "created_at" },
            };
        }

        public Task<int> Create(Models.Reservation entity)
        {
            return base.Create(entity, _idField);
        }

        public Task<Models.Reservation> Retrieve(int id)
        {
            return base.Retrieve(_idField, id);
        }

        public Task<Models.Reservation> Retrieve(string id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.Reservation> RetrieveAll(QueryParameters queryParameters = null)
        {
            return base.RetrieveAll(queryParameters);
        }

        public Task<bool> Update(int id, ReservationUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string id, ReservationUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            return base.Delete(_idField, id);
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}