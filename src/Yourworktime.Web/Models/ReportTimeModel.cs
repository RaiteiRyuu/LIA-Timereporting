using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yourworktime.Core.Models
{
    public class ReportTimeModel
    {
        public string Task { get; set; }
        public string Project { get; set; }
        public string Tag { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
