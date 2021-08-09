using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DltaIngest.Models
{
    public class Azure
    {
        public Azure()
        {

        }

        [BsonId]
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public DateTime IngestionTimeStamp { get; set; }
        public string GroupName { get; set; }
        public string SubscriptionName { get; set; }
        public string DeltaID { get; set; }
        public string ParentID { get; set; }
        public string MetaDataApplied { get; set; }
        public List<string> DltaMeta { get; set; } = new List<string>();
    }
}
