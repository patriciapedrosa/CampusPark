﻿using Library;
using Library.Model;
using ParkDACE.ServiceParkingSpot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkTU
{
    public partial class Form1 : Form
    {

        static string CONNECTIONSTR = "Server=f0bd6467-8d2c-4782-aee0-a9a501091e04.sqlserver.sequelizer.com;Database=dbf0bd64678d2c4782aee0a9a501091e04;User ID=spjoenncymdyiakz;Password=J6ZRZ4Ex46AYiijuagUPuW7jPTnZxYVZFLYkDkAXe8MneQ6YtV7moRJU7PbgQNae;";
        private MqttClient client;
        private string[] topics = { "park" , "spots"};
        private Boolean serviceEnabled;

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
                    MessageBox.Show("Service unavailable!");
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

                this.BeginInvoke((MethodInvoker)delegate
                {
                    //acesso a componentes visuais do form devem ser colocados aqui
                    richTextBoxMsgRcvd.AppendText($"{e.Topic}:{Encoding.UTF8.GetString(e.Message)}\n");
                });

            //if e.topic == spots
            //mete para a 



        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serviceEnabled)
            {
                Service_Disconect();
            }
        }

        public static int Insert(string query, List<SqlParameter> sqlParameters)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(CONNECTIONSTR);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                cmd.Parameters.AddRange(sqlParameters.ToArray());

                int affectedRows = cmd.ExecuteNonQuery();
                conn.Close();

                return affectedRows;
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return 0;
            }
        }



        public static int InsertIntoPark(Park park)
        {
            return Insert(
                "INSERT INTO Park VALUES (@Id, @Description, @Number_spots, @Operating_Hours, @Special_Spots)",
                new List<SqlParameter>
                {
                    new SqlParameter("@Id", park.Id),
                    new SqlParameter("@Description", park.Description),
                    new SqlParameter("@Number_spots", park.Number_Spots),
                    new SqlParameter("@Operating_Hours", park.Operating_Hours),
                    new SqlParameter("@Special_Spots", park.Special_Spots)
                });
        }

        public static int InsertIntoSpot(Spot spot)
        {
            return Insert(
                "INSERT INTO Spot VALUES ( @Id, @Location, @Status, @Time_Status, @Status_Battery, @Park_Id) ",
                new List<SqlParameter>
                {
                    new SqlParameter("@Id", spot.Id),
                    new SqlParameter("@Location", spot.Location),
                    new SqlParameter("@Status", spot.Status),
                    new SqlParameter("@Time_Status", spot.Time_Status),
                    new SqlParameter("@Status_Battery", spot.Status_Battery),
                    new SqlParameter("@Park_Id", spot.Park_Id)
                });
        }
    }
}
