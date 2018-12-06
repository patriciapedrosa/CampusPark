using ParkDACE.ServiceParkingSpot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using Library.Model;
using System.Xml;
using System.IO;

namespace ParkDACE
{
    public partial class Form1 : Form
    {
        //get xml e xsd path
        string fileXml = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/ParkDACE/App_Data/ParkingNodesConfig.xml");
        string fileXsd = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/ParkDACE/App_Data/ParkingNodesConfig.xsd");

        XmlDocument doc = new XmlDocument();
        //dll
        private BackgroundWorker bw = new BackgroundWorker();
        private ParkingSensorNodeDll.ParkingSensorNodeDll dll = null;

        //mosquitto
        private Boolean serviceEnabled;
        private MqttClient broker;
        private string topic = "data";
        private string ip = "172.";

        public Form1()
        {
            InitializeComponent();
        }


    public void NewSensorValueFunction(string str)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                listBox1.Items.Add(str);
            });
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ServiceParkingSpotClient servico = new ServiceParkingSpotClient();
            ParkingSpot[] spots = servico.GetSpots();

            foreach (ParkingSpot s in spots)
            {
                listBox1.Items.Add(s.Id + ";" + s.Name + ";" + s.Status + ";" + s.Timestramp + ";" + s.Battery);
            }

        }

        private void checkBoxDll_CheckedChanged(object sender, EventArgs e)
        {
            if (serviceEnabled && checkBoxDll.Checked == false)
            {
                Service_Disconect();
                serviceEnabled = !serviceEnabled;
            }
            else if (serviceEnabled == false && checkBoxDll.Checked)
            {
                Boolean connected = Service_Connect();
                if (connected)
                {
                    serviceEnabled = !serviceEnabled;
                }
                else
                {
                    checkBoxDll.Checked = false;
                }
            }
        }

        private Boolean Service_Connect()
        {
            Library.HandlerXml myclass = new Library.HandlerXml(fileXml, fileXsd);
            bool valid = myclass.ValidateXml();
            if (valid)
            {
                try
                {
                    dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();
                    dll.Initialize(NewSensorValueFunction, 1000);
                    bw.RunWorkerAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            else
            {
                MessageBox.Show("Message: '" + myclass.ValidationMessage + Environment.NewLine);
                return false;
            }
            
        }

        private void Service_Disconect()
        {
            dll.Stop();
            if (broker.IsConnected)
            {
                broker.Disconnect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<Spot> sp= GetLocation();

            foreach (Spot s in sp)
            {
                listBox1.Items.Add(s.Location);
            }

        }

        public List<Spot> GetLocation()
        {
            List<Spot> spots = new List<Spot>();
            

                
                doc.Load(fileXml);
                XmlNodeList r = doc.SelectNodes("/parkingLocation/provider/parkInfo");
                foreach (XmlNode item in r)
                {
                    Spot p = new Spot();
                    p.Location = item["geoLocationFile"].InnerText;

                    spots.Add(p);
                }
           //get excel numbers

            return spots;

        }

        //final mostrar na consola valores enviados


    }
}
