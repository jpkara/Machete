using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Domain

{
    public class TransportVehicle : Record
    {
        public string Name;
        public int Capacity;
        public int transportProviderID { get; set; }
        public virtual TransportProvider TransportProvider { get; set; }
        public virtual ICollection<TransportVehicleAvailability> Availability { get; set; }
        public virtual ICollection<TransportVehicleAvailabilityOverride> AvailabilityOverrides { get; set; }

    }
}
