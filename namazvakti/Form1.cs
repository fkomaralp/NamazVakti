using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Media;

namespace namazvakti
{
    public partial class Form1 : Form
    {
        public class NamazVakitleri
        {
            public string Imsak { get; set; }
            public string Gunes { get; set; }
            public string Oglen { get; set; }
            public string Ikindi { get; set; }
            public string Aksam { get; set; }
            public string Yatsi { get; set; }
            public string Kible { get; set; }

        }

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 60;

            label15.Text = DateTime.Now.ToString("dd.MM.yyyy");
            label16.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void OnStart(string v)
        {
            NamazVakitleri nv = GetNamazVakitleri(ConvertTurkishChars(v));

            imsak.Text = nv.Imsak;
            gunes.Text = nv.Gunes;
            ogle.Text = nv.Oglen;
            ikindi.Text = nv.Ikindi;
            aksam.Text = nv.Aksam;
            yatsi.Text = nv.Yatsi;
            kible.Text = nv.Kible;

        }

        public static string ConvertTurkishChars(string text)
        {
            text = text.ToUpper();

            String[] olds = { "Ğ", "ğ", "Ü", "ü", "Ş", "ş", "İ", "ı", "Ö", "ö", "Ç", "ç" };
            String[] news = { "G", "g", "U", "u", "S", "s", "I", "i", "O", "o", "C", "c" };

            for (int i = 0; i < olds.Length; i++)
            {
                text = text.Replace(olds[i], news[i]);
            }

            return text;
        }

        private void Update(object sender, EventArgs e)
        {
            OnStart(comboBox1.Text);
        }

        private void tick(object sender, EventArgs e)
        {
            OnStart(comboBox1.Text);
        }

        private NamazVakitleri GetNamazVakitleri(string encoded) 
        {
            return JsonConvert.DeserializeObject<NamazVakitleri>(new WebClient().DownloadString("http://hocaokudumu.com/namazsaati?Sehir=" + encoded + "&Ulke=TURKIYE&Tarih=" + DateTime.Now.ToString("dd.MM.yyyy") + "&format=json"));
        }


        //NamazVakitleri nv;
        SoundPlayer simpleSound;
        private void AlarmTimer(object sender, EventArgs e)
        {
            //NamazVakitleri nv;

            //if (String.IsNullOrEmpty(nv.Aksam))
            //{
            //    nv = GetNamazVakitleri(ConvertTurkishChars(comboBox1.Text));
            //}
             
            int min = Convert.ToInt32(numericUpDown1.Value);

            if (Convert.ToDateTime(aksam.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(gunes.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(ikindi.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(imsak.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(kible.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(ogle.Text).AddMinutes(-min) >= DateTime.Now
                || Convert.ToDateTime(yatsi.Text).AddMinutes(-min) >= DateTime.Now
                )
            {

                if (checkBox1.Checked) { 

                    SoundPlayer simpleSound = new SoundPlayer("alarm.wav");
                    simpleSound.PlayLooping();

                    DialogResult r = MessageBox.Show("Namaz Vakti!", "Namaz Vakti!");

                    if (r == DialogResult.OK)
                    {
                        simpleSound.Stop();
                    }

                }
            }

        }


        //Saat
        bool clock = false;
        private void tickclock(object sender, EventArgs e)
        {

            //Saat güncelle
            label16.Text = DateTime.Now.ToString("HH:mm:ss");

            //string[] clocks = new string[] { imsak.Text, ogle.Text, ikindi.Text, aksam.Text, yatsi.Text };

            var clocks = new Dictionary<Label, string>();

            clocks.Add(label23, imsak.Text);
            clocks.Add(label9, ogle.Text);
            clocks.Add(label12, ikindi.Text);
            clocks.Add(label14, aksam.Text);
            clocks.Add(label20, yatsi.Text);

            var next = new List<string>();
            var early = new Dictionary<string, Label>();

            foreach (var item in clocks)
	        {

		        TimeSpan duration = DateTime.Parse(item.Value).Subtract(DateTime.Parse(label16.Text));
                if (duration.TotalHours > 0)
                {
                    next.Add(duration.ToString());
                    label11.Text = next[0].ToString();

                    //item.Key.BackColor = Color.Teal;
                }
                else
                {
                    early.Add(duration.ToString(), item.Key); 
                }

                

	        }

            early.Last().Value.BackColor = Color.Teal;

        }

        private void ToFront(object sender, EventArgs e)
        {
            this.TopMost = false;

            if (checkBox2.Checked)
            {
                this.TopMost = true;
            }
        }
    } 

    
}
