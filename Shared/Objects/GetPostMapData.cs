using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Objects
{
    public class GetPostMapData
    {
        public List<PopupContent> PopupContentList { get; set; } = new List<PopupContent>();
        public string SelectedCss { get; set; } = "";
        public string SelectedJs { get; set; } = "";
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int DefaultX { get; set; }
        public int DefaultY { get; set; }
        public int LowerX { get; set; }
        public int LowerY { get; set; }
        public int HigherX { get; set; }
        public int HigherY { get; set; }
        public int BoundX { get; set; }
        public int BoundY { get; set; }
    }
}
