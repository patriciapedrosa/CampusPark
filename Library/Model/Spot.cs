using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Spot
    {
        public string Id { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime Time_Status { get; set; }
        public Boolean Status_Battery { get; set; }
        public int Park_Id { get; set; }
    }
}
