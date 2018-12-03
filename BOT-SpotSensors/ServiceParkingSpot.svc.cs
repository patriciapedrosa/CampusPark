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
            List<Spot> spots = new List<Spot>();
            string fileXml = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\parkingSpot.xml";
            string fileXsd = AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\parkingSpot.xsd";

            Library.HandlerXml myclass = new Library.HandlerXml(fileXml, fileXsd);
            bool valid = myclass.ValidateXml();
            if (valid)
            {

                XmlDocument doc = new XmlDocument();

                doc.Load(filePath);
                XmlNodeList r = doc.SelectNodes("/parkingSpots/parkingSpot");
                foreach (XmlNode item in r)
                {
                    Spot s = new Spot();
                    s.Id = item["id"].InnerText;
                    s.Name = item["name"].InnerText;
                    s.Battery = XmlConvert.ToBoolean(item["batteryStatus"].InnerText);

                    s.Timestramp = DateTime.Parse(item["status-timestamp"].InnerText);
                    s.Status = item["status-value"].InnerText;


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
