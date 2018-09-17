using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class URL
    {
        public string URLsSet { get; set; }
        public string[] URLs { get; set; }
        public string[] Titles { get; set; }
        public string[] ResStatusCodes { get; set; }
    }
}