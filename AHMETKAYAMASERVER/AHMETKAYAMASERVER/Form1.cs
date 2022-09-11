using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.Net.Sockets;

namespace AHMETKAYAMASERVER
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        Socket socket;
        Client client;
        Thread baglantialthr,lduzen;
        public static List<Client> clients = new List<Client>();
        List<Client> REMOVEclients = new List<Client>();
        MySqlConnection con;
        MySqlDataAdapter da;
        MySqlCommand cmd;
        string constr = "Server=127.0.0.1;Database=ahmetkaya;Uid=root;Pwd=123123;";

        Thread listeThr;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                con = new MySqlConnection(constr);
                con.Open();
                cmd = new MySqlCommand("", con);
            }catch (Exception e)
            {
                MessageBox.Show("DB BAGLANAMADI !");
                Environment.Exit(1);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            baglantialthr = new Thread(new ThreadStart(baglantial));
            lduzen = new Thread(new ThreadStart(listeduzen));
            listeThr = new Thread(new ThreadStart(tabloDoldur));
            listeThr.Start();
        }
        void tabloDoldur()
        {
            while(true)
            {
                listView1.Items.Clear();
                foreach (Client item in clients)
                {
                    string[] lvi = { item.Nick, item.Isim, item.Oyun, item.Dc };
                    listView1.Items.Add(new ListViewItem(lvi));
                }
                Thread.Sleep(2000);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(int.Parse(textBox1.Text));
            listener.Start();
            baglantialthr.Start();
            lduzen.Start();
        }
        void listeduzen()
        {
            while(true)
            {
                foreach (Client item in clients)
                {
                    if (!item.bagli)
                    {
                        item.setOnline(false);
                        REMOVEclients.Add(item);
                    }
                }
                foreach (Client item in REMOVEclients)
                {
                    clients.Remove(item);
                }
                REMOVEclients.Clear();
                label3.Text = clients.Count.ToString();
            }
        }
        void baglantial()
        {
            while(true)
            {
                socket = listener.AcceptSocket();
                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        client = new Client(socket,cmd);
                        clients.Add(client);
                    }
                }
            }
        }
       
    }
}