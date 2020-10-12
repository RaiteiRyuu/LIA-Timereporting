using MongoDB.Bson;
using MongoDB.Driver;
using MongoServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yourworktime.Core.Services
{
    class ReportTimeService
    {
        private IMongoDatabase database;
        private string tableName;

        public ReportTimeService(IMongoDatabase database, string tableName)
        {
            this.database = database;
            this.tableName = tableName;
        }

        // Create
        public async Task InsertReport(ReportTimeModel report)
        {
            IMongoCollection<ReportTimeModel> collection = database.GetCollection<ReportTimeModel>(tableName);
            await collection.InsertOneAsync(report);
        }

        // Read
        public async Task<List<ReportTimeModel>> LoadReportsFromUserId(Guid userId)
        {
            IMongoCollection<ReportTimeModel> collection = database.GetCollection<ReportTimeModel>(tableName);
            var filter = Builders<ReportTimeModel>.Filter.Eq("Owner", userId);

            var found = await collection.FindAsync(filter);
            return found.ToList();
        }

        // Update
        public async Task UpsertReport(Guid id, ReportTimeModel report)
        {
            var collection = database.GetCollection<ReportTimeModel>(tableName);

            await collection.ReplaceOneAsync(
                new BsonDocument("Id", id),
                report,
                new ReplaceOptions { IsUpsert = true });
        }

        // Delete
        public async Task DeleteReport(Guid id)
        {
            var collection = database.GetCollection<ReportTimeModel>(tableName);
            var filter = Builders<ReportTimeModel>.Filter.Eq("Id", id);
            await collection.DeleteOneAsync(filter);
        }
    }
}
