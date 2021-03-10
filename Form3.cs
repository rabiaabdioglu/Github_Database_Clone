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
    public partial class Form3 : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("Host = localhost; Username = postgres; Password = 3578; Database = dis_hastane;");
        DataTable veritablosu = new DataTable();

        public Form3()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand doktor_id = new NpgsqlCommand("SELECT \"personel\".personel_id from  personel  inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi ||' '||  personel.adi_soyadi = '" + Form1.doktor_secim + "'", baglanti);

            NpgsqlCommand randevu_id = new NpgsqlCommand("SELECT randevu_id from randevu where  randevu.hekim_id='" + doktor_id.ExecuteScalar() + "' and randevu.tarih ='" +Convert.ToDateTime(dataGridView1.CurrentRow.Cells[0].Value.ToString()) + "'", baglanti);
          
            
            
            if (MessageBox.Show("Randevu kaydını silmek istediğinize emin misiniz?", "Onay Verin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Kayıt Silindi.");
                NpgsqlCommand randevu_iptal = new NpgsqlCommand("DELETE from randevu where randevu_id='"+ Convert.ToInt32( randevu_id.ExecuteScalar())+"'", baglanti);
               randevu_iptal.ExecuteNonQuery();
            }


            baglanti.Close();

            yenile();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 gecis = new Form1();
            gecis.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void yenile()
        {
            veritablosu.Clear();


            baglanti.Open();

            NpgsqlCommand doktor_id = new NpgsqlCommand("SELECT  \"personel\".\"personel_id\" from \"personel\"   inner join unvan on personel.unvan_id = unvan.unvan_id where   unvan.unvan_adi || ' ' ||  personel.adi_soyadi ='" + Form1.doktor_secim + "'", baglanti);

            NpgsqlCommand komut = new NpgsqlCommand("SELECT \"randevu\".\"tarih\",\"hasta\".\"tc\",\"hasta\".\"adi_soyadi\",\"randevu\".\"randevu_verilis_tarih\" from \"randevu\"  inner join personel on personel.personel_id = randevu.hekim_id inner join hasta on hasta.hasta_id = randevu.hasta_id where randevu.hekim_id='" + Convert.ToInt32(doktor_id.ExecuteScalar())+"'", baglanti);

            


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
        private void Form3_Load(object sender, EventArgs e)
        {
       
            veritablosu.Columns.Add("Tarih");
            veritablosu.Columns.Add("Hasta Tc");
            veritablosu.Columns.Add("Hasta Adı Soyadı");
            veritablosu.Columns.Add("Randevu Veriliş Tarihi"); yenile();
     baglanti.Open();
            NpgsqlCommand brans_oku = new NpgsqlCommand("select brans_adi from brans inner join personel_brans on personel_brans.brans_id=brans.brans_id inner join personel on personel.personel_id = personel_brans.personel_id inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi || ' ' || personel.adi_soyadi = '" + Form1.doktor_secim + "'", baglanti);

            label3.Text = Form1.doktor_secim;

            NpgsqlDataReader okuyucu = brans_oku.ExecuteReader();

            while (okuyucu.Read())
            {
                label4.Text += okuyucu["brans_adi"] + "\n";
            }

            baglanti.Close();
            baglanti.Open();







            NpgsqlCommand doktor_komut = new NpgsqlCommand("SELECT \"unvan\".\"unvan_adi\",\"personel\".\"adi_soyadi\" from \"personel\"   inner join unvan on personel.unvan_id = unvan.unvan_id where   unvan.unvan_adi || ' ' ||  personel.adi_soyadi !='"+Form1.doktor_secim+"'", baglanti);
            NpgsqlDataReader doktor_oku = doktor_komut.ExecuteReader();



            while (doktor_oku.Read())
            {
                comboBox2.Items.Add(doktor_oku["unvan_adi"] + " " + doktor_oku["adi_soyadi"]);
            }

            baglanti.Close();
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text=="")
            {
                MessageBox.Show("Önce Doktor Seçimi Yapınız...");
            }
            else
            {



                baglanti.Open(); 
                
                NpgsqlCommand   doktor_id = new NpgsqlCommand("SELECT \"personel\".\"personel_id\" from \"personel\"   inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi || ' ' ||  personel.adi_soyadi !='" + Form1.doktor_secim + "'", baglanti);

                NpgsqlCommand randevu_id = new NpgsqlCommand("SELECT randevu_id from randevu    where hekim_id='" + Convert.ToInt32(doktor_id.ExecuteScalar()) + "' and tarih ='" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "'", baglanti);
                doktor_id = new NpgsqlCommand("SELECT \"personel\".\"personel_id\" from \"personel\"   inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi || ' ' ||  personel.adi_soyadi ='" + comboBox2.Text + "'", baglanti);

               
                string guncelle = "UPDATE randevu SET  hekim_id='" + Convert.ToInt32(doktor_id.ExecuteScalar())+ "' where  randevu_id='" + Convert.ToInt32(randevu_id.ExecuteScalar()) + "'" ;

                NpgsqlCommand degis = new NpgsqlCommand(guncelle, baglanti);

                degis.ExecuteNonQuery();
               baglanti.Close();  yenile();
               

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
