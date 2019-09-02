using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace lff
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetStartLocation();
            timer1.Start();

            WriteInfo("Connecting...");

        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    var html = new HtmlWeb()
                    {
                        PreRequest = request =>
                        {
                            request.AutomaticDecompression =
                                DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            return true;
                        }
                    };
                    var doc = html.Load(Constants.Path);
                    var track = doc.DocumentNode.SelectSingleNode(@"//td[@class='chartlist-name']");
                    var artist = doc.DocumentNode.SelectSingleNode(@"//td[@class='chartlist-artist']");

                    WriteInfo(PrepareTrackName(new[] { track.InnerText, artist.InnerText }));

                }
                catch (WebException we)
                {
                    WriteInfo("Connection issues" + we.Message);
                }
                catch (Exception ex)
                {
                    WriteInfo(ex.Message + ex.Message);
                }

            });
        }

        private string PrepareTrackName(string[] info)
        {
            var track = info[0]?.Trim();
            var artist = info[1]?.Trim();
            if (track != null)
            {
                track = WebUtility.HtmlDecode(track);

            }
            if (artist != null)
            {
                artist = WebUtility.HtmlDecode(artist);
            }
            return $"{artist} - {track}";
        }

        private void WriteInfo(string text)
        {
            var tip = new ToolTip();
            if (MainLabel.InvokeRequired)
            {
                MainLabel.Invoke(new MethodInvoker(() =>
                {
                    MainLabel.Text = text;
                    tip.SetToolTip(MainLabel, MainLabel.Text);
                }));
            }
            else
            {
                MainLabel.Text = text;
                tip.SetToolTip(MainLabel, MainLabel.Text);
            }
        }

        private void SetStartLocation()
        {
            var workingArea = Screen.GetWorkingArea(this);
            Location = new Point(workingArea.Right - Size.Width,
                workingArea.Bottom - Size.Height);
        }

        private void MainLabel_Click(object sender, EventArgs e)
        {
            if (MainLabel.InvokeRequired)
            {
                MainLabel.Invoke(new MethodInvoker(() => Clipboard.SetText(MainLabel.Text)));
              
            }
            else
            {
                Clipboard.SetText(MainLabel.Text);
            }
        }
    }
}
