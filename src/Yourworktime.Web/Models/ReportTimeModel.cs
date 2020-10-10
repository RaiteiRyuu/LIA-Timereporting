using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yourworktime.Core.Models
{
    class ReportTimeModel
    {
        public string Task { get; set; }
        public string Project { get; set; }
        public string Tag { get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public DateTime Date { get; set; }
    }
}
