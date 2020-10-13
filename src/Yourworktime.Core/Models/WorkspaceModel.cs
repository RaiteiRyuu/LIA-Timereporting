using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yourworktime.Core.Models
{
    public class WorkspaceModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string  Name { get; set; }
        public string ProfileImagePath { get; set; }
        public Guid Owner { get; set; }
        public Guid[] Members { get; set; }
        public DateTime RegisteredTime { get; set; }
    }
}

