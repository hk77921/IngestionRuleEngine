using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DltaIngest.Models
{
   public class SqlDatabase
    {
        public string ElasticPoolName { get; set; }
        public string RegionName { get; set; }
        public string Edition { get; set; } //DatabaseEdition is enum type
        public Guid? CurrentServiceObjectiveId { get; set; }
        public long MaxSizeBytes { get; set; }
        public string DefaultSecondaryLocation { get; set; }
        public Guid? RequestedServiceObjectiveId { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public string Collation { get; set; }
        public DateTime? EarliestRestoreDate { get; set; }
        public string RequestedServiceObjectiveName { get; set; }  //ServiceObjectiveName enum type 
        public string SqlServerName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ParentId { get; set; }
        public string ServiceLevelObjective { get; set; } //ServiceObjectiveName is enum type
        public string Region { get; set; }  // Region enum type
        public bool IsDataWarehouse { get; set; }
        public string DatabaseId { get; set; }
    }
}
