using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Yourworktime.Core.Services;

namespace Yourworktime.Core
{
    public static class ServerHandler
    {
        private const string connectionStr = "mongodb+srv://admin:tVLvP36sRxn7ALPZ@cluster0.b1jpo.azure.mongodb.net/database?retryWrites=true&w=majority";

        public static UserService UserService 
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService();

                return _userService;
            }
        }
        private static UserService _userService;

        private static MongoClient client 
        {
            get
            {
                if (_client == null)
                    Connect(connectionStr);

                return _client;
            }
            set => _client = value;
        }
        private static MongoClient _client;

        internal static IMongoDatabase GetDatabase(string database)
        {
            return client.GetDatabase(database);
        }

        private static void Connect(string connetionString)
        {
            client = new MongoClient(connetionString);
        }
    }
}
