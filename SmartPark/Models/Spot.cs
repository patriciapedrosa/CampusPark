using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Spot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime Time_Status { get; set; }
        public Boolean Status_Battery { get; set; }
        public int Park_Id { get; set; }
    }
}