using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http; //httpclient
using SmartPark.Models;

namespace ParkDashboard
{
    public partial class Form1 : Form
    {

        string baseURI = @"http://localhost:63497/"; //needs to be updated!
        List<Park> parks = new List<Park>();
        List<Spot> spots = new List<Spot>();
        HttpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonParks_Click(object sender, EventArgs e)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync($"api/parks").Result;
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                //converter json to list<parks>
                parks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Park>>(jsonResponse);
            }
            else
            {
                MessageBox.Show("erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
            }

            richTextBoxParks.Clear();
            foreach (Park item in parks)
            {
                richTextBoxParks.AppendText(showPark(item));

            }

        }

        private string showPark(Park p)
        {
            return string.Format("{0} : {1} {2} {3} {4} \n", p.Id.ToString(), p.Description, p.Number_Spots.ToString(), p.Operating_Hours.ToString(), p.Special_Spots.ToString());
        }

        private void buttonSpots_Click(object sender, EventArgs e)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseURI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync($"api/spots").Result;
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                //converter json to list<spots>
                spots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Spot>>(jsonResponse);
            }
            else
            {
                MessageBox.Show("erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
            }

            richTextBoxSpots.Clear();
            foreach (Spot item in spots)
            {
                richTextBoxSpots.AppendText(showSpot(item));

            }
        }

        private string showSpot(Spot p)
        {
            return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
        }
    }
}
