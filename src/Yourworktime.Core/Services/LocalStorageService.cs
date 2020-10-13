using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Yourworktime.Core.Models;

namespace Yourworktime.Core.Services
{
    public class LocalStorageService
    {
        private IMongoDatabase database;
        private string tableName;

        public LocalStorageService(IMongoDatabase database, string tableName)
        {
            this.database = database;
            this.tableName = tableName;
        }

        // Create
        public async Task InsertItem(LocalStorageModel obj)
        {
            IMongoCollection<LocalStorageModel> collection = database.GetCollection<LocalStorageModel>(tableName);
            var filter = Builders<LocalStorageModel>.Filter.Eq("Key", obj.Key);

            if (await collection.FindAsync<LocalStorageModel>(filter) != null)
                await collection.InsertOneAsync(obj);
        }

        // Read
        public async Task<LocalStorageModel> LoadItemByKey(string key)
        {
            key = GetModifiedKey(key);
            var collection = database.GetCollection<LocalStorageModel>(tableName);
            var filter = Builders<LocalStorageModel>.Filter.Eq("Key", key);

            var found = await collection.FindAsync(filter);

            var sfoundList = found.ToList();
            return sfoundList.Count == 0 ? null : sfoundList.First();
        }

        // Update
        public async Task UpsertItem<T>(string key, T value)
        {
            key = GetModifiedKey(key);
            var collection = database.GetCollection<LocalStorageModel>(tableName);
            var filter = Builders<LocalStorageModel>.Filter.Eq("Key", key);

            await collection.ReplaceOneAsync(
                filter: new BsonDocument("Key", key),
                options: new ReplaceOptions { IsUpsert = true },
                replacement: new LocalStorageModel() { Key = key, Value = value });
        }

        // Delete
        public async Task DeleteItem(string key)
        {
            key = GetModifiedKey(key);
            var collection = database.GetCollection<LocalStorageModel>(tableName);
            var filter = Builders<LocalStorageModel>.Filter.Eq("Key", key);

            await collection.DeleteOneAsync(filter);
        }

        private string GetModifiedKey(string key)
        {
            return string.Concat(key, "_", Utils.GetHashedLocalIpAddress());
        }
    }
}
