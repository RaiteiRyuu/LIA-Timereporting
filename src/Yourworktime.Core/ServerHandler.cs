using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
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

        public SigninService SigninService 
        {
            get 
            {
                if (_signinService == null)
                    _signinService = new SigninService(UserService, configuration);

                return _signinService;
            }
        }
        private SigninService _signinService;

        private MongoClient client 
        {
            get
            {
                if (_client == null)
                    Connect(configuration["ConnectionString"]);

                return _client;
            }
            set => _client = value;
        }
        private static MongoClient _client;

        private IConfiguration configuration;

        public ServerHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
            Connect(configuration["ConnectionString"]);
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
