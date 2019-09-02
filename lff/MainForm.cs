using System;
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
            if (MainLabel.InvokeRequired)
            {
                MainLabel.Invoke(new MethodInvoker(() => MainLabel.Text = text));
            }
            else
            {
                MainLabel.Text = text;
            }
        }

    }
}
