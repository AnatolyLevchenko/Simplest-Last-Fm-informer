using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lff
{
    public partial class LyricsForm : Form
    {
        public LyricsForm()
        {
            InitializeComponent();

            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Top);
            TopMost = true;

        }

        public async void Show(string artist,string title)
        {
           this.Show();

           try
           {
               this.richTextBox1.Text = JsonConvert.DeserializeObject<LyricsModel>(await Lyrics(artist, title)).Lyrics;
           }
           catch (Exception e)
           {
               richTextBox1.Text = "Error :(";
           }
          
        }

        static async Task<string> Lyrics(string artist, string title)
        {
            var baseAddress = new Uri("https://api.Lyrics.ovh/v1/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                using (var response = await httpClient.GetAsync($"{artist}/{title}"))
                {

                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
