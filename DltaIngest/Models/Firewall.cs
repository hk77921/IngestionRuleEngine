using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DltaIngest.Models
{
    public class Firewall
    {
        public string SqlServerName { get; set; }
        public string Kind { get; set; }
        public string StartIPAddress { get; set; }
        public string ParentId { get; set; }
        public string EndIPAddress { get; set; }
    }
}
