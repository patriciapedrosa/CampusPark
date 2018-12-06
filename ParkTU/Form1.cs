using Library;
using Library.Model;
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
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkTU
{
    public partial class Form1 : Form
    {

        
        private MqttClient client;
        private string[] topics = { "data" , "spots"};
        private Boolean serviceEnabled;
        private Boolean ip;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkBoxService_CheckedChanged(object sender, EventArgs e)
        {
            if (serviceEnabled && checkBoxService.Checked == false)
            {
                Service_Disconect();
                serviceEnabled = !serviceEnabled;
            }
            else if (serviceEnabled == false && checkBoxService.Checked)
            {
                Boolean connected = Service_Connect();
                if (connected)
                {
                    serviceEnabled = !serviceEnabled;
                }
                else
                {
                    checkBoxService.Checked = false;
                }


            }
        }

        private Boolean Service_Disconect()
        {
            try
            {
                client.Unsubscribe(topics);
                if (client.IsConnected)
                {
                    client.Disconnect();
                }
                else
                {
                    client = null;
                    MessageBox.Show("Service 'Mosquitto Broker' unavailable!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem disconnecting!");
                return false;
            }
            return true;
        }

        private Boolean Service_Connect()
        {
            try {
                client = new MqttClient(textBoxIP.Text);
                client.Connect(Guid.NewGuid().ToString());
                if (!client.IsConnected)
                {
                    MessageBox.Show("Error conneting to broker!");
                }
                else
                {
                    client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
                    byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
                    client.Subscribe(topics, qosLevels);
                    return true;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Service 'Mosquitto Broker' unavailable!\nInnerExceptionMessage: " +
                                exception.InnerException.Message);
            }
            return false;
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if (e.Topic == "data")
            {
                
            }
            else if (e.Topic == "spots")
            {
                string msg = Encoding.UTF8.GetString(e.Message);
                ServiceParkingSpotClient servico = new ServiceParkingSpotClient();
                ParkingSpot[] spots = servico.GetSpots();
                /*foreach (Spot spot in spots)
                {
                   
                }*/
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serviceEnabled)
                Service_Disconect();
        }
    }
}
