using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TXTSCREEN;
namespace AHMET_KAYA
{
    public enum ISTEKTIPI
    {
        OYUN,MESAJ
    }
    public class Client
    {
        public string ip, nick;
        public TcpClient Istemci;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        Thread mesajthr,kontrolthr;
        ISTEKBILDIRIM ib;
        MesajIstek FORMmib;
        bool OYUNBEKLE;
        public static bool MSGKBLKNTRL;
        Form1 chatscreen;
        public Client(string ip, string nick)
        {
            this.ip = ip;
            this.nick = nick;
        }

        public bool baglan()
        {
            Istemci = new TcpClient(ip, 2525);
            if(Istemci.Connected)
            {
                MessageBox.Show("baglandi");
                mesajthr = new Thread(new ThreadStart(kontrolmesajigonder));
                kontrolthr = new Thread(new ThreadStart(mesajKontrol));
            }
            stream = Istemci.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            ib = new ISTEKBILDIRIM(ref writer);
            FORMmib = new MesajIstek(ref writer);
            kontrolthr.Start();
            mesajthr.Start();
            
            return true;
        }
        void kontrolmesajigonder()
        {
            while(true)
            {
                writer.WriteLine("#" + nick + "#");
                writer.Flush();
                Thread.Sleep(4000);
                Menu.i = 1;
            }
        }
        void mesajKontrol()
        {
            string Mesaj;
            string[] istekbilgi;
            while(true)
            {
                Menu.i = 0;
                Mesaj = reader.ReadLine();
                if (Mesaj != null && Mesaj.Length != 0)
                {
                    MessageBox.Show(Mesaj);
                    if(Mesaj.StartsWith("#OI")) // OYUN ISTEGI MI KONTROL ETMEK ICIN FORM : #OI:NICK:OYUN:DISCORD:OI#
                    {
                        istekbilgi = Mesaj.Split(':');
                        MessageBox.Show("N:" + istekbilgi[1]+" o:" + istekbilgi[2]+" DC:" + istekbilgi[3]);
                        ib.setText(istekbilgi[1], istekbilgi[2], istekbilgi[3]);
                        ib.Show();
                        Application.Run(); // WTF ?
                        
                    }else if (Mesaj.StartsWith("#CVP"))  //gelen cvp CVP:KIMDEN:CEVAP:CVP
                    {
                        istekbilgi = Mesaj.Split(':'); 
                        string kimden = istekbilgi[1]; 
                        string cevap = istekbilgi[2];
                        string partoftext;
                        if (OYUNBEKLE)
                        {
                            partoftext = "Oyun";
                        }
                        else
                            partoftext = "Mesajlaşma";
                        if(cevap=="EVET")
                        {
                           
                            MessageBox.Show(kimden+" "+partoftext+" isteğini kabul etti !","HARİKA !",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }else if(cevap=="HAYIR")
                        {
                            MessageBox.Show(kimden + ""+partoftext+" Oyun isteğini red etti !", "TÜH !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if(!OYUNBEKLE && cevap == "EVET")
                        {
                            MessageBox.Show("asdas");
                            startChat(kimden);
                        }
                    }
                    else if (Mesaj.StartsWith("#MI"))  //gelen cvp CVP:KIMDEN:CEVAP:CVP
                    {
                        istekbilgi = Mesaj.Split(':');
                        string kimden = istekbilgi[1];
                        FORMmib.setText(kimden);
                        FORMmib.Show();
                        Application.Run();
                        if(MSGKBLKNTRL)
                        {
                            chatscreen = new Form1(ref writer,ref reader, kimden);
                            chatscreen.Show();
                            Application.Run();
                        }
                    }
                    else if (Mesaj.StartsWith("#MSG"))  //gelen cvp CVP:KIMDEN:CEVAP:CVP
                    {
                        if (Mesaj.Split(':')[1] == "END")
                        {
                            Form1.sohbetdevam = false;
                        }
                        MessageBox.Show("MESAJ EKLEDIM : "+Mesaj);
                        Form1.mesajlar.Add(Mesaj);
                    }
                }
                Thread.Sleep(2000);
            }
        }
        public void IstekYolla(string kime,ISTEKTIPI tip)// OYUN ISTEGI MI KONTROL ETMEK ICIN FORM : #OI:NICK:OI#
        {
            string mesaj;
            if(tip == ISTEKTIPI.OYUN)
                writer.WriteLine("#OI:"+kime+":OI#");
            else if(tip == ISTEKTIPI.MESAJ)
                writer.WriteLine("#MI:" + kime + ":MI#");
            writer.Flush();
        }
        public void MesajIstekYolla(string kime)// OYUN ISTEGI MI KONTROL ETMEK ICIN FORM : #OI:NICK:OI#
        {
            writer.WriteLine("#MI:" + kime + ":MI#");
            writer.Flush();
        }
        void startChat(string kimle)
        {
            writer.WriteLine("#SC:"+kimle+":SC#");
            writer.Flush();
            chatscreen = new Form1(ref writer,ref reader,kimle);
            chatscreen.Show();
            Application.Run();
        }
    }
}
