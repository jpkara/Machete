using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain
{
    public class TransportVehicleAvailabilityTimeBlock : Record
    {
        public virtual TransportVehicleAvailability ParentAvailability { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class TransportVehicleAvailabilityOverrideTimeBlock : Record
    {
        public virtual TransportVehicleAvailabilityOverride ParentOverride { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
