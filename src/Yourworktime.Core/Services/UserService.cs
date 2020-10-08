using MongoDB.Bson;
using MongoDB.Driver;
using MongoServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
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

        // Authenticate user (When signing in)
        public async Task<bool> AuthenticateUser(string email, string password)
        {
            email = email.ToLower();

            // Check if email exists.
            List<UserModel> models = await LoadUsersByField("Email", email);
            if (models.Count == 0)
                return false;

            // Get user by email.
            UserModel user = models.First();

            // Hash password with user salt.
            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(password, user.Salt));

            // Compare hashed password with users hashed password.
            return user.Password.Equals(hashedPassword);
        }

        // Authorize user (When signing up)
        public async Task<bool> AuthorizeUser(UserModel model)
        {
            CleanUpUserModel(model);

            // Check is email already exists.
            if (await CountUsersByField("Email", model.Email) > 0)
                return false;

            // Create a hashed version of the password.
            string salt = Utils.GetSalt(16);
            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(model.Password, salt));

            // Set registration date.
            DateTime dateNow = DateTime.UtcNow;

            // Save user in database.
            UserModel newUser = new UserModel()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                RegisteredDate = dateNow,
                Salt = salt,
                Password = hashedPassword

            };
            await InsertUser(newUser);

            return true;
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

        private void CleanUpUserModel(UserModel model)
        {
            model.FirstName = model.FirstName.UppercaseFirst().Trim();
            model.LastName = model.LastName.UppercaseFirst().Trim();
            model.Email = model.Email.ToLower().Trim();
        }
    }
}
