using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleAvailability : Record
    {
        public int Day;
        public virtual ICollection<TransportVehicleAvailabilityTimeBlock> TimeBlocks { get; set; }
    }

    public class TransportVehicleAvailabilityOverride : Record
    {
        public DateTime OverrideDate { get; set; }        
    }
}
