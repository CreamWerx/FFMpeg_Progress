using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProgressLabel
{
    public partial class Progress: UserControl
    {
        private int valueProgress = 0;

        public int Value 
        { 
            get => valueProgress;
            set { valueProgress = value; progressLabel.Width = value.PercentOf(this.Width); Debug.WriteLine("width " + progressLabel.Width); } 
        }
        public Progress()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            //Debug.WriteLine(Width);
            progressLabel.Height = Height;
            //Debug.WriteLine(progressLabel.Height);
        }
    }

    public static class Extrnsions
    {
        public static int PercentOf(this int percent, int of)
        {
            return (percent * of) / 100;
        }
    }
}
