using MongoDB.Bson;
using MongoDB.Driver;
using MongoServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Yourworktime.Core.Services
{
    // Todo: Make methods async!
    // Todo: Use JWT
    // Todo: don't return null, return empty object
    public class UserService
    {
        private static string databaseName = "database";
        private static string tableName = "users";

        private IMongoDatabase database;

        public UserService()
        {
            database = ServerHandler.GetDatabase(databaseName);
        }

        // authenticate user (When signing in)
        public bool AuthenticateUser(string email, string password)
        {
            // check if email exists
            // get user by email
            // hash password with user salt
            // compare hashed password with users hashed password

            return true;
        }

        // authorize user (When signing up)
        public bool AuthorizeUser(UserModel model)
        {
            // check is email already exists
            UserModel user = LoadUserByField(tableName, "Email", model.Email.ToLower());
            if (user != null)
                return false;

            // create a hashed version of the password
            string salt = Utils.GetSalt(32);
            string hashedPassword = Utils.ComputeSha256Hash(string.Concat(model.Password, salt));

            // set registration date
            DateTime dateNow = DateTime.UtcNow;

            // save user in database
            UserModel newUser = new UserModel()
            {
                FirstName = model.FirstName.UppercaseFirst(),
                LastName = model.LastName.UppercaseFirst(),
                Email = model.Email.ToLower(),
                RegisteredDate = dateNow,
                Hash = salt,
                Password = hashedPassword

            };
            InsertUser(tableName, newUser);

            return true;
        }

        // Create
        public void InsertUser(string table, UserModel user)
        {
            IMongoCollection<UserModel> collection = database.GetCollection<UserModel>(table);
            collection.InsertOne(user);
        }

        // Read
        public List<UserModel> LoadUsers(string table)
        {
            IMongoCollection<UserModel> collection = database.GetCollection<UserModel>(table);

            var found = collection.Find(new BsonDocument());
            if (found.CountDocuments() == 0)
            {
                return null;
            }
            return found.ToList();
        }

        public UserModel LoadUserById(string table, Guid id)
        {
            var collection = database.GetCollection<UserModel>(table);
            var filter = Builders<UserModel>.Filter.Eq("Id", id);

            var found = collection.Find(filter);
            if (found.CountDocuments() == 0)
            {
                return null;
            }
            return found.First();
        }

        public UserModel LoadUserByField<T>(string table, string field, T value)
        {
            var collection = database.GetCollection<UserModel>(table);
            var filter = Builders<UserModel>.Filter.Eq(field, value);

            var found = collection.Find(filter);
            if (found.CountDocuments() == 0)
            {
                return null;
            }
            return found.First();
        }

        // Update
        public void UpsertUser(string table, Guid id, UserModel record)
        {
            var collection = database.GetCollection<UserModel>(table);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        // Delete
        public void DeleteUser(string table, Guid id)
        {
            var collection = database.GetCollection<UserModel>(table);
            var filter = Builders<UserModel>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
