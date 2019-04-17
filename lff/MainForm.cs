using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace lff
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            timer1.Start();
            this.timer1.Interval = 10000;

            MainLabel.Text = "Connecting...";
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Thread th = new Thread(() =>
            {
                try
                {

                    HtmlWeb html = new HtmlWeb()
                    {
                        PreRequest = request =>
                        {
                            // Make any changes to the request object that will be used.
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            return true;
                        }
                    };
                    HtmlDocument doc = html.Load(Constants.Path);


                    var items = doc.DocumentNode.SelectSingleNode("//td[@class='chartlist-name']//span");

                    var info = items.InnerText.Split(new[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    if (this.InvokeRequired)
                        this.Invoke(new Action(() => { MainLabel.Text = PrepareTrackName(info); }));
                    else MainLabel.Text = PrepareTrackName(info);

                }
                catch (WebException exce)
                {
                    MainLabel.Text = "Connection issues";
                }
                catch (Exception ec)
                {
                    MainLabel.Text = ec.Message;
                }
            })
            { IsBackground = true };

            th.Start();
        }

        private string PrepareTrackName(string[] info)
        {
            return Regex.Unescape(string.Join(" ", info));
        }

    }

    static class Constants
    {
        public static string Path = "http://last.fm/user/kypiwindy";
    }
}
