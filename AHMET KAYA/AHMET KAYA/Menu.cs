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
    public partial class Menu : Form
    {
        dbmanager manager;
        public Client user;
        DataTable users;
        Thread monitorTHR;
        DataTable verilerim;
        public static bool kayitgiris = false;
        public static int i = 0;
        public Menu(string nick)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            user = new Client("127.0.0.1",nick);
            user.baglan();
            verilerim = new DataTable();
            this.manager = new dbmanager();
            
        }
        void kisiveriGuncelle()
        {
            manager.selectSorgu("SELECT * FROM Users WHERE Nick='"+user.nick+"'");
            
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            label1.Text = user.nick;
            monitorTHR = new Thread(new ThreadStart(getOnlinePlayers));
            monitorTHR.Start();
        }
        void setStatus(string a)
        {
            label2.Text = a;
        }
        void getOnlinePlayers()
        {
            while(true)
            {
                listView1.Items.Clear();
                users = manager.selectSorgu("SELECT * FROM Users WHERE online='true'");
                for (int i = 0; i < users.Rows.Count; i++)
                {
                    string[] lvi = { users.Rows[i][1].ToString(), users.Rows[i][2].ToString(), users.Rows[i][3].ToString(), users.Rows[i][4].ToString() };
                    listView1.Items.Add(new ListViewItem(lvi));
                }
                setStatus(DateTime.Now + " ONLINES : " + users.Rows.Count);
                
                Thread.Sleep(2000);
                button1.Enabled = true;
            }
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e) // OYUN ISTEGI MI KONTROL ETMEK ICIN FORM : #OI:NICK:OYUN:DISCORD:OI#
        {
            if (listView1.SelectedItems[0].SubItems.Count == 0)
                return;
            user.IstekYolla(listView1.SelectedItems[0].SubItems[1].Text);
            button1.Enabled = false;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(i.ToString());
        }
    }
}
