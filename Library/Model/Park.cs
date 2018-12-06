﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Park
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Number_Spots { get; set; }
        public DateTime Operating_Hours { get; set; }
        public int Special_Spots { get; set; }
    }
}
