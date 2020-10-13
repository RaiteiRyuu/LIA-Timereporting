using MongoDB.Bson;
using MongoDB.Driver;
using Yourworktime.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourworktime.Core.Services
{
    public class UserService
    {
        private IMongoDatabase database;
        private string tableName;

        public UserService(IMongoDatabase database, string tableName)
        {
            this.database = database;
            this.tableName = tableName;
        }

        // Create
        public async Task InsertUser(UserModel user)
        {
            IMongoCollection<UserModel> collection = database.GetCollection<UserModel>(tableName);
            await collection.InsertOneAsync(user);
        }

        // Read
        public async Task<List<UserModel>> LoadUsers()
        {
            IMongoCollection<UserModel> collection = database.GetCollection<UserModel>(tableName);

            var found = await collection.FindAsync(new BsonDocument());
            return found.ToList();
        }

        public async Task<UserModel> LoadUserById(Guid id)
        {
            var collection = database.GetCollection<UserModel>(tableName);
            var filter = Builders<UserModel>.Filter.Eq("Id", id);

            var found = await collection.FindAsync(filter);
            if (!found.Any())
                return null;
            return found.First();
        }

        public async Task<List<UserModel>> LoadUsersByField<T>(string field, T value)
        {
            var collection = database.GetCollection<UserModel>(tableName);
            var filter = Builders<UserModel>.Filter.Eq(field, value);

            var found = await collection.FindAsync(filter);
            return found.ToList();
        }

        public async Task<long> CountUsersByField<T>(string field, T value)
        {
            var collection = database.GetCollection<UserModel>(tableName);
            var filter = Builders<UserModel>.Filter.Eq(field, value);

            return await collection.CountDocumentsAsync(filter);
        }

        // Update
        public void UpsertUser(Guid id, UserModel user)
        {
            var collection = database.GetCollection<UserModel>(tableName);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                user,
                new ReplaceOptions { IsUpsert = true });
        }

        // Delete
        public void DeleteUser(Guid id)
        {
            var collection = database.GetCollection<UserModel>(tableName);
            var filter = Builders<UserModel>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
