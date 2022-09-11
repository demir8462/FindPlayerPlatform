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
    public partial class girisyapp : Form
    {
        dbmanager manager;
        Login lgnform;
        public static Form girisyap;
        Menu menu;
        public girisyapp()
        {
            InitializeComponent();
        }

        private void girisyapp_Load(object sender, EventArgs e)
        {
            manager = new dbmanager();
            manager.sqlbaglan();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lgnform = new Login();
            girisyap = this;
            Hide();
            lgnform.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
            {
                if(manager.usercontrol(textBox1.Text, textBox2.Text)) // şifre kontrol
                {
                    menu = new Menu(textBox1.Text);
                    menu.Show();
                    Hide();
                }else
                {
                    MessageBox.Show("HATALI ŞİFRE","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }
    }
}
