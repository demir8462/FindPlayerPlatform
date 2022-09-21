namespace TXTSCREEN
{
    public partial class Form1 : Form
    {
        StreamWriter sw;
        StreamReader sr;
        Thread MesajKontrol;
        public static bool sohbetdevam;
        string kimle;
        public static List<String> mesajlar = new List<String>();
        public Form1(ref StreamWriter writer,ref StreamReader reader,string kimle)
        {
            InitializeComponent();
            sw = writer;
            sr = reader;
            sohbetdevam = true;
            MesajKontrol = new Thread(new ThreadStart(MesajAl));
            MesajKontrol.Start();
            this.kimle = kimle;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("ME:" + textBox2.Text);
            textBox1.AppendText(Environment.NewLine);
            MesajYolla(textBox2.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        void MesajAl()
        {
            while(sohbetdevam)
            {
                string gelenline = sr.ReadLine();
                if (gelenline != null && gelenline.Length != 0)
                {
                    gelenline = gelenline.Substring(1, gelenline.Length - 2);
                    int ayracsayac = 0;
                    int startfrom = 0;
                    string kime = gelenline.Split(':')[2];
                    for (int i = 0; i < gelenline.Length; i++)
                    {
                        if (gelenline[i] == ':')
                            ayracsayac++;
                        if (ayracsayac == 3)
                        {
                            startfrom = i + 1;
                            break;
                        }
                    }
                    string mesaj = gelenline.Substring(startfrom, gelenline.Length - startfrom);
                    textBox1.AppendText(kime+":"+mesaj);
                    textBox1.AppendText(Environment.NewLine);
                }
                Thread.Sleep(1000);
            }
            MessageBox.Show("SOHBET SONA ERDI !");
            Close();
        }
        void MesajYolla(string m)
        {

            sw.WriteLine("#MSG:TO:"+kimle+":"+m+"#");
            sw.Flush();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sohbetdevam = false;
            sw.WriteLine("#MSG:END:" + kimle + "#");
            sw.Flush();
        }
    }
}