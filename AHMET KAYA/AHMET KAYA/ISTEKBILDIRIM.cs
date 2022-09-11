using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHMET_KAYA
{
    public partial class ISTEKBILDIRIM : Form
    {
        string nick, oyun, dc;
        Client client;
        StreamWriter writer;
        public ISTEKBILDIRIM(ref StreamWriter writer)
        {
            InitializeComponent();
            this.writer = writer;   
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // CVP:KIME:CEVAP:CVP giden cvp CVP:KIMDEN:CEVAP:CVP
        {
            CevapYolla("EVET",nick);
            this.Hide();
            Application.ExitThread();
        }

        private void ISTEKBILDIRIM_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CevapYolla("HAYIR",nick);
            this.Hide();
            Application.ExitThread();
        }

        public void setText(string nick,string oyun,string dc)
        {
            this.nick = nick;
            this.oyun = oyun;
            this.dc = dc;
            textBox1.Text = oyun;
            textBox2.Text = dc;
            label1.Text = nick + "'DAN OYUN ISTEGI !";
        }
        public void CevapYolla(string cevap, string kime)
        {
            writer.WriteLine("#CVP:" + kime + ":" + cevap + ":CVP#");
            writer.Flush();
        }
    }
}
