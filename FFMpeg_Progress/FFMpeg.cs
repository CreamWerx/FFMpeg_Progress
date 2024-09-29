using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FFMpeg_Progress
{
    public static class FFMpeg
    {
        public static event EventHandler<(int, int)> ExtractionInProgress;
        public static int BookmarkCount { get; private set; } = 0;
        //public static int BookmarkComplete { get; private set; } = 0;

        public static void JoinSegments(ListBox.ObjectCollection bookmarks, string outputfile)
        {
            Debug.WriteLine("JoinSegments  -  " + outputfile);
            var bml = new List<BookMark>();
            foreach (var item in bookmarks)
            {
                bml.Add((BookMark)item);
            }

            string Folder = Path.GetDirectoryName(bml[0].FilePath) + "\\";
            string filetext = string.Empty;
            foreach (var item in bml)
            {
                filetext += "file '" + item.FilePath + "'" + Environment.NewLine;
            }

            string temptextfile = Folder + "temp.txt";
            File.WriteAllText(temptextfile, filetext);

            string ffparams = $"-f concat -safe 0 -i \"{temptextfile}\" -c copy \"{outputfile}\"";
            //Debug.WriteLine(ffparams);

            ProcessStartInfo psInfo = PSInfo(ffparams);
            {
                //Debug.WriteLine(psInfo.Arguments);
                using (Process exe = Process.Start(psInfo))
                {
                    exe.WaitForExit();
                    //Thread.Sleep(500);
                }
                File.Delete(temptextfile);
            }

        }
        public static void ExtractSegments(ICollection bookmarks, string inputfile, IProgress<int> progress)
        {
            BookmarkCount = bookmarks.Count;
            //lblprogress[2].Invoke(new Action(() => lblprogress[2].Text = $"0 of {BookmarkCount}"));
            progress.Report(0);
            // TODO: Iterator below throws exception when playing the same bookmark that is being transcoded.
            string inputfileFolder = Path.GetDirectoryName(inputfile);
            inputfileFolder = Path.Combine(inputfileFolder, "clips");
            if (!Directory.Exists(inputfileFolder))
            {
                Directory.CreateDirectory(inputfileFolder);
            }
            var n = 1;
            foreach (var item in bookmarks)
            {

                //Debug.WriteLine("getting item number " + n.ToString() + " of " + bookmarks.Count.ToString() + Environment.NewLine
                //    + "Start: " + ((BookMark)item).Begin + Environment.NewLine
                //    + "End: " + ((BookMark)item).End + Environment.NewLine);
                BookMark bm = (BookMark)item;
                string outputFile = $@"{inputfileFolder}\{bm.Begin}.mp4";
                //ExtractionInProgress.Invoke(null, (n, BookmarkCount));
                ExtractSegment(bm, inputfile, outputFile, progress);
                n++;
                if (n > BookmarkCount)
                {
                    n = 0;
                    //ExtractionInProgress?.Invoke(null, (0, 0));
                    //lblprogress[0].Invoke(new Action(() => lblprogress[0].BackColor = SystemColors.InactiveCaption));
                    break;
                }

            }
        }

        public static void ExtractSegment(BookMark bookMark, string inputfile, string outputfile, IProgress<int> progress)
        {
            if (File.Exists(outputfile))
            {
                DialogResult overwrite = MessageBox.Show("File already exists. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (overwrite != DialogResult.Yes)
                {
                    //lblprogress[0].Invoke(new Action(() => lblprogress[0].BackColor = SystemColors.InactiveCaption));
                    return;
                }
                File.Delete(outputfile);
            }

            //string inputfileSorted = "\"" + inputfile + "\""; // The path to the file wrapped in quotes in case of spaces.
            //string i = "-i " + inputfileSorted; // Switch to indicate this is the input file.
            //string ss = " -ss " + Math.Round(bookMark.Begin, 3).ToString(); // The start time of the segment to extract.
            //string t = " -t " + Math.Round(bookMark.Duration, 3).ToString(); // The duration of time required to extract.
            //string c = " -vcodec h264 -acodec copy " + "\"" + outputfile + "\""; // Codec to use and output file path.

            //string ffparams = i + ss + t + c;

            string inputfileSorted = "\"" + inputfile + "\""; // The path to the file wrapped in quotes in case of spaces.
            string i = "-i " + inputfileSorted; // Switch to indicate this is the input file.
            string ss = " -ss " + Math.Round(bookMark.Begin, 3).ToString(); // The start time of the segment to extract.
            string t = " -t " + Math.Round(bookMark.Duration, 3).ToString(); // The duration of time required to extract.
            string c = " -vcodec h264 -acodec copy " + "\"" + outputfile + "\""; // Codec to use and output file path.

            string ffparams = i + ss + t + c;
            //Debug.WriteLine(" Args: " + ffparams);


            ProcessStartInfo psInfo = new ProcessStartInfo();
            {
                psInfo.FileName = "ffmpeg.exe";
                psInfo.UseShellExecute = false;
                psInfo.CreateNoWindow = true;
                psInfo.WindowStyle = ProcessWindowStyle.Hidden;
                psInfo.Arguments = ffparams;
                psInfo.RedirectStandardOutput = true;
                psInfo.RedirectStandardError = true;

                var exe = new Process();
                exe.StartInfo = psInfo;
                exe.EnableRaisingEvents = true;
                exe.Start();
                //lblProgressBase.BackColor = Color.Yellow;
                //lblprogress[0].Invoke(new Action(() => lblprogress[0].BackColor = Color.Orange));
                string processOutput = null;
                double duration = bookMark.Duration.Truncate(2);
                //BookmarkComplete++;
                //lblprogress[2].Invoke(new Action(() => lblprogress[2].Text = $"{BookmarkComplete} of {BookmarkCount}"));
                while ((processOutput = exe.StandardError.ReadLine()) != null)
                {
                    //Debug.WriteLine(processOutput);
                    if (processOutput.StartsWith("frame"))
                    {
                        int percent = FFMpegDataStringToPercent(processOutput, duration);
                        progress.Report(percent);
                        //lblprogress[1].Invoke(new Action(() => lblprogress[1].Progress(percent)));
                        Debug.WriteLine("percent " + percent.ToString());
                    }

                }
                //progress.Report(0);
                //lblprogress[1].Invoke(new Action(() => lblprogress[1].Progress(0)));
                //lblprogress[0].Invoke(new Action(() => lblprogress[0].BackColor = SystemColors.InactiveCaption));

            }

        }

        /// <summary>
        /// Parses relevant lines of FFMpeg stderrorout, to determine the
        ///  progress of a file transcode as a percentage.
        /// A typical data string would be "frame=62 fps=18 q=28.0 size=0kB time=00:00:02.86 bitrate=0.1kbits/s dup=1 drop=0 speed=0.854x"
        /// </summary>
        /// <param name="datastring"></param>
        /// <param name="duration">The duration of the conversion as double in seconds</param>
        /// <returns>int beween 0 and 100</returns>
        public static int FFMpegDataStringToPercent(string datastring, double duration)
        {
            try
            {
                datastring = datastring.Substring(datastring.IndexOf("time=") + 5, 12);
                char[] delim = { ':', '.' };
                string[] split = datastring.Split(delim);

                double secs = Convert.ToDouble(split[2] + "." + split[3]);
                double minutes = Convert.ToDouble(split[1]);
                double hours = Convert.ToDouble(split[0]);

                double time = secs + (minutes * 60) + (hours * 60 * 60);
                double percentElapsed = ((time / duration) * 100);

                return Convert.ToInt32(Math.Round(percentElapsed));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //private async Task<bool> AnalyzeAsync()
        //{
        //    //string ffProbePath = @"C:\Users\suzyc\Downloads\ffmpeg\ffmpeg-4.2.2-win64-static\bin\ffprobe.exe";

        //    keyframeList.Clear();
        //    ProcessStartInfo psInfo = new ProcessStartInfo();
        //    {
        //        psInfo.FileName = "ffprobe.exe";
        //        psInfo.UseShellExecute = false;
        //        psInfo.CreateNoWindow = true;
        //        psInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        psInfo.Arguments = "-loglevel error -skip_frame nokey -select_streams v:0 -show_entries frame=pkt_pts_time -of csv=print_section=0 " + "\"" + FileName + "\"";
        //        psInfo.RedirectStandardOutput = true;

        //        //psInfo.RedirectStandardError = true;
        //    }

        //    {
        //        using (Process exe = new Process())
        //        {
        //            exe.StartInfo = psInfo;
        //            exe.ErrorDataReceived += new DataReceivedEventHandler(errorHandler);
        //            //exe.ErrorDataReceived += new DataReceivedEventHandler(DataHandler);
        //            exe.Start();
        //            // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput?view=netcore-3.1
        //            string raw = exe.StandardOutput.ReadToEnd();
        //            exe.WaitForExit();
        //            //Console.Beep();
        //            //while (!exe.HasExited)
        //            //{
        //            //    Thread.Sleep(200);
        //            //}

        //            //textBoxInfo.Text = raw;
        //            var a = raw.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        //            textBoxInfo.Text = raw;
        //            a.RemoveAt(a.Count - 1);
        //            foreach (var item in a)
        //            {
        //                //textBoxInfo.AppendText(item);
        //                keyframeList.Add(double.Parse(item));
        //                Debug.WriteLine(item);

        //                //textBoxInfo.AppendText(Environment.NewLine);
        //            }
        //        }

        //    }
        //    return true;
        //}

        static ProcessStartInfo PSInfo(string ffparams)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            {
                psInfo.FileName = "ffmpeg.exe";
                psInfo.UseShellExecute = false;
                psInfo.CreateNoWindow = true;
                psInfo.WindowStyle = ProcessWindowStyle.Hidden;
                psInfo.Arguments = ffparams;
                psInfo.RedirectStandardOutput = true;


            }

            return psInfo;
        }
    }
}

