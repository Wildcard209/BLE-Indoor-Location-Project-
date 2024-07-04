using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Beacon
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? MacAddress { get; set; }
        public Guid UUID { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int RSSI1M { get; set; }
        public int RSSI { get; set; }
        public float LocationX {  get; set; }
        public float LocationY { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
