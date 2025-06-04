using FinalProject.Repository.Base;
using FinalProject.Repository.Helpers;
using FinalProject.Repository.Interfaces.FavoriteWorkspace;
using Microsoft.Data.SqlClient;

namespace FinalProject.Repository.Implementations.FavoriteWorkspace
{
    public class FavoriteWorkspaceRepository : BaseRepository<Models.FavoriteWorkspace>, IFavoriteWorkspaceRepository
    {
        public FavoriteWorkspaceRepository(ConnectionFactory connectionFactory) : base(connectionFactory)
        {

        }

        private const string _idField = "id";
        protected override string GetTableName() => "favorite_workspaces";

        protected override IEnumerable<string> GetColumns() => new[] { _idField, "user_id", "workspace_id", "created_at" };

        protected override Models.FavoriteWorkspace MapReaderToEntity(SqlDataReader reader)
        {
            return new Models.FavoriteWorkspace
            {
                Id = Convert.ToInt32(reader[_idField]),
                UserId = Convert.ToInt32(reader["user_id"]),
                WorkspaceId = Convert.ToInt32(reader["workspace_id"]),
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
                { "CreatedAt", "created_at" },
            };
        }

        public Task<int> Create(Models.FavoriteWorkspace entity)
        {
            return base.Create(entity, _idField);
        }

        public Task<Models.FavoriteWorkspace> Retrieve(int id)
        {
            return base.Retrieve(_idField, id);
        }

        public Task<Models.FavoriteWorkspace> Retrieve(string id)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.FavoriteWorkspace> RetrieveAll(QueryParameters queryParameters = null)
        {
            return base.RetrieveAll(queryParameters);
        }

        public Task<bool> Update(int id, FavoriteWorkspaceUpdate update)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string id, FavoriteWorkspaceUpdate update)
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