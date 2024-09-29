using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFMpeg_Progress
{
    public partial class Form1 : Form
    {
        public List<Label> Labels { get; set; }
        public List<BookMark> BookMarks { get; set; }

        public IProgress<int> ConversionProgress;

        public Form1()
        {
            InitializeComponent();
        }

        public void ProgressChanged(int val)
        {
            progress1.Value = val;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           ConversionProgress = new Progress<int>(ProgressChanged);

            BookMarks = new List<BookMark>
            {
                new BookMark
                {
                    Begin = 0,
                    End = 50,
                    Duration = 50,
                    FilePath = @"C:\Test\copied.mp4"
                }
            };
        }

        private void FFMpeg_ItemComplete(object sender, (int, int) e)
        {
            string lableText = $"{e.Item1} of {e.Item2}";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await Task.Run(()=> FFMpeg.ExtractSegments(BookMarks, @"C:\Test\copied.mp4", ConversionProgress));
            progress1.Value = 0;
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
