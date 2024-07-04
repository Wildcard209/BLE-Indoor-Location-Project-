using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class MapPopups
    {
        public Guid PopupLocationId { get; set; }
        public Guid PopupId { get; set; }
        public PopupLocation PopupLocation { get; set; } = new PopupLocation();
        public Popup Popup { get; set; } = new Popup();
    }
}
