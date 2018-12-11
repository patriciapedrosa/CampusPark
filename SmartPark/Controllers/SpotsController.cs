using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SmartPark.Models;
using System.Globalization;

namespace SmartPark.Controllers
{
    public class SpotsController : ApiController
    {
        protected static string CONNECTIONSTR =
            "Server=f0bd6467-8d2c-4782-aee0-a9a501091e04.sqlserver.sequelizer.com;Database=dbf0bd64678d2c4782aee0a9a501091e04;User ID=spjoenncymdyiakz;Password=J6ZRZ4Ex46AYiijuagUPuW7jPTnZxYVZFLYkDkAXe8MneQ6YtV7moRJU7PbgQNae;";

        // route: api/spots/
        [Route("api/spots/")]
        public IHttpActionResult Get()
        {
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Spot", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (string)reader["Id"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],  
                        Park_Id = (int)reader["Park_Id"]
                    };
                    spots.Add(p);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return NotFound();
            }

            return Ok(spots);
        }

        //api/spots/getStatusSpotsSpecificParkGivenMoment/1/04-12-2018 21_30_00

        //route: api/spots/getStatusSpotsSpecificParkGivenMoment/{Park_Id}
        [Route("api/spots/getStatusSpotsSpecificParkGivenMoment/{Park_Id}/{Time_Status}")]
        public IHttpActionResult GetStatusSpots_SpecificPark_GivenMoment(int Park_Id, string data)
        {
            //2. Status of all parking spots in a specific park for a given moment; 
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            DateTime data1 = Convert.ToDateTime(data);
            CultureInfo provider = CultureInfo.InvariantCulture;
            string format, str;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Spot Where Park_Id = '" + Park_Id + "' AND Time_Status(datavalue) ='" + data1.Year + '/' + data1.Month + '/' + data1.Day + ' ' + data1.Hour + ':' + data1.Minute + ':' + data1.Second + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                str = reader.GetString(0);
                format = "dd-MM-yyyy hh_mm_ss";
                data1 = DateTime.ParseExact(str, format, provider);

                try
                {
                    data1 = DateTime.ParseExact(str, format, provider);
                    Console.WriteLine("{0} converts to {1}.", str, data1.ToString());
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not in the correct format.", str);
                }

                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (string)reader["Id"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    spots.Add(p);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return NotFound();
            }

            return Ok(spots);
        }

        //route: api/spots/getParkId/{Park_Id}
        [Route("api/spots/getParkId/{Park_Id:int}")]
        public IHttpActionResult GetSpotsBelonging_SpecificPark(int Park_Id)
        {
            //5. List of parking spots belonging to a specific park; 
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Spot Where Park_Id = '" + Park_Id + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (string)reader["Id"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }
                reader.Close();
                conn.Close();

                if (cont == 0)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return NotFound();
            }

            return Ok(spots);
        }

        //route: api/spots/sensorsToBeReplaced
        [Route("api/spots/sensorsToBeReplaced")]
        public IHttpActionResult GetSpotsSensorNeedToBeReplaced()
        {
            //8.List of parking spots sensors that need to be replaced because of its critical battery level, within the overall platform;
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Spot Where Status_Battery = '" + true + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (string)reader["Id"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    spots.Add(p);
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return NotFound();
            }

            return Ok(spots);
        }

        //route: api/spots/sensorsToBeReplaced/{Park_Id}
        [Route("api/spots/sensorsToBeReplaced/{Park_Id:int}")]
        public IHttpActionResult GetSpotsNeedToBeReplaced_SpecificPark(int Park_Id)
        {
            //9. List of parking spots sensors that need to be replaced for a specific park;
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Spot Where Park_Id = '" + Park_Id + "' AND Status_Battery = '" + true + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (string)reader["Id"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }
                reader.Close();
                conn.Close();

                if (cont == 0)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }

            return Ok(spots);
        }

        //route: api/spots/occupancyRate/{Park_Id}
        [Route("api/spots/occupancyRate/{Park_Id:int}")]
        public IHttpActionResult GetOccupancyRate_SpecificPark(int Park_Id)
        {
            //10. Instant occupancy rate in a specific park.
            float occupancyRate = 0;
            string str = null;
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                float spotsTotal = 0, spotsOccupied = 0;
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Spot Where Park_Id = '" + Park_Id + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {       
                    spotsTotal = reader.GetInt32(0);
                }
                reader.Close();

                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Spot Where Park_Id = '" + Park_Id + "' AND Spot.Status = 'occupied' ", conn);
                SqlDataReader reader2 = cmd2.ExecuteReader();

                while (reader2.Read())
                {
                    spotsOccupied = reader2.GetInt32(0);
                }
                if (spotsTotal == 0)
                {
                    return NotFound();
                }
                else
                {
                    
                    occupancyRate = spotsOccupied / spotsTotal * 100;
                    str = "A taxa de ocupação do parque " + Park_Id + " é de " + occupancyRate.ToString() + "%";
                    reader2.Close();
                    conn.Close();
                }
            }
            catch (Exception) //usar "ex" dentro do Exception se quiser usar o return Ok de baixo para testar
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                return NotFound();
                //return Ok(ex.ToString()); <- usar isto quando quiser verificar o que está a correr mal
            }

            return Ok(str);
        }
    }
}
