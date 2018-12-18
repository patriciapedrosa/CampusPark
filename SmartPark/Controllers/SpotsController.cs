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

        //api/spots/
        [Route("api/spots/")]
        public IHttpActionResult Get()
        {
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            try 
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM(SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot) a WHERE rn = 1", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
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

        //api/spots/parks/1/date/2018-12-23T21:30:00
        [Route("api/spots/parks/{Park_Id:int}/date/{Time_Status:datetime:regex(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2})}")]
        public IHttpActionResult GetStatusSpots_SpecificPark_GivenMoment(int Park_Id, DateTime Time_Status)
        {
            //2. Status of all parking spots in a specific park for a given moment;
            //return Ok(Time_Status.ToString());
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id AND Time_Status <= @Time_Status) a WHERE rn = 1", conn); // uso o sqlconnection conn e uso aquele comando sql
                cmd.Parameters.AddWithValue("@Park_Id", Park_Id);
                cmd.Parameters.AddWithValue("@Time_Status", Time_Status);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    spots.Add(p);
                    cont++;
                }

                if (cont == 0)
                {
                    return NotFound();
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                //return NotFound();
                return Ok(ex.ToString());
            }

            return Ok(spots);
        }

        //api/spots/parks/1/startDate/2018-10-23T21:20:00/endDate/2018-12-04T22:00:00
        [Route("api/spots/parks/{Park_Id:int}/startDate/{startDate:datetime:regex(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2})}/endDate/{endDate:datetime:regex(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2})}")]
        public IHttpActionResult GetStatusSpots_SpecificPark_GivenPeriod(int Park_Id, DateTime startDate, DateTime endDate)
        {
            //3. List of status of all parking spots in a specific park for a given time period;
            if (endDate < startDate)
            {
                return Ok("ERRO! A segunda data deve ser superior à primeira");
            }
            else
            {
                List<Spot> spots = new List<Spot>();
                SqlConnection conn = new SqlConnection(CONNECTIONSTR);
                int cont = 0;

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM(SELECT Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id AND Time_Status between @startDate AND @endDate) a WHERE rn = 1 ", conn); // uso o sqlconnection conn e uso aquele comando sql
                    cmd.Parameters.AddWithValue("@Park_Id", Park_Id);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Spot p = new Spot
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Location = (string)reader["Location"],
                            Status = (string)reader["Status"],
                            Time_Status = (DateTime)reader["Time_Status"],
                            Status_Battery = (Boolean)reader["Status_Battery"],
                            Park_Id = (int)reader["Park_Id"]
                        };
                        spots.Add(p);
                        cont++;
                    }

                    if (cont == 0)
                    {
                        return NotFound();
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
        }

        //api/spots/parksFree/1/date/2018-12-23T21:30:00
        [Route("api/spots/parksFree/{Park_Id:int}/date/{Time_Status:datetime:regex(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2})}")]
        public IHttpActionResult GetStatusSpotsFree_SpecificPark_GivenMoment(int Park_Id, DateTime Time_Status)
        {
            //4.List of free parking spots from a specific park for a given moment;
            List <Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DROP TABLE Spot_aux;", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                reader.Close();
                SqlCommand cmd2 = new SqlCommand("SELECT * INTO Spot_aux FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id,  ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id AND Time_Status <= @Time_Status) a WHERE rn = 1; ", conn);
                cmd2.Parameters.AddWithValue("@Park_Id", Park_Id);
                cmd2.Parameters.AddWithValue("@Time_Status", Time_Status);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {

                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand("SELECT * FROM Spot_aux WHERE Status = 'empty';", conn);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader3["Id"],
                        Name = (string)reader3["Name"],
                        Location = (string)reader3["Location"],
                        Status = (string)reader3["Status"],
                        Time_Status = (DateTime)reader3["Time_Status"],
                        Status_Battery = (Boolean)reader3["Status_Battery"],
                        Park_Id = (int)reader3["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }
                reader3.Close();
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

        //api/spots/parks/1
        [Route("api/spots/parks/{Park_Id:int}")]
        public IHttpActionResult GetSpotsBelonging_SpecificPark(int Park_Id)
        {
            //5. List of parking spots belonging to a specific park; 
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id) a WHERE rn = 1", conn); // uso o sqlconnection conn e uso aquele comando sql
                cmd.Parameters.AddWithValue("@Park_Id", Park_Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }

                if (cont == 0)
                {
                    return NotFound();
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

        //api/spots/B-1/date/2018-12-23T21:30:00
        [Route("api/spots/{Name}/date/{Time_Status:datetime:regex(\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2})}")]
        public IHttpActionResult GetSpecificSpot_GivenMoment(string Name, DateTime Time_Status)
        {
            //7. Detailed information about a specific parking spot in a given moment (should also indicate if the spot is free or occupied);
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Name = @Name AND Time_Status <= @Time_Status) a WHERE rn = 1", conn); // uso o sqlconnection conn e uso aquele comando sql
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Time_Status", Time_Status);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Name"],
                        Location = (string)reader["Location"],
                        Status = (string)reader["Status"],
                        Time_Status = (DateTime)reader["Time_Status"],
                        Status_Battery = (Boolean)reader["Status_Battery"],
                        Park_Id = (int)reader["Park_Id"]
                    };
                    spots.Add(p);
                    cont++;
                }

                if (cont == 0)
                {
                    return NotFound();
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

        //api/spots/sensorsToBeReplaced
        [Route("api/spots/sensorsToBeReplaced")]
        public IHttpActionResult GetSpotsSensorNeedToBeReplaced()
        {
            //8.List of parking spots sensors that need to be replaced because of its critical battery level, within the overall platform;
            List<Spot> spots = new List<Spot>();
            SqlConnection conn = new SqlConnection(CONNECTIONSTR);
            int cont = 0;
            try
            {
                conn.Open();
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Spot Where Status_Battery = '" + true + "' ", conn); // uso o sqlconnection conn e uso aquele comando sql
                SqlCommand cmd = new SqlCommand("DROP TABLE Spot_aux;", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                reader.Close();
                SqlCommand cmd2 = new SqlCommand("SELECT * INTO Spot_aux FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id,  ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot) a WHERE rn = 1; ", conn);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {

                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand("SELECT * FROM Spot_aux WHERE Status_Battery = 'true';", conn);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader3["Id"],
                        Name = (string)reader3["Name"],
                        Location = (string)reader3["Location"],
                        Status = (string)reader3["Status"],
                        Time_Status = (DateTime)reader3["Time_Status"],
                        Status_Battery = (Boolean)reader3["Status_Battery"],
                        Park_Id = (int)reader3["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }
                reader3.Close();
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
                //return Ok(ex.ToString());
                return NotFound();
            }

            return Ok(spots);
        }

        //api/spots/sensorsToBeReplaced/1
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
                SqlCommand cmd = new SqlCommand("DROP TABLE Spot_aux;", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                }
                reader.Close();
                SqlCommand cmd2 = new SqlCommand("SELECT * INTO Spot_aux FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id,  ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id) a WHERE rn = 1; ", conn);
                cmd2.Parameters.AddWithValue("@Park_Id", Park_Id);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {

                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand("SELECT * FROM Spot_aux WHERE Status_Battery = 'true';", conn);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    Spot p = new Spot
                    {
                        Id = (int)reader3["Id"],
                        Name = (string)reader3["Name"],
                        Location = (string)reader3["Location"],
                        Status = (string)reader3["Status"],
                        Time_Status = (DateTime)reader3["Time_Status"],
                        Status_Battery = (Boolean)reader3["Status_Battery"],
                        Park_Id = (int)reader3["Park_Id"]
                    };
                    cont++;
                    spots.Add(p);
                }
                reader3.Close();
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

        //api/spots/occupancyRate/1
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
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id, ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id) a WHERE rn = 1", conn); // uso o sqlconnection conn e uso aquele comando sql
                cmd.Parameters.AddWithValue("@Park_Id", Park_Id);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {       
                    spotsTotal = reader.GetInt32(0);
                }
                reader.Close();

                SqlCommand cmd1 = new SqlCommand("DROP TABLE Spot_aux;", conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {

                }
                reader1.Close();
                SqlCommand cmd2 = new SqlCommand("SELECT * INTO Spot_aux FROM (SELECT  Id, Name, Location, Time_Status, Status, Status_Battery, Park_Id,  ROW_NUMBER() OVER(PARTITION BY Name ORDER BY Time_Status DESC, Id DESC) rn FROM Spot WHERE Park_Id = @Park_Id) a WHERE rn = 1; ", conn);
                cmd2.Parameters.AddWithValue("@Park_Id", Park_Id);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {

                }
                reader2.Close();
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Spot_aux WHERE Status = 'occupied';", conn);
                SqlDataReader reader3 = cmd3.ExecuteReader();
                while (reader3.Read())
                {
                    spotsOccupied = reader3.GetInt32(0);
                }
                if (spotsTotal == 0)
                {
                    return NotFound();
                }
                else
                {
                    occupancyRate = spotsOccupied / spotsTotal * 100;
                    str = "A taxa de ocupação do parque " + Park_Id + " é de " + occupancyRate.ToString("00.00") + "%";
                    reader3.Close();
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
