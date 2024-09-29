using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFMpeg_Progress
{
    public class BookMark
    {
        public BookMark()
        {
        }

        public BookMark(string name)
        {
            // removed the assignment  _ =
            double.TryParse(name, out double Begin);
        }

        public double Begin { get; set; }
        public double End { get; set; }
        public double Duration { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Begin.ToString();
        }
    }
}
