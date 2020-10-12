using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoServer.Core.Models
{
    public class ReportTimeModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid Owner { get; set; }
        public string Task { get; set; }
        public string Project { get; set; }
        public string Tag { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
