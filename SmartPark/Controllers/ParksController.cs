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
    public class ParksController : ApiController
    {
        protected static string CONNECTIONSTR =
            "Server=f0bd6467-8d2c-4782-aee0-a9a501091e04.sqlserver.sequelizer.com;Database=dbf0bd64678d2c4782aee0a9a501091e04;User ID=spjoenncymdyiakz;Password=J6ZRZ4Ex46AYiijuagUPuW7jPTnZxYVZFLYkDkAXe8MneQ6YtV7moRJU7PbgQNae;";

        //route: api/parks/
        [Route("api/parks/")]
        public IEnumerable<Park> Get()
        {
            //1.List of available parks in the platform;
            List<Park> parks = new List<Park>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Park", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Park p = new Park
                    {
                        Id = (int)reader["Id"],
                        Description = (string)reader["Description"],
                        Number_Spots = (int)reader["Number_Spots"],
                        Operating_Hours = (DateTime)reader["Operating_Hours"],
                        Special_Spots = (int)reader["Special_Spots"]
                    };
                    parks.Add(p);
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

            return parks;
        }

        //route: api/parks/{Id}
        [Route("api/parks/{Id}")]
        public IEnumerable<Park> Get(int id)
        {
            //6. Detailed information about a specific park;
            List<Park> parks = new List<Park>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Park Where Id=@id", conn); // uso o sqlconnection conn e uso aquele comando sql
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Park p = new Park
                    {
                        Id = (int)reader["Id"],
                        Description = (string)reader["Description"],
                        Number_Spots = (int)reader["Number_Spots"],
                        Operating_Hours = (DateTime)reader["Operating_Hours"],
                        Special_Spots = (int)reader["Special_Spots"]
                    };
                    parks.Add(p);
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

            return parks;
        }
    }
}
