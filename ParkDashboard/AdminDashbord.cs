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
    public partial class AdminDashbord : Form
    {

        string baseURI = @"http://localhost:63497/"; //needs to be updated!
        List<Park> parks = new List<Park>();
        List<Spot> spots = new List<Spot>();
        HttpClient client;

        public AdminDashbord()
        {
            InitializeComponent();
        }

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 7)
            {
                textBoxParkID.Enabled = false;
                textBoxSpotID.Enabled = false;
                textBoxGivenMoment.Enabled = false;
                textBoxStartDate.Enabled = false;
                textBoxEndDate.Enabled = false;
            }

            if (comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 3 )
            {
                textBoxParkID.Enabled = true;
                textBoxSpotID.Enabled = false;
                textBoxGivenMoment.Enabled = true;
                textBoxStartDate.Enabled = false;
                textBoxEndDate.Enabled = false;
            }

            if (comboBox1.SelectedIndex == 4 || comboBox1.SelectedIndex == 5 || comboBox1.SelectedIndex == 8 || comboBox1.SelectedIndex == 9)
            {
                textBoxParkID.Enabled = true;
                textBoxSpotID.Enabled = false;
                textBoxGivenMoment.Enabled = false;
                textBoxStartDate.Enabled = false;
                textBoxEndDate.Enabled = false;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                textBoxParkID.Enabled = true;
                textBoxSpotID.Enabled = false;
                textBoxGivenMoment.Enabled = false;
                textBoxStartDate.Enabled = true;
                textBoxEndDate.Enabled = true;
            }

            if (comboBox1.SelectedIndex == 6)
            {
                textBoxParkID.Enabled = false;
                textBoxSpotID.Enabled = true;
                textBoxGivenMoment.Enabled = true;
                textBoxStartDate.Enabled = false;
                textBoxEndDate.Enabled = false;
            }

        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != 0 && comboBox1.SelectedIndex != 1 && comboBox1.SelectedIndex != 2 && comboBox1.SelectedIndex != 3 && comboBox1.SelectedIndex != 4 && comboBox1.SelectedIndex != 5 && comboBox1.SelectedIndex != 6 && comboBox1.SelectedIndex != 7 && comboBox1.SelectedIndex != 8 && comboBox1.SelectedIndex != 9)
            {
                MessageBox.Show("Opção inválida! Escolha uma da lista.");
            }

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
                if (string.IsNullOrEmpty(textBoxParkID.Text) || string.IsNullOrEmpty(textBoxGivenMoment.Text))
                {
                    if (string.IsNullOrEmpty(textBoxParkID.Text))
                    {
                        MessageBox.Show("Precisa de indicar um Park ID");
                    }

                    else
                    {
                        MessageBox.Show("Precisa de indicar uma hora. Formato: hh:mm");
                    }
                }
                else
                {
                    //if(string.Format(textBoxGivenMoment.Text) != %h)

                    
                }
            }

            if (comboBox1.SelectedIndex == 2)
            {
                //3.List of status of all parking spots in a specific park for a given time period;
            }

            if (comboBox1.SelectedIndex == 3)
            {
                //4. List of free parking spots from a specific park for a given moment;
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
                    aux_Park();

                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    int i = 1; //se depois do foreach continuar a 1 quer dizer que o Park ID não existe
                    foreach (Park p in parks)
                    {
                        i = verificarParkID(p, ID, i);
                    }

                    if (i == 0)
                    {
                        client = new HttpClient();
                        client.BaseAddress = new Uri(baseURI);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync($"api/spots/getParkId/" + textBoxParkID.Text).Result;
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            //converter json to list<spots>
                            spots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Spot>>(jsonResponse);
                        }
                        else
                        {
                            MessageBox.Show("Erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
                        }

                        richTextBox.Clear();
                        foreach (Spot item in spots)
                        {
                            richTextBox.AppendText(showSpot(item));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esse Park ID não existe! Insira outro");
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
                    int i = 1; //se depois do foreach continuar a 1 quer dizer que o Park ID não existe
                    foreach (Park p in parks)
                    {
                        i = verificarParkID(p, ID, i);
                    }

                    if (i == 0)
                    {
                        client = new HttpClient();
                        client.BaseAddress = new Uri(baseURI);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync($"api/parks/" + textBoxParkID.Text).Result;
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            //converter json to list<parks>
                            parks = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Park>>(jsonResponse);
                        }
                        else
                        {
                            MessageBox.Show("Erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
                        }

                        richTextBox.Clear();
                        foreach (Park item in parks)
                        {
                            richTextBox.AppendText(showPark(item));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esse Park ID não existe! Insira outro");
                    }
                }
            }

            if (comboBox1.SelectedIndex == 6)
            {
                //7. Detailed information about a specific parking spot in a given moment;
            }

            if (comboBox1.SelectedIndex == 7)
            {
                //8. List of parking spots sensors that need to be replaced;
                client = new HttpClient();
                client.BaseAddress = new Uri(baseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"api/spots/sensorsToBeReplaced").Result;
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    //converter json to list<spots>
                    spots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Spot>>(jsonResponse);
                }
                else
                {
                    MessageBox.Show("Erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
                }

                richTextBox.Clear();
                foreach (Spot item in spots)
                {
                    richTextBox.AppendText(showSpot(item));
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
                    aux_Park();
                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    int aux = 1; //se depois do foreach continuar a 1 quer dizer que o Park ID não existe
                    foreach (Park p in parks)
                    {
                        aux = verificarParkID(p, ID, aux);
                    }
                    if (aux == 0)
                    {
                        client = new HttpClient();
                        client.BaseAddress = new Uri(baseURI);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync($"api/spots/sensorsToBeReplaced/" + textBoxParkID.Text).Result;
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            //converter json to list<spots>
                            spots = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Spot>>(jsonResponse);
                        }
                        else
                        {
                            MessageBox.Show("Erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
                        }

                        richTextBox.Clear();
                        foreach (Spot item in spots)
                        {
                            richTextBox.AppendText(showSpot(item));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Esse Park ID não existe! Insira outro");
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
                    aux_Park();
                    string str = null;
                    int ID = Convert.ToInt32(textBoxParkID.Text);
                    int aux = 1; //se depois do foreach continuar a 1 quer dizer que o Park ID não existe
                    foreach (Park p in parks)
                    {
                        aux = verificarParkID(p, ID, aux);
                    }

                    if (aux == 0)
                    {
                        client = new HttpClient();
                        client.BaseAddress = new Uri(baseURI);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync($"api/spots/occupancyRate/" + textBoxParkID.Text).Result;
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = response.Content.ReadAsStringAsync().Result;
                            //converter json to String;
                            str = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(jsonResponse);
                        }
                        else
                        {
                            MessageBox.Show("Erro a chamar a api" + response.StatusCode + response.ReasonPhrase);
                        }

                        richTextBox.Clear();
                        richTextBox.AppendText(str);
                    }
                    else
                    {
                        MessageBox.Show("Esse Park ID não existe! Insira outro");
                    }
                }
            }
        }

        private int verificarParkID(Park p, int ID, int aux)
        {

            if (ID == p.Id)
            {
                aux = 0;
            }
            return aux;
        }

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

        private string showSpecificPark(Park p, int ID)
        {
            if (ID == p.Id)
            {
                return string.Format("{0} : {1} {2} {3} {4} \n", p.Id.ToString(), p.Description, p.Number_Spots.ToString(), p.Operating_Hours.ToString(), p.Special_Spots.ToString());
            }
            return null;
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
            return null;
        }

        private void textBoxParkID_KeyPress(object sender, KeyPressEventArgs e)
        {
            int asc = e.KeyChar;

            if (!char.IsDigit(e.KeyChar) && asc != 08 && asc != 127)

            {
                e.Handled = true;
                MessageBox.Show("Este campo aceita apenas caracteres numéricos.");
            }
        }

        private string showSpot(Spot p)
        {
            return string.Format("{0} : {1} {2} {3} {4} {5}\n", p.Id, p.Location, p.Status, p.Time_Status.ToString(), p.Status_Battery.ToString(), p.Park_Id.ToString());
        }
    }
}
