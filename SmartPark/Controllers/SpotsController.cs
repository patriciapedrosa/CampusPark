using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using SmartPark.Models;

namespace SmartPark.Controllers
{
    public class SpotsController : ApiController
    {
        protected static string CONNECTIONSTR =
            "Server=f0bd6467-8d2c-4782-aee0-a9a501091e04.sqlserver.sequelizer.com;Database=dbf0bd64678d2c4782aee0a9a501091e04;User ID=spjoenncymdyiakz;Password=J6ZRZ4Ex46AYiijuagUPuW7jPTnZxYVZFLYkDkAXe8MneQ6YtV7moRJU7PbgQNae;";

        //[Route("api/spots")]
        public IEnumerable<Spot> Get()
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
            }

            return spots;
        }
    }
}
