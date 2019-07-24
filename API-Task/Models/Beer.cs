using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITask.Models
{
    public class Beer
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Image_url { get; set; }
        public String Description { get; set; }
        public String First_brewed { get; set; }
        public String ABV { get; set; }
        public List<String> FoodPairing { get; set; }
        public Volume Volume { get; set; }
    }

    public class Volume
    {
        public Double Value { get; set; }
        public String Unit { get; set; }
    }
}
