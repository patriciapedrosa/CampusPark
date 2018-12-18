using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Park
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Number_Spots { get; set; }
        public string Operating_Hours { get; set; }
        public int Special_Spots { get; set; }
    }
}