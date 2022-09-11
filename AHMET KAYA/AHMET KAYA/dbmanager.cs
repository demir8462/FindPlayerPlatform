using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using MySql.Data.MySqlClient;

namespace AHMET_KAYA
{
    public class dbmanager
    {
        public MySqlConnection con;
        public MySqlDataAdapter da = new MySqlDataAdapter();
        public MySqlCommand cmd = new MySqlCommand();
        public DataTable dataTable = new DataTable();
        string constr = "Server=127.0.0.1;Database=ahmetkaya;Uid=root;Pwd=123123;";
        public void sqlbaglan()
        {
            con = new MySqlConnection(constr);
            con.Open();
        }
        public bool nick_kayit_kontrol(string nick)
        {
            
            selectSorgu("SELECT Nick FROM Users WHERE Nick='" + nick + "';");
            if (dataTable.Rows.Count != 0)
                return false;
            return true;
        }
        public dbmanager()
        {
            sqlbaglan();
        }
        public DataTable selectSorgu(string komut)
        {
            dataTable.Clear();
            cmd.CommandText = komut;
            cmd.Connection = con;
            da.SelectCommand = cmd;
            
            da.Fill(dataTable);
            return dataTable;
        }
        public bool kayit_ekle(string isim,string nick,string oyun,string discord,string pass)
        {
            cmd = new MySqlCommand("INSERT INTO Users (Isim,Nick,Oyun,Discord,Online,PASSWORD) VALUES(@Isim,@Nick,@OYUN,@Discord,@Online,@PASSWORD);", con);
            cmd.Parameters.AddWithValue("@Isim",isim);
            cmd.Parameters.AddWithValue("@Nick", nick);
            cmd.Parameters.AddWithValue("@OYUN", oyun);
            cmd.Parameters.AddWithValue("@Discord", discord);
            cmd.Parameters.AddWithValue("@Online", "true");
            cmd.Parameters.AddWithValue("@PASSWORD", pass);
            if (cmd.ExecuteNonQuery() == 1)
                return true;
            else
                return false;
        }
        public bool usercontrol(string nick,string sifre)
        {
            selectSorgu("SELECT PASSWORD FROM Users WHERE Nick='"+nick+"'");
            if (dataTable.Rows.Count == 0)
                return false;
            if (dataTable.Rows[0][0].ToString() == sifre)
                return true;
            return false;
        }
    }
    
}
