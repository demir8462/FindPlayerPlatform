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
    public partial class Login : Form
    {
        string secilioyunlar;
        public dbmanager manager;
        public Login()
        {
            InitializeComponent();
            manager = new dbmanager();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkedListBox1.SelectedItems.Count ==0)
            {
                MessageBox.Show("En az bir tane ilgi alanı seç !","Heyy",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if(textBox1.Text.Length ==0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
            {
                MessageBox.Show("Gerekli bilgileri doldur!", "Heyy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!manager.nick_kayit_kontrol(textBox2.Text))
            {
                MessageBox.Show("Bu nick alınmıs !", "Heyy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (string item in checkedListBox1.CheckedItems)
            {
                MessageBox.Show(item);
                secilioyunlar += item + ";";
            }
            manager.kayit_ekle(textBox1.Text, textBox2.Text, secilioyunlar, textBox4.Text, textBox3.Text);
            Menu menu = new Menu(textBox2.Text);
            menu.Show();
            this.Hide();
            Menu.kayitgiris = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            girisyapp.girisyap.Show();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
