using FinalProject.Repository.Base;
using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.Workspace;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations.Workspace
{
    public class WorkspaceRepository : BaseRepository<Models.Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }

        private const string _idField = "id";
        protected override string GetTableName() => "workspaces";

        protected override IEnumerable<string> GetColumns() => new[] { _idField, "name", "floor", "zone", "has_monitor", "has_docking_station", "is_near_window", "is_near_printer" };

        protected override Models.Workspace MapReaderToEntity(SqlDataReader reader)
        {
            return new Models.Workspace
            {
                Id = Convert.ToInt32(reader[_idField]),
                Name = Convert.ToString(reader["name"]),
                Floor = Convert.ToInt32(reader["floor"]),
                Zone = Convert.ToString(reader["zone"]),
                HasMonitor = Convert.ToBoolean(reader["has_monitor"]),
                HasDockingStation = Convert.ToBoolean(reader["has_docking_station"]),
                IsNearWindow = Convert.ToBoolean(reader["is_near_window"]),
                IsNearPrinter = Convert.ToBoolean(reader["is_near_printer"]),
            };
        }

        protected override Dictionary<string, string> MapPropertiesToColumns()
        {
            return new Dictionary<string, string>
            {
                { "Id", _idField },
                { "Name", "name" },
                { "Floor", "floor" },
                { "Zone", "zone" },
                { "HasMonitor", "has_monitor" },
                { "HasDockingStation", "has_docking_station" },
                { "IsNearWindow", "is_near_window" },
                { "IsNearPrinter", "is_near_printer" },
            };
        }

        public Task<int> Create(Models.Workspace entity)
        {
            return base.Create(entity, _idField);
        }

        public Task<Models.Workspace> Retrieve(int id)
        {
            return base.Retrieve(_idField, id);
        }

        public Task<Models.Workspace> Retrieve(string id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.Workspace> RetrieveAll(QueryParameters queryParameters = null)
        {
            return base.RetrieveAll(queryParameters);
        }

        public Task<bool> Update(int id, WorkspaceUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string id, WorkspaceUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}