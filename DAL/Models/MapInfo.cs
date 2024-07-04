using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MapInfo
    {
        public Guid ID { get; set; } = new Guid();
        public Guid? CurrentCssId { get; set; }
        public Guid? CurrentJsId { get; set; }
        public int? ImageHeight { get; set; }
        public int? ImageWidth { get; set; }
        public int? DefaultX { get; set; }
        public int? DefaultY { get; set; }
        public int? LowerX { get; set; }
        public int? LowerY { get; set; }
        public int? HigherX { get; set; }
        public int? HigherY { get; set; }
        public int? BoundX { get; set; }
        public int? BoundY { get; set; }

        public required Css? CurrentCss { get; set; }
        public required Javascript? CurrentJs { get; set; }
    }
}
