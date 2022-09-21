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
    public partial class MesajIstek : Form
    {
        StreamWriter w;
        string kimden;
        public MesajIstek(ref StreamWriter writer)
        {
            InitializeComponent();
            w = writer;
        }

        private void MesajIstek_Load(object sender, EventArgs e)
        {

        }
        public void setText(string kimden)
        {
            label1.Text = kimden + "'DAN MESAJ ISTEGI";
            this.kimden = kimden;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            w.WriteLine("#CVP:"+kimden+":EVET:CVP#");
            Client.MSGKBLKNTRL = true;
            Application.ExitThread();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            w.WriteLine("#CVP:" + kimden + ":HAYIR:CVP#");
            Application.ExitThread();
        }

        private void MesajIstek_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
        }
    }
}
