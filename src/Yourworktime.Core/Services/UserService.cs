using MongoDB.Bson;
using MongoDB.Driver;
using MongoServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yourworktime.Core.Services
{
    public class UserService
    {
        private IMongoDatabase database;

        public UserService()
        {
            database = ServerHandler.GetDatabase("users");
        }

        // authenticate user (When signing in)
        public bool AuthenticateUser(string table, string email, string password)
        {
            // check if email exists
            // get user by email
            // hash password with user salt
            // compare hashed password with users hashed password

            return true;
        }

        // authorize user (When signing up)
        public bool AuthorizeUser(string table, UserModel model)
        {
            // check is email already exists
            // create a hashed version of the password
            // Set registration date 
            // save user in database

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
            return collection.Find(new BsonDocument()).ToList();
        }

        public UserModel LoadUserById(string table, Guid id)
        {
            var collection = database.GetCollection<UserModel>(table);
            var filter = Builders<UserModel>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
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
