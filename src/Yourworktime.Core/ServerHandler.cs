using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Yourworktime.Core.Services;

namespace Yourworktime.Core
{
    public class ServerHandler
    {
        private static string userDbName = "database";
        private static string userTableName = "users";

        public UserService UserService 
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService(client.GetDatabase(userDbName), userTableName);

                return _userService;
            }
        }
        private UserService _userService;

        private MongoClient client 
        {
            get
            {
                if (_client == null)
                    Connect(connectionString);

                return _client;
            }
            set => _client = value;
        }
        private static MongoClient _client;

        private string connectionString;

        public ServerHandler(string connectionString)
        {
            this.connectionString = connectionString;
            Connect(connectionString);
        }

        internal IMongoDatabase GetDatabase(string database)
        {
            return client.GetDatabase(database);
        }

        private void Connect(string connetionString)
        {
            client = new MongoClient(connetionString);
        }
    }
}
