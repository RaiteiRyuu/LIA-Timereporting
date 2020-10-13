using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Yourworktime.Core.Models
{
    public class UserModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string Role { get; set; }
        public string ProfileImagePath { get; set; }
    }
}
