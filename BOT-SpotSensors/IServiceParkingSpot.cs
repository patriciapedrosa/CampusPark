using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BOT_SpotSensors
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    //
    public interface IServiceParkingSpot
    {
        [OperationContract]
        List<ParkingSpot> GetSpots();
    }


    [DataContract]
    public class ParkingSpot
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public DateTime Timestramp { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public Boolean Battery { get; set; }
    }
}
