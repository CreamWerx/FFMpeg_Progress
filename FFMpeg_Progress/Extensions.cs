using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFMpeg_Progress
{
    public static class Extensions
    {
        public static void Progress(this Label lbl, int value)
        {
            lbl.Width = value * 2;
            lbl.Update();
        }

        public static double Truncate(this double val, int dp = 3)
        {
            return Math.Round(val, dp);
        }
    }
}
