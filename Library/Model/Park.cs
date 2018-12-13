using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library.Model
{
    public class Park
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public int Number_Spots { get; set; }
        public string Operating_Hours { get; set; }
        public int Special_Spots { get; set; }
    }

}
