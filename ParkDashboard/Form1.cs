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

        /*private void buttonParks_Click(object sender, EventArgs e)
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
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("1. List of available parks in the platform;");
            comboBox1.Items.Add("2. Status of all parking spots in a specific park for a given moment;");
            comboBox1.Items.Add("3. List of status of all parking spots in a specific park for a given time period;");
            comboBox1.Items.Add("4. List of free parking spots from a specific park for a given moment;");
            comboBox1.Items.Add("5. List of parking spots belonging to a specific park;");
            comboBox1.Items.Add("6. Detailed information about a specific park;");
            comboBox1.Items.Add("7. Detailed information about a specific parking spot in a given moment;");
            comboBox1.Items.Add("8. List of parking spots sensors that need to be replaced;");
            comboBox1.Items.Add("9. List of parking spots sensors that need to be replaced for a specific park;");
            comboBox1.Items.Add("10. Instant occupancy rate in a specific park.");
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //1. List of available parks in the platform;
                aux_Park();

                foreach (Park item in parks)
                {
                    richTextBox.AppendText(showPark(item));

                }
            }

            if (comboBox1.SelectedIndex == 1)
            {
                //2. Status of all parking spots in a specific park for a given moment;
                //button1.Enabled = false;
            }
            if (comboBox1.SelectedIndex == 4)
            {
                //5. List of parking spots belonging to a specific park;
                if (string.IsNullOrEmpty(textBoxParkID.Text))
                {
                    MessageBox.Show("Precisa de indicar um Park ID");
                }
                else
                {
                    aux_Spot();

                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    foreach (Spot item in spots)
                    {
                        string aux;
                        aux = showListSpots_SpecificPark(item, ID);
                        if (aux != ID.ToString())
                        {
                            richTextBox.AppendText(aux);
                        }
                    }
                }
            }

            if (comboBox1.SelectedIndex == 5)
            {
                //6. Detailed information about a specific park;
                if (string.IsNullOrEmpty(textBoxParkID.Text))
                {
                    MessageBox.Show("Precisa de indicar um Park ID");
                }
                else
                {
                    aux_Park();

                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    foreach (Park item in parks)
                    {
                        string aux;
                        aux = showSpecificPark(item, ID);
                        if (aux != ID.ToString())
                        {
                            richTextBox.AppendText(aux);
                        }
                    }
                }
            }

            if(comboBox1.SelectedIndex == 7)
            {
                //8. List of parking spots sensors that need to be replaced;
                aux_Spot();

                foreach (Spot item in spots)
                {
                    string aux;
                    aux = showListSpotsSensorsNeedReplaced(item);
                    if (aux != null)
                    {
                        richTextBox.AppendText(aux);
                    }
                }
            }

            if (comboBox1.SelectedIndex == 8)
            {
                //9. List of parking spots sensors that need to be replaced for a specific park;
                if (string.IsNullOrEmpty(textBoxParkID.Text))
                {
                    MessageBox.Show("Precisa de indicar um Park ID");
                }
                else
                {
                    aux_Spot();

                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    foreach (Spot item in spots)
                    {
                        string aux;
                        aux = showListSpotsSensorsNeedReplaced_SpecificPark(item, ID);
                        if (aux != null)
                        {
                            richTextBox.AppendText(aux);
                        }
                    }
                }
            }

            if (comboBox1.SelectedIndex == 9)
            {
                //10. Instant occupancy rate in a specific park.
                if (string.IsNullOrEmpty(textBoxParkID.Text))
                {
                    MessageBox.Show("Precisa de indicar um Park ID");
                }
                else
                {
                    aux_Spot();
                    aux_Park();

                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    int aux = 1; //se depois do foreach continuar a 1 quer dizer que o Park ID não existe
                    foreach (Park p in parks)
                    {
                        if (ID == p.Id)
                        {
                            aux = 0;
                        }
                    }

                    if (aux == 0)
                    {
                        string occ = "occupied", emp = "empty";
                        float i = 0, total = 0, occupacyRate = 0;
                        foreach (Spot item in spots)
                        {
                            if (ID == item.Park_Id)
                            {
                                if (string.Compare(item.Status, occ) == 0)
                                {
                                    i++;
                                }
                                if (string.Compare(item.Status, occ) == 0 || string.Compare(item.Status, emp) == 0)
                                {
                                    total++;
                                }
                            }
                        }
                        //VERIFICAR OS PARKS QUE EXISTEM E NÃO TEM SPOTS (dá uma conta toda estranha)
                        occupacyRate = i / total * 100;
                        richTextBox.AppendText(occupacyRate.ToString());
                        richTextBox.AppendText("%");
                    }
                    else
                    {
                        MessageBox.Show("Esse Park ID não existe! Insira outro");
                    }
                }
            }
        }

        /*private int verificarPark(Park p, int ID)
        {
            if (ID == p.Id)
            {
                return 0;
            }
        }*/

        private void aux_Park()
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
                //converter json to list<spots>
                parks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Park>>(jsonResponse);
            }
            else
            {
                MessageBox.Show("erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
            }

            richTextBox.Clear();
        }

        private void aux_Spot()
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

            richTextBox.Clear();
        }

        /*private int showOccupancyRate_SpecificPark(Spot p, int ID)
        {
            if (ID == p.Park_Id)
            {
                return ID;
            }
            return 0;
        }

        private int showOccupancyRate_SpecificPark(Spot p, int ID, int aux, int total, int occupacyRate)
        {
            string occ = "occupied", emp = "empty";
            if (ID == p.Park_Id)
            {
                if (string.Compare(p.Status,occ) == 0)
                {
                    aux++;
                    //MessageBox.Show(aux.ToString());
                }
                if (string.Compare(p.Status, occ) == 0 || string.Compare(p.Status, emp) == 0)
                {
                    total++;
                    //MessageBox.Show(total.ToString());
                }
            }
            MessageBox.Show(aux.ToString());
            occupacyRate = aux / total * 100;
            //MessageBox.Show(occupacyRate.ToString());
            return occupacyRate;
        }*/

        private string showListSpotsSensorsNeedReplaced_SpecificPark(Spot p, int ID)
        {
            if (ID == p.Park_Id)
            {
                Boolean aux = true;
                if (p.Status_Battery == aux)
                {
                    return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
                }
            }
            return null;
        }

        private string showListSpotsSensorsNeedReplaced(Spot p)
        {
            Boolean aux = true;
            if (p.Status_Battery == aux)
            {
                return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
            }
            return null;
        }

        private string showSpecificPark(Park p, int ID)
        {
            if (ID == p.Id)
            {
                return string.Format("{0} : {1} {2} {3} {4} \n", p.Id.ToString(), p.Description, p.Number_Spots.ToString(), p.Operating_Hours.ToString(), p.Special_Spots.ToString());
            }
            return ID.ToString();
        }

        private string showPark(Park p)
        {
           return string.Format("{0} : {1} {2} {3} {4} \n", p.Id.ToString(), p.Description, p.Number_Spots.ToString(), p.Operating_Hours.ToString(), p.Special_Spots.ToString());
        }

        private string showListSpots_SpecificPark(Spot p, int ID)
        {
            if (ID == p.Park_Id)
            {
                return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
            }
            return ID.ToString();
        }

        /*private string showSpot(Spot p)
        {
            return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
        }*/
    }
}
