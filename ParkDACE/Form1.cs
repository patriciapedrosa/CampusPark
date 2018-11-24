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

namespace ParkDACE
{
    public partial class Form1 : Form
    {

        private BackgroundWorker bw = new BackgroundWorker();

        private ParkingSensorNodeDll.ParkingSensorNodeDll dll = null;

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

        private void button1_Click(object sender, EventArgs e)
        {
            
            dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();
            dll.Initialize(NewSensorValueFunction, 1000);
            bw.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceParkingSpot.ServiceParkingSpotClient servico = new ServiceParkingSpotClient();

            listBox1.DataSource = servico.GetSpots();
            listBox1.DisplayMember = "Name";
        }
    }
}
