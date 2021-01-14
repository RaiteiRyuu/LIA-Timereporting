using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Yourworktime.Core.Models;

namespace Yourworktime.Core.Services
{
     public class WorkspaceService
    {
        private IMongoDatabase database;
        private string tableName;

        public WorkspaceService(IMongoDatabase database, string tableName)
        {
            this.database = database;
            this.tableName = tableName;
        }

        // Create
        public async Task InsertWorkspace(WorkspaceModel workspace)
        {
            IMongoCollection<WorkspaceModel> collection = database.GetCollection<WorkspaceModel>(tableName);
            await collection.InsertOneAsync(workspace);
        }

        // Read
        public async Task<List<WorkspaceModel>> LoadWorkspacesOfUserOwner(Guid userId)
        {
            IMongoCollection<WorkspaceModel> collection = database.GetCollection<WorkspaceModel>(tableName);
            var filter = Builders<WorkspaceModel>.Filter.Eq("Owner", userId);

            var found = await collection.FindAsync(filter);
            return found.ToList() ?? null;
        }

        public async Task<WorkspaceModel> LoadWorkspaceById(Guid id)
        {
            var collection = database.GetCollection<WorkspaceModel>(tableName);
            var filter = Builders<WorkspaceModel>.Filter.Eq("Id", id);

            var found = await collection.FindAsync(filter);
            if (!found.Any())
                return null;
            return found.First();
        }

        public async Task<List<WorkspaceModel>> LoadWorkspacesByField<T>(string field, T value)
        {
            var collection = database.GetCollection<WorkspaceModel>(tableName);
            var filter = Builders<WorkspaceModel>.Filter.Eq(field, value);

            var found = await collection.FindAsync(filter);
            return found.ToList();
        }

        // Update
        public void UpsertWorkspace(Guid id, WorkspaceModel workspace)
        {
            var collection = database.GetCollection<WorkspaceModel>(tableName);

            var result = collection.ReplaceOne(
                new BsonDocument("Id", id),
                workspace,
                new ReplaceOptions { IsUpsert = true });
        }

        // Delete
        public void DeleteWorkspace(Guid id)
        {
            var collection = database.GetCollection<WorkspaceModel>(tableName);
            var filter = Builders<WorkspaceModel>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
