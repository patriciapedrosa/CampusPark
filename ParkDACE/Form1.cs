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
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkDACE
{
    public partial class Form1 : Form
    {
        //xml e xsd path
        string fileXml = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/ParkDACE/App_Data/ParkingNodesConfig.xml");
        string fileXsd = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/ParkDACE/App_Data/ParkingNodesConfig.xsd");
        string xmlParkingspot = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/Library/App_Data/parkingSpot.xml");
        //excel path
        string fileExcel = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "CampusPark/ParkDACE/App_Data/");
        //xml
        XmlDocument doc = new XmlDocument();
        //dll
        private BackgroundWorker bw = new BackgroundWorker();
        private ParkingSensorNodeDll.ParkingSensorNodeDll dll = null;
        //mosquitto
        private Boolean serviceEnabled;
        private MqttClient broker;
        private string[] topics ={"spots", "park"};
        private string ip = "127.0.0.1";



        public Form1()
        {
            InitializeComponent();
            comboBoxTopic.DataSource = topics;
            comboBoxTopic.SelectedIndex = 0;
        }

        //iniciar ou parar dll e bloker
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

        //iniciar serviço
        private Boolean Service_Connect()
        {
            Library.HandlerXml myclass = new Library.HandlerXml(fileXml, fileXsd);
            bool valid = myclass.ValidateXml();
            if (valid)
            {
                try
                {
                    dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();
                    dll.Initialize(ReadDataFromDll, 1000);
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

        private void ConnectAndPublish()
        {
            broker = new MqttClient(ip);
            broker.Connect(Guid.NewGuid().ToString());
            if (!broker.IsConnected)
            {
                MessageBox.Show("error connecting to broker");
            }

            broker.MqttMsgUnsubscribed += Broker_MqttMsgUnsubscribed;

            if (topics[0] == comboBoxTopic.SelectedValue.ToString())
            {
                List<string> listaSpots = new List<string>();

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlParkingspot);
                XmlNodeList spots = doc.SelectNodes("/parkingSpots/parkingSpot");
                foreach (XmlNode item in spots)
                {
                    Spot s = new Spot();
                    s.Park_Id = item["id"].InnerText;
                    s.Id = item["name"].InnerText;
                    s.Location = item["location"].InnerText;
                    s.Status = item["status-value"].InnerText;
                    s.Time_Status = DateTime.Parse(item["status-timestamp"].InnerText);
                    s.Status_Battery = bool.Parse(item["batteryStatus"].InnerText);

                    string msgEnviar = s.Park_Id + ";" + s.Id + ";" + s.Location + ";" + s.Status + ";" + s.Time_Status + ";" + s.Status_Battery;
                    listBox1.Items.Add(msgEnviar);
                    byte[] msg = Encoding.ASCII.GetBytes(msgEnviar);
                    string topic = comboBoxTopic.SelectedValue.ToString();
                    broker.Publish(topic, msg);
                }
            }
            else if (topics[1] == comboBoxTopic.SelectedValue.ToString())
            {



                List<string> listaParks = new List<string>();
                doc.Load(fileXml);
                XmlNodeList parks = doc.SelectNodes("/parkingLocation/provider/parkInfo");
                foreach (XmlNode item in parks)
                {
                    Park p = new Park();
                    p.Description = item["description"].InnerText;
                    p.Id = item["id"].InnerText;
                    p.Number_Spots = int.Parse(item["numberOfSpots"].InnerText);
                    p.Operating_Hours = item["operatingHours"].InnerText;
                    p.Special_Spots = int.Parse(item["numberOfSpecialSpots"].InnerText);


                    string msgEnviar = p.Id + ";" + p.Number_Spots + ";" + p.Operating_Hours + ";" + p.Special_Spots + ";" + p.Description;
                    listBox1.Items.Add(msgEnviar);
                    byte[] msg = Encoding.ASCII.GetBytes(msgEnviar);
                    string topic = comboBoxTopic.SelectedValue.ToString();
                    broker.Publish(topic, msg);
                }
            }

        }


        private void Broker_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            MessageBox.Show("unsubscribe done!");
        }


        //parar serviço
        private void Service_Disconect()
        {
            dll.Stop();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private static void releaseCOMObject(Object obj)
        {
            try
            {
                //free memory 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                System.Diagnostics.Debug.WriteLine("exception releasing COM object: " + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        

        public void AddLocation(string nome, string Newlocation)
        {
           
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlParkingspot);

            XmlNode location = doc.SelectSingleNode("parkingSpots/parkingSpot[name='" + nome + "']/location");
            location.InnerText = Newlocation;
            
            doc.Save(xmlParkingspot);
        }




        //ler dados da dll, acrescentar ao xml
        private void ReadDataFromDll(string message)
        {
            //apresentar na caixa
            this.BeginInvoke((MethodInvoker)delegate
            {
                listBox1.Items.Add(message + Environment.NewLine);
            });
            Spot spot = GetElementosdaDll(message);
            AddData(spot);

            
        }

        //partir dados da dll 
        private Spot GetElementosdaDll(string str)
        {
            Spot s = new Spot();

            char delimiter = ';';
            string[] elementos = str.Split(delimiter);
            s.Id = elementos[1];
            s.Location = "";
            s.Park_Id = elementos[0];
            s.Status = elementos[3];
            if (s.Status == "0")
            {
                s.Status = "empty";
            }
            else
            {
                s.Status = "occupied";
            }
            if (elementos[4] == "0")
            {
                s.Status_Battery = false;
            }
            else
            {
                s.Status_Battery = true;
            }
            s.Time_Status = DateTime.Parse(elementos[2]);

            return s;
        }

        //adicionar elementos da dll no xml
        public void AddData(Spot elementoLido)
        {
                doc.Load(xmlParkingspot);
                XmlNode node = doc.SelectSingleNode("/parkingSpots");
                Library.HandlerXml myclass = new Library.HandlerXml(xmlParkingspot);

                if (node != null)
                {
                    node.AppendChild(elementoLido.ToXML(doc));
                    myclass.XmlData = doc.OuterXml;
                }
                doc.Save(xmlParkingspot);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (broker.IsConnected)
            {
                broker.Disconnect();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            ServiceParkingSpotClient servico = new ServiceParkingSpotClient();
            ParkingSpot[] spots = servico.GetSpots();

            foreach (ParkingSpot s in spots)
            {
                DateTime now = DateTime.Now;
                s.Timestramp = now;
                //add ao xml
                //listBox1.Items.Add(s.Id + ";" + s.Name + ";" + s.Status + ";" + s.Timestramp + ";" + s.Battery);
                AddData(new Spot(s.Name,s.Location,s.Status,s.Timestramp,s.Battery,s.Id));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
        doc.Load(fileXml);
        XmlNodeList r = doc.SelectNodes("/parkingLocation/provider/parkInfo");
            foreach (XmlNode item in r)
            {
                String location;
                location = item["geoLocationFile"].InnerText;

                Excel.Application excelApp = new Excel.Application();
                excelApp.Visible = true;

                Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(fileExcel + location);
                Excel.Worksheet excelWorkSheet = excelWorkbook.ActiveSheet;
                Excel.Range excelRange = excelWorkSheet.UsedRange;

                string content;
                int rCnt;
                int cCnt;
                int rw = excelRange.Rows.Count;
                int cl = excelRange.Columns.Count;

                for (rCnt = 6; rCnt <= rw; rCnt++)
                {
                    for (cCnt = 1; cCnt <= cl; cCnt++)
                    {
                        content = (string)(excelRange.Cells[rCnt, cCnt] as Excel.Range).Text;
                        listBox1.Items.Add(content);


                        string no = excelWorkSheet.Cells[rCnt, 1].value;
                        if (no != null)
                        {
                            string lo = excelWorkSheet.Cells[rCnt, 2].value;
                            AddLocation(no, lo);
                        }

                    }
                }

                excelWorkbook.Close();
                excelApp.Quit();
                releaseCOMObject(excelRange);
                releaseCOMObject(excelWorkSheet);
                releaseCOMObject(excelWorkbook);
                releaseCOMObject(excelApp);
            }
            listBox1.Items.Clear();

            ConnectAndPublish();
        }
    }
}
