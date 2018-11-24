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
        string filePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"App_Data\parkingSpot.xml";

        public List<Spot> GetSpots()
        {
            XmlDocument doc = new XmlDocument();
            List<Spot> spots = new List<Spot>();
            doc.Load(filePath);
            XmlNodeList root = doc.SelectNodes("/parkingSpots/parkingSpot/status");
            XmlNodeList r = doc.SelectNodes("/parkingSpots/parkingSpot");
            foreach (XmlNode item in r)
            {
                Spot s = new Spot();
                s.Id = item["id"].InnerText;
                s.Name = item["name"].InnerText;
                s.Battery = XmlConvert.ToBoolean(item["batteryStatus"].InnerText);
                foreach (XmlNode i in root)
                {
                    s.Timestramp = DateTime.Parse(i["timestamp"].InnerText);
                    s.Status = i["value"].InnerText;
                }

                spots.Add(s);
            }
            

            return spots;
        }
    }
}
