using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Popup
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public Guid HtmlId { get; set; }
    }
}
