using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dis_hastanesi
{
    public partial class Form2 : Form
    {

        NpgsqlConnection baglanti = new NpgsqlConnection("Host = localhost; Username = postgres; Password = 3578; Database = dis_hastane;");
        DataTable veritablosu = new DataTable();
        string secilen="";

        public Form2()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        } 
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || comboBox6.Text == "" || comboBox4.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("Boş yer bırakmayınız.");
            }
            else
            {

                baglanti.Open();

                NpgsqlCommand sehir_sorgu = new NpgsqlCommand("SELECT id from \"sehir\" where  baslik = '" + comboBox4.Text + "'", baglanti);

                string ekle = "insert into hasta(tc,adi_soyadi,cinsiyet,dogum_tarih,sehir_id,telno) values(" + textBox5.Text + ",'" + textBox1.Text
                    + "','" + comboBox6.Text + "','" + dateTimePicker1.Value + "','" + sehir_sorgu.ExecuteScalar() + "','"+textBox2.Text+"')";

                NpgsqlCommand eklekomut = new NpgsqlCommand(ekle, baglanti);
                eklekomut.ExecuteNonQuery();
                MessageBox.Show("Hasta Kaydı Oluşturuldu.");

                //veritablosu.Clear();
                //   yenile();

                //MessageBox.Show(Form1.no + "  nolu hastanın randevu kaydı oluşturuldu.");


                baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || comboBox6.Text == "" || comboBox4.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("Boş yer bırakmayınız.");
            }
            else
            {
                baglanti.Open();
                NpgsqlCommand sehir_sorgu = new NpgsqlCommand("SELECT id from \"sehir\" where  baslik = '" + comboBox4.Text + "'", baglanti);

                string guncelle = "UPDATE hasta SET  adi_soyadi='" + textBox1.Text
                            + "' , dogum_tarih='" + dateTimePicker1.Value + "' , cinsiyet='"
                            + comboBox6.Text + "' , sehir_id='" + sehir_sorgu.ExecuteScalar() +
                            "', telno='"
                            + textBox2.Text + "' where tc =" + textBox5.Text;

                NpgsqlCommand degis = new NpgsqlCommand(guncelle, baglanti);

                degis.ExecuteNonQuery();
                MessageBox.Show("Hasta kaydı güncellendi.");

                baglanti.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
                baglanti.Open();

            NpgsqlCommand komut = new NpgsqlCommand("delete from hasta where tc ='" + textBox5.Text+"'", baglanti);

            if (MessageBox.Show(textBox5.Text + " Tc nolu hastanın kaydını silmek istediğinize emin misiniz?", "Onay Verin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Kayıt Silindi.");
                komut.ExecuteNonQuery();

            }
           

            baglanti.Close(); yenile();
        }


        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

          
            if (Form1.randevu_id_getir!="")
            {
                baglanti.Open();
                NpgsqlCommand hasta_tc_bul = new NpgsqlCommand("SELECT \"hasta\".tc from  hasta inner join randevu on hasta.hasta_id=randevu.hasta_id where randevu_id = '" +Convert.ToInt32( Form1.randevu_id_getir )+ "'", baglanti);
                textBox5.Text = hasta_tc_bul.ExecuteScalar().ToString();
                baglanti.Close();button7.PerformClick();
            }

             baglanti.Open();

            NpgsqlCommand ulkekomut = new NpgsqlCommand("Select ulke_baslik from ulke", baglanti);
            NpgsqlDataReader ulke_oku = ulkekomut.ExecuteReader();

            while (ulke_oku.Read())
            {
                comboBox3.Items.Add(ulke_oku["ulke_baslik"]);
            }


            baglanti.Close();



            comboBox3.SelectedIndex = 222;


            veritablosu.Columns.Add("İd");

            veritablosu.Columns.Add("Tarih");
            veritablosu.Columns.Add("Hasta Adı Soyadı");
            veritablosu.Columns.Add("Unvan");
            veritablosu.Columns.Add("Doktor Adı Soyadı");
            //veritablosu.Columns.Add("Stajyer Adı Soyadı");

            veritablosu.Columns.Add("Tanı");
            veritablosu.Columns.Add("Uygulanan Tedavi");
            veritablosu.Columns.Add("İlaç");






        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {comboBox4.Text = "";
            comboBox4.Items.Clear();
            baglanti.Open();

            NpgsqlCommand sehirkomut = new NpgsqlCommand("SELECT sehir.baslik from \"sehir\" inner join ulke on ulke.id = sehir.ulke_id where  ulke_baslik = '" + comboBox3.Text + "'", baglanti);
            NpgsqlDataReader sehir_oku = sehirkomut.ExecuteReader();

            while (sehir_oku.Read())
            {
                comboBox4.Items.Add(sehir_oku["baslik"]);
            }


            baglanti.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            NpgsqlCommand tedavi_komut = new NpgsqlCommand("Select tedavi_adi from tedavi", baglanti);
            NpgsqlDataReader tedavi_oku = tedavi_komut.ExecuteReader();

            while (tedavi_oku.Read())
            {
                comboBox1.Items.Add(tedavi_oku["tedavi_adi"]);
            }


            baglanti.Close();

        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            NpgsqlCommand tani_komut = new NpgsqlCommand("Select tani_adi from tani", baglanti);
            NpgsqlDataReader tani_oku = tani_komut.ExecuteReader();

            while (tani_oku.Read())
            {
                comboBox2.Items.Add(tani_oku["tani_adi"]);
            }

            baglanti.Close();

        }

        private void comboBox5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand ilac = new NpgsqlCommand("Select ilac_adi from ilac", baglanti);
            NpgsqlDataReader ilacoku = ilac.ExecuteReader();

            while (ilacoku.Read())
            {
                comboBox5.Items.Add(ilacoku["ilac_adi"]);
            }

            baglanti.Close();
        }

        private void comboBox3_Click(object sender, EventArgs e)
        {



        } 
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


        }

        private void yenile()
        {
            baglanti.Open();

            if (textBox5.Text != "")
            {
                NpgsqlCommand hasta_kayit = new NpgsqlCommand("SELECT    islem_kayıt . islem_id , islem_kayıt . tarih   ,  hasta . adi_soyadi  , unvan . unvan_adi  , personel . adi_soyadi  , stajyer_hekim . adi_soyadi , tani . tani_adi , tedavi . tedavi_adi   , ilac . ilac_adi    from  islem_kayıt   inner join tani on tani.tani_id = islem_kayıt.tani_id inner join hasta on hasta.hasta_id = islem_kayıt.hasta_id  inner join tedavi on tedavi.tedavi_id = islem_kayıt.tedavi_id  inner join ilac on ilac.ilac_id = islem_kayıt.ilac_id inner join stajyer_hekim on islem_kayıt.stajyer_id = stajyer_hekim.stajyer_id  inner join personel on personel.personel_id = islem_kayıt.personel_id  inner join unvan on unvan.unvan_id = personel.unvan_id    where  hasta.tc='" + Convert.ToInt64(textBox5.Text) + "'", baglanti);


                NpgsqlDataReader hasta_oku = hasta_kayit.ExecuteReader();

                while (hasta_oku.Read())
                {
                    DataRow satır = veritablosu.NewRow();

                    for (int i = 0; i < veritablosu.Columns.Count; i++) satır[i] = hasta_oku[i].ToString();


                    veritablosu.Rows.Add(satır);
                }

                dataGridView1.DataSource = veritablosu;
            }




            baglanti.Close();
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            veritablosu.Clear();
         baglanti.Open();
                if (textBox5.Text!="")
            {   

                NpgsqlCommand kisi_varmi = new NpgsqlCommand("SELECT count(*) from \"hasta\" where  tc = '" + textBox5.Text + "'", baglanti);

                if (Convert.ToInt32(kisi_varmi.ExecuteScalar())== 0)
                {
                MessageBox.Show("Tc no ya ait hasta kaydı bulunamadı.");
                }
                else { 
                    NpgsqlCommand kisikomut = new NpgsqlCommand("SELECT * from \"hasta\" inner join sehir on hasta.sehir_id = sehir.id inner join ulke on sehir.ulke_id = ulke.id where  tc = '" + textBox5.Text + "'", baglanti);

                    NpgsqlDataReader okuyucu = kisikomut.ExecuteReader();


                    while (okuyucu.Read())
                    {

                        textBox1.Text = okuyucu["adi_soyadi"].ToString();
                        comboBox3.Text = okuyucu["ulke_baslik"].ToString();
                        comboBox4.Text = okuyucu["baslik"].ToString();
                        dateTimePicker1.Text = okuyucu["dogum_tarih"].ToString();
                        comboBox6.Text = okuyucu["cinsiyet"].ToString();
                        textBox2.Text = okuyucu["telno"].ToString();


                    } }


                baglanti.Close();
            }
            yenile();


        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*ekle ve değiş butonlarında boş alan bırakmayı engelleyen if*/
            if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || comboBox6.Text == "" || comboBox4.Text == "" || comboBox3.Text == "" || comboBox1.Text == "" || comboBox2.Text == "" || comboBox5.Text == "")
            {
                MessageBox.Show("Boş yer bırakmayınız.");
            }
            else
            {

                baglanti.Open();

                NpgsqlCommand ilacid = new NpgsqlCommand("Select ilac_id from ilac where ilac_adi='" + comboBox5.Text + "'", baglanti);
                NpgsqlCommand hastaid = new NpgsqlCommand("Select hasta_id from hasta where tc='" + textBox5.Text + "'", baglanti);
                NpgsqlCommand tedaviid = new NpgsqlCommand("Select tedavi_id from tedavi where tedavi_adi='" + comboBox1.Text + "'", baglanti);

                NpgsqlCommand taniid = new NpgsqlCommand("Select tani_id from tani where tani_adi='" + comboBox2.Text + "'", baglanti);
                if (Convert.ToInt32(taniid.ExecuteScalar()) == 0)
                {
                    NpgsqlCommand tani_ekle = new NpgsqlCommand("insert into tani (tani_adi) values('"+comboBox2.Text+"')", baglanti);
                    tani_ekle.ExecuteNonQuery();

                }

                NpgsqlCommand doktor_id = new NpgsqlCommand("SELECT \"personel\".personel_id from  personel  inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi||' '||personel.adi_soyadi ='" + Form1.doktor_secim + "'", baglanti);
                NpgsqlCommand stajyer_id = new NpgsqlCommand("Select stajyer_id from stajyer_hekim where adi_soyadi='" + Form1.stajyer_secim + "'", baglanti);

                string ekle = "insert into islem_kayıt(hasta_id,tani_id,tedavi_id,tarih,ilac_id,personel_id,stajyer_id) values('" + Convert.ToInt32(hastaid.ExecuteScalar()) + "','" + Convert.ToInt32(taniid.ExecuteScalar())
                    + "','" + Convert.ToInt32(tedaviid.ExecuteScalar()) + "','" +Convert.ToDateTime( DateTime.Now.ToString("yyyy/MM/dd HH:mm") )+ "','" + Convert.ToInt32(ilacid.ExecuteScalar()) + "','" + Convert.ToInt32(doktor_id.ExecuteScalar())+"','" + Convert.ToInt32(stajyer_id.ExecuteScalar()) + "')";

                NpgsqlCommand eklekomut = new NpgsqlCommand(ekle, baglanti);
                eklekomut.ExecuteNonQuery();
                veritablosu.Clear();
                MessageBox.Show("İşlem kaydı oluşturuldu.");


                baglanti.Close(); yenile();
            }

        }

        private void comboBox2_DpiChangedBeforeParent(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        { 
               if (Convert.ToDateTime(dateTimePicker2.Value) < Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) )
                {
                    MessageBox.Show("Randevu zamanı yanlış seçildi..");

                }

                else {

                    baglanti.Open();

                    NpgsqlCommand hastaid = new NpgsqlCommand("Select hasta_id from hasta where tc='" + textBox5.Text + "'", baglanti);
                    NpgsqlCommand doktor_id = new NpgsqlCommand("SELECT \"personel\".personel_id from  personel  inner join unvan on personel.unvan_id = unvan.unvan_id where unvan.unvan_adi || ' ' ||  personel.adi_soyadi ='" + Form1.doktor_secim + "'", baglanti);

                    string ekle = "INSERT into randevu(hasta_id,hekim_id,randevu_verilis_tarih,tarih) values('" + Convert.ToInt32(hastaid.ExecuteScalar()) + "','" + Convert.ToInt32(doktor_id.ExecuteScalar())
+ "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")) + "','" + Convert.ToDateTime(dateTimePicker2.Value) + "')";

                    NpgsqlCommand eklekomut = new NpgsqlCommand(ekle, baglanti);
                    eklekomut.ExecuteNonQuery();

                    MessageBox.Show(dateTimePicker2.Text + "  tarihine randevu oluşturuldu.");
            

                   baglanti.Close();
           

                }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            comboBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            secilen = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand islem_kayıt_sil = new NpgsqlCommand("delete from islem_kayıt where islem_id ='"+Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString() )+ "'" ,baglanti);
            if (MessageBox.Show("İşlem kaydını silmek istediğinize emin misiniz?", "Onay Verin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("Kayıt Silindi.");

            islem_kayıt_sil.ExecuteNonQuery();
                       }


            baglanti.Close();
            veritablosu.Clear();
            yenile();  
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 gecis = new Form1();
            gecis.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            veritablosu.Reset();
            veritablosu.Columns.Add("İd");

            veritablosu.Columns.Add("Tarih");
            veritablosu.Columns.Add("Hasta Adı Soyadı");
            baglanti.Open();
            NpgsqlCommand hasta_kayit = new NpgsqlCommand("SELECT  * from rontgen", baglanti);


            NpgsqlDataReader hasta_oku = hasta_kayit.ExecuteReader();

            while (hasta_oku.Read())
            {
                DataRow satır = veritablosu.NewRow();

                for (int i = 0; i < veritablosu.Columns.Count; i++) satır[i] = hasta_oku[i].ToString();


                veritablosu.Rows.Add(satır);
            }

            dataGridView1.DataSource = veritablosu;





            baglanti.Close();

        }
    }
}
