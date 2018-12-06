using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace BOT_SpotSensors
{
   
    public class Service1 : IServiceParkingSpot
    {

        public List<ParkingSpot> GetSpots()
        {
            List<ParkingSpot> spots = new List<ParkingSpot>();
            string fileXml = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\parkingSpot.xml";
            string fileXsd = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\parkingSpot.xsd";

            Library.HandlerXml myclass = new Library.HandlerXml(fileXml, fileXsd);
            bool valid = myclass.ValidateXml();
            if (valid)
            {

                XmlDocument doc = new XmlDocument();

                doc.Load(fileXml);
                XmlNodeList r = doc.SelectNodes("/parkingSpots/parkingSpot");
                foreach (XmlNode item in r)
                {
                    ParkingSpot s = new ParkingSpot();
                    s.Id = item["id"].InnerText;
                    s.Name = item["name"].InnerText;
                    s.Location = item["location"].InnerText;
                    s.Status = item["status-value"].InnerText;
                    s.Timestramp = DateTime.Parse(item["status-timestamp"].InnerText);
                    s.Battery = XmlConvert.ToBoolean(item["batteryStatus"].InnerText);

                    spots.Add(s);
                }
            }
            else
            {
                throw new Exception("Message received from DLL does not respect schema rules. Message: '" + 
                    "'" + myclass.ValidationMessage + Environment.NewLine);
            }
            

                 return spots;

        }
    }
}
