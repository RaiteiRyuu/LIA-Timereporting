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
        private static string databaseName = "database";
        private static string userTableName = "users";
        public static string workspacetableName = "workspace";

        public UserService UserService 
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService(client.GetDatabase(databaseName), userTableName);

                return _userService;
            }
        }
        private UserService _userService;


        public WorkspaceService WorkspaceService
        {
            get
            {
                if (_workspaceService == null)
                    _workspaceService = new WorkspaceService(client.GetDatabase(databaseName), workspacetableName);

                return _workspaceService;
            }
        }
        private WorkspaceService _workspaceService;

        public SignInService SignInService 
        {
            get 
            {
                if (_signInService == null)
                    _signInService = new SignInService(UserService, configuration);

                return _signInService;
            }
        }
        private SignInService _signInService;

        public SignUpService SignUpService
        {
            get
            {
                if (_signUpService == null)
                    _signUpService = new SignUpService(UserService, configuration);

                return _signUpService;
            }
        }
        private SignUpService _signUpService;

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
