using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AHMETKAYAMASERVER
{
    public class Client
    {
        public bool bagli;
        public string Nick,Isim,Oyun,Dc;
        public string ipadres;
        public Socket soket;
        public NetworkStream stream;
        public StreamWriter gonderici;
        public StreamReader alici;
        Thread baglantikontrolthreadi,veriGuncelleThr;

        string gelenline;
        bool nickalindi=false;
        short atlandi=0;

        MySqlCommand cmd;
        MySqlDataAdapter da = new MySqlDataAdapter();
        DataTable dt = new DataTable();
        public Client(Socket soket,MySqlCommand cmd)
        {
            this.soket = soket;
            stream = new NetworkStream(soket);
            gonderici = new StreamWriter(stream);
            alici = new StreamReader(stream);
            baglantikontrolthreadi = new Thread(new ThreadStart(baglantikontrol));
            baglantikontrolthreadi.Start();
            bagli = true;
            this.cmd = cmd;
            
        }
        void veriGuncelle()
        {
            while(bagli)
            {
                if (!nickalindi)
                    continue;
                cmd.CommandText = "SELECT * FROM Users WHERE Nick='" + Nick + "'";
                da.SelectCommand = cmd;
                da.Fill(dt);
                Nick = dt.Rows[0][2].ToString();
                Isim = dt.Rows[0][1].ToString();
                Oyun = dt.Rows[0][3].ToString();
                Dc = dt.Rows[0][4].ToString();
                Thread.Sleep(2000);
            }
        }
        public void baglantikontrol()
        {

            try
            {
                while (bagli)
                {
                    gelenline = alici.ReadLine();
                    if (gelenline != null)
                    {
                        if (gelenline.Length > 0 && gelenline.StartsWith('#') && gelenline.EndsWith('#'))
                        {
                            gelenline = gelenline.Replace("#", "");
                            if (!nickalindi)
                            {
                                gelenline = gelenline.Replace("#", "");
                                Nick = gelenline;
                                setOnline(true);
                                nickalindi = true;
                                veriGuncelleThr = new Thread(new ThreadStart(veriGuncelle));
                                veriGuncelleThr.Start();
                            }else if(gelenline.StartsWith("OI"))
                            {
                                string[] OI = gelenline.Split(':'); // OI:NİCK:OI
                                string kime = OI[1];
                                string gonderilecekmesaj = "#OI:" + Nick + ":" + Oyun + ":" + Dc + ":OI#";
                                MessageBox.Show("SERVERE GELEN MESAJ:"+gonderilecekmesaj);
                                foreach (Client item in Form1.clients)
                                {
                                    if(item.Nick == kime)
                                    {
                                        MessageBox.Show("MESAJ GONDERILIYOR");
                                        item.gonderici.WriteLine(gonderilecekmesaj);
                                        item.gonderici.Flush();
                                    }
                                }
                            }else if(gelenline.StartsWith("CVP")) // gelen cvp : CVP:KIME:CEVAP:CVP giden cvp CVP:KIMDEN:CEVAP:CVP
                            {
                                string[] CEVAPS = gelenline.Split(':'); // OI:NİCK:OI
                                string kime = CEVAPS[1];
                                string cevap = CEVAPS[2];
                                string gonderilecekmesaj = "#CVP:" + Nick + ":"+cevap + ":CVP#";
                                foreach (Client item in Form1.clients)
                                {
                                    if (item.Nick == kime)
                                    {
                                        MessageBox.Show("CEVAP GONDERILIYOR");
                                        item.gonderici.WriteLine(gonderilecekmesaj);
                                        item.gonderici.Flush();
                                    }
                                }
                            }
                        }
                        atlandi = 0;
                    }
                    else
                    {
                        MessageBox.Show("atlandı : " + (atlandi + 1).ToString());
                        atlandi++;
                        if (atlandi == 2)
                        {
                            baglantikoptu();
                        }
                    }
                    Thread.Sleep(2000);
                }
            }catch(Exception e)
            {
                baglantikoptu();
            }
        }
        void baglantikoptu()
        {
            alici.Close();
            gonderici.Close();
            stream.Close();
            soket.Close();
            bagli = false;
            setOnline(false);
        }
        void mesajal(bool keep)
        {
            if (keep)
                baglantikontrolthreadi.Start();
            else
                baglantikontrolthreadi.Abort();
        }

        public void setOnline(bool status)
        {
            MessageBox.Show("UPDATE Users SET online='" + status.ToString() + "' WHERE Nick='" + Nick + "'");
            cmd.CommandText = "UPDATE Users SET online='" + status.ToString() + "' WHERE Nick='" + Nick + "'";
            if (cmd.ExecuteNonQuery() != 1)
                MessageBox.Show("hata");
        }
    }
}
