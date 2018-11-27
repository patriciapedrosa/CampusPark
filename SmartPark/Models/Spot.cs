using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Spot
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime Time_Status { get; set; }
        public Boolean Status_Baterry { get; set; }
        public int Park_Id { get; set; }
    }
}