using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dis_hastanesi
{
    public partial class Form1 : Form
    {

        NpgsqlConnection baglanti = new NpgsqlConnection("Host = localhost; Username = postgres; Password = 3578; Database = dis_hastane;");
        DataTable veritablosu = new DataTable();

        public static string doktor_secim ="";

        public static string stajyer_secim = "";
        public static string randevu_id_getir="";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void yenile()
        {
            veritablosu.Clear();


            baglanti.Open();


            NpgsqlCommand komut = new NpgsqlCommand("SELECT \"randevu\".\"tarih\",\"unvan\".\"unvan_adi\",\"personel\".\"adi_soyadi\",\"hasta\".\"adi_soyadi\",\"randevu\".\"randevu_verilis_tarih\" from \"randevu\"  inner join personel on personel.personel_id = randevu.hekim_id inner join hasta on hasta.hasta_id = randevu.hasta_id inner join unvan on personel.unvan_id = unvan.unvan_id  where randevu.tarih::TIMESTAMP::DATE='" + Convert.ToDateTime(DateTime.Today) + "'", baglanti);
        
            if (doktor_secim!="") {
                DateTime dt = DateTime.Now;
                NpgsqlCommand doktor_id = new NpgsqlCommand("SELECT \"personel\".personel_id from  personel  inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi || ' ' ||  personel.adi_soyadi = '" + doktor_secim + "'", baglanti);

             komut = new NpgsqlCommand("SELECT \"randevu\".\"tarih\",\"unvan\".\"unvan_adi\",\"personel\".\"adi_soyadi\",\"hasta\".\"adi_soyadi\",\"randevu\".\"randevu_verilis_tarih\" from \"randevu\"  inner join personel on personel.personel_id = randevu.hekim_id inner join hasta on hasta.hasta_id = randevu.hasta_id inner join unvan on personel.unvan_id = unvan.unvan_id where randevu.hekim_id=" + doktor_id.ExecuteScalar()+ " and randevu.tarih::TIMESTAMP::DATE = '" + Convert.ToDateTime(DateTime.Today) + "'", baglanti);


            }


            NpgsqlDataReader okuyucu = komut.ExecuteReader();


            while (okuyucu.Read())
            {
                DataRow satır = veritablosu.NewRow();

                for (int i = 0; i < veritablosu.Columns.Count; i++) satır[i] = okuyucu[i].ToString();


                veritablosu.Rows.Add(satır);
            }

            dataGridView1.DataSource = veritablosu;


            baglanti.Close();
        }




        private void button1_Click(object sender, EventArgs e)
        {


            if (comboBox2.Text == "")      MessageBox.Show("Doktor Seçimi Yapınız...");
            else if(comboBox1.Text == "") MessageBox.Show("Stajyer Seçimi Yapınız...");

                else
                {
                doktor_secim = comboBox2.Text;
                stajyer_secim = comboBox1.Text;
                Form3 gecis = new Form3();
                gecis.Show();
                this.Hide();}


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (doktor_secim != "")
            {
                baglanti.Open();
                NpgsqlCommand randevu_id = new NpgsqlCommand("SELECT randevu_id from randevu    where  tarih ='" +dataGridView1.CurrentRow.Cells[0].Value.ToString()+ "'", baglanti);

              randevu_id_getir=  randevu_id.ExecuteScalar().ToString();
                baglanti.Close();

                button3.PerformClick();

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (comboBox2.Text == "") MessageBox.Show("Doktor Seçimi Yapınız...");
            else if (comboBox1.Text == "") MessageBox.Show("Stajyer Seçimi Yapınız...");

            else
            {
                doktor_secim = comboBox2.Text;
                stajyer_secim = comboBox1.Text;
                Form2 gecis = new Form2();
                gecis.Show();
                this.Hide();
            }


        



    }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            doktor_secim = comboBox2.Text;
            stajyer_secim = comboBox1.Text;

            yenile();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            veritablosu.Columns.Add("Randevu Tarihi");

            veritablosu.Columns.Add("Unvan");
            veritablosu.Columns.Add("Doktor Adı");
            veritablosu.Columns.Add("Hasta Adı");
            veritablosu.Columns.Add("Randevu Veriliş Tarihi");  
            baglanti.Open();
            NpgsqlCommand doktor_komut = new NpgsqlCommand("SELECT \"unvan\".\"unvan_adi\",\"personel\".\"adi_soyadi\" from \"personel\"   inner join unvan on personel.unvan_id = unvan.unvan_id ", baglanti);
            NpgsqlDataReader doktor_oku = doktor_komut.ExecuteReader();

            while (doktor_oku.Read())
            {
                comboBox2.Items.Add(doktor_oku["unvan_adi"]+" "+doktor_oku["adi_soyadi"]);
            }

            baglanti.Close();

            baglanti.Open();
            NpgsqlCommand stajyer_komut = new NpgsqlCommand("SELECT * from \"stajyer_hekim\"  ", baglanti);
            NpgsqlDataReader stajyer_oku = stajyer_komut.ExecuteReader();

            while (stajyer_oku.Read())
            {
                comboBox1.Items.Add( stajyer_oku["adi_soyadi"]);

                listBox1.Items.Add(stajyer_oku["adi_soyadi"] + " ------ " + stajyer_oku["stajyer_puan"]+"\n");
                
            }






            baglanti.Close();
     
            yenile();   }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
