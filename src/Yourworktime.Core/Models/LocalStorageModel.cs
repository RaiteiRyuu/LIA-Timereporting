using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Yourworktime.Core.Models
{
    public class LocalStorageModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}
