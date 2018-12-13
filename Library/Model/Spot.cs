using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library.Model
{
    public class Spot
    {
        public Spot()
        {
        }

        public Spot(string name, string location, string status, DateTime time_Status, bool status_Battery, string park_Id)
        {
            Id = name;
            Location = location;
            Status = status;
            Time_Status = time_Status;
            Status_Battery = status_Battery;
            Park_Id = park_Id;
        }

        public string Id { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime Time_Status { get; set; }
        public Boolean Status_Battery { get; set; }
        public string Park_Id { get; set; }



        public XmlNode ToXML(XmlDocument doc)
        {
            //criar elemento
            XmlElement parkingSpot = doc.CreateElement("parkingSpot");
            XmlElement idSensor = doc.CreateElement("id");
            idSensor.InnerText = Park_Id;
            XmlElement nome = doc.CreateElement("name");
            nome.InnerText = Id;
            XmlElement location = doc.CreateElement("location");
            location.InnerText = Location;
            XmlElement statusValue = doc.CreateElement("status-value");
            statusValue.InnerText = Status;
            XmlElement timestramp = doc.CreateElement("status-timestamp");
            timestramp.InnerText = Time_Status.ToString("dd-MM-yyyy hh:mm");
            XmlElement battery = doc.CreateElement("batteryStatus");
            battery.InnerText = Status_Battery.ToString();
            parkingSpot.AppendChild(idSensor);
            parkingSpot.AppendChild(nome);
            parkingSpot.AppendChild(location);
            parkingSpot.AppendChild(statusValue);
            parkingSpot.AppendChild(timestramp);
            parkingSpot.AppendChild(battery);

            return parkingSpot;
        }

    }
}
