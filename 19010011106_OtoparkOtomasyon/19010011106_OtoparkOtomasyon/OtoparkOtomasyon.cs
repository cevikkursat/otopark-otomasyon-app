using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using System.IO;
using System.Timers;
using System.Data.SqlClient;

namespace _19010011106_OtoparkOtomasyon
{
    public partial class OtoparkOtomasyon : Form
    {
        public OtoparkOtomasyon()
        {
            InitializeComponent();
        }

        int otoparkBoyut = 200;
        String gelen_deger;
        int kat1Arac = 0;
        int kat2Arac = 0;
        int gosterilenKat = 0;
        string fDurum = "Çalışmıyor";
        string iDurum = "Çalışmıyor";
        string yDurum = "Çalışmıyor";
        bool ilkOkuma = false;
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-M7GIO33;Initial Catalog=otoparkOtomasyon;Integrated Security=True");
        SqlDataAdapter da;
        SqlCommand cmd;
        DataSet ds;
        bool[] arrBir = {false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false};
        bool[] arrIki = {false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false,
                         false,false,false,false,false,false,false,false,false,false};

        private void OtoparkOtomasyon_Load(object sender, EventArgs e)
        {
            try
            {
                String[] port_names = SerialPort.GetPortNames();
                cmbPorts.Items.AddRange(port_names);
                if (lbSistem.Text == "Sistem Kapalı")
                {
                    gbAracCikis.Enabled = false;
                    gbAracGiris.Enabled = false;
                    gbKatBir.Enabled = false;
                    gbKatIki.Enabled = false;
                    gbOtoparkBilgileri.Enabled = false;
                }

                cmd = new SqlCommand();
                baglanti.Open();
                cmd.Connection = baglanti;
                da = new SqlDataAdapter("Select * From aracKayit where aracCikisi = @cikis", baglanti);
                da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                ds = new DataSet();
                da.Fill(ds, "aracKayit");
                for (int i = 0; i < ds.Tables["aracKayit"].Rows.Count; i++)
                {
                    lbToplamArac.Text = ds.Tables["aracKayit"].Rows.Count + "/200";
                    cmbAracNumaralari.Items.Add(ds.Tables["aracKayit"].Rows[i]["id"]);
                }
                baglanti.Close();
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbSistem_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSistem.Checked)
            {
                gbSistem.Enabled = true;
            }
            else
            {
                gbSistem.Enabled = false;
            }
        }

        private void btnKatBirGor_Click(object sender, EventArgs e)
        {
            gosterilenKat = 1;
            otoparkDuzenGetir();
        }

        private void btnKatIkiGor_Click(object sender, EventArgs e)
        {
            gosterilenKat = 2;
            otoparkDuzenGetir();
        }

        void otoparkDuzenGetir()
        {
            bool[] arr;
            gbOtopark.Controls.Clear();

            if (gosterilenKat != 0)
            {
                if (gosterilenKat == 1) arr = arrBir;
                else arr = arrIki;

                int toplamArac = 0;
                int kactane = 0;
                int ustBosluk = 17;
                int yanBosluk = 125;
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Button btnArac = new Button();
                        btnArac.Name = (toplamArac + 1).ToString();
                        if (arr[toplamArac])
                        {
                            btnArac.BackgroundImage = Properties.Resources.doluGarage;
                        }
                        else
                        {
                            btnArac.BackgroundImage = Properties.Resources.bosGarage;
                        }
                        btnArac.ForeColor = Color.White;
                        btnArac.TextAlign = ContentAlignment.MiddleCenter;
                        btnArac.BackgroundImageLayout = ImageLayout.Stretch;
                        btnArac.Text = (char)(65 + y) + "-" + (x + 1).ToString();
                        btnArac.Font = new Font(Font.FontFamily, 9);
                        btnArac.Font = new Font(Font, FontStyle.Bold);
                        btnArac.FlatStyle = FlatStyle.Flat;
                        btnArac.FlatAppearance.BorderSize = 0;
                        btnArac.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#818181");
                        btnArac.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        btnArac.Enabled = false;
                        btnArac.Width = 50;
                        btnArac.Height = 50;
                        gbOtopark.Controls.Add(btnArac);
                        btnArac.Top = ustBosluk;
                        btnArac.Left = yanBosluk + (kactane * 60);
                        kactane++;
                        toplamArac++;
                    }
                    ustBosluk += 60;
                    kactane = 0;
                }
                toplamArac = 0;
            }
        }

        private void tbAracPlaka_TextChanged(object sender, EventArgs e)
        {
            if (tbAracPlaka.Text.Length > 0) btnArabaGirisYap.Enabled = true;
            else btnArabaGirisYap.Enabled = false;

        }

        private void cmbAracNumaralari_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAracNumaralari.SelectedIndex >= 0) btnAracCikisYap.Enabled = true;

        }

        private void tbCikisAracPlaka_TextChanged(object sender, EventArgs e)
        {
            if (tbCikisAracPlaka.Text.Length > 0) btnAracCikisYap.Enabled = true;
            else btnAracCikisYap.Enabled = false;
        }

        private void rbPlakayaGore_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPlakayaGore.Checked)
            {
                tbCikisAracPlaka.Enabled = true;
                cmbAracNumaralari.Enabled = false;
            }
        }

        private void rbNumarayaGore_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNumarayaGore.Checked)
            {
                tbCikisAracPlaka.Text = "";
                tbCikisAracPlaka.Enabled = false;
                cmbAracNumaralari.Enabled = true;
            }
        }

        private void cbRaporAl_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRaporAl.Checked) btnRaporAl.Enabled = true;
            else btnRaporAl.Enabled = false;

        }

        private void btnSistemiAc_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cmbPorts.Text;
                serialPort1.BaudRate = Convert.ToInt32(cmbBoundRates.Text);
                serialPort1.Open();
                dbLogTimer.Start();
                gbAracCikis.Enabled = true;
                gbAracGiris.Enabled = true;
                gbKatBir.Enabled = true;
                gbKatIki.Enabled = true;
                gbOtoparkBilgileri.Enabled = true;
                gbSistem.Enabled = false;
                cbSistem.Checked = false;
                lbSistem.Text = "Sistem Açık";
                lbSistem.ForeColor = Color.Green;
                MessageBox.Show(this, "Sistem Açıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSistemiKapa_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogTimer.Stop();
                serialPort1.Close();
                gbAracCikis.Enabled = false;
                gbAracGiris.Enabled = false;
                gbKatBir.Enabled = false;
                gbKatIki.Enabled = false;
                gbOtoparkBilgileri.Enabled = false;
                gbSistem.Enabled = false;
                cbSistem.Checked = false;
                lbSistem.Text = "Sistem Kapalı";
                lbSistem.ForeColor = Color.Red;
                MessageBox.Show(this, "Sistem Kapatıldı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                gelen_deger = serialPort1.ReadLine();
                this.BeginInvoke(new EventHandler(OkumaIslemi));
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OkumaIslemi(object sender, System.EventArgs e)
        {
            try
            {
                String[] gelenArr;
                gelenArr = gelen_deger.Split('-');
                if (gelenArr.Length > 10)
                {
                    if (gelenArr[0].Split(' ')[1] == "1")
                    {
                        katBirYangin.Text = "Çalışıyor";
                        katIkiYangin.Text = "Çalışıyor";
                        btnKatBirYanginCalistir.Text = "Yangın Sensörü Durdur";
                        btnKatIkiYanginCalistir.Text = "Yangın Sensörü Durdur";
                        btnKatBirYanginCalistir.BackColor = Color.DimGray;
                        btnKatBirYanginCalistir.ForeColor = Color.White;
                        btnKatIkiYanginCalistir.BackColor = Color.DimGray;
                        btnKatIkiYanginCalistir.ForeColor = Color.White;
                        if (yDurum == "Çalışmıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        yDurum = "Çalışıyor";
                    }
                    else
                    {
                        katBirYangin.Text = "Çalışmıyor";
                        katIkiYangin.Text = "Çalışmıyor";
                        btnKatBirYanginCalistir.Text = "Yangın Sensörü Çalıştır";
                        btnKatIkiYanginCalistir.Text = "Yangın Sensörü Çalıştır";
                        btnKatBirYanginCalistir.BackColor = Color.WhiteSmoke;
                        btnKatBirYanginCalistir.ForeColor = Color.Black;
                        btnKatIkiYanginCalistir.BackColor = Color.WhiteSmoke;
                        btnKatIkiYanginCalistir.ForeColor = Color.Black;
                        if (yDurum == "Çalışıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        yDurum = "Çalışmıyor";

                    }
                    if (gelenArr[1].Split(' ')[1] == "1")
                    {
                        katBirIsik.Text = "Çalışıyor";
                        katIkiIsik.Text = "Çalışıyor";
                        btnKatBirIsikCalistir.Text = "Işık Durdur";
                        btnKatIkiIsikCalistir.Text = "Işık Durdur";
                        btnKatBirIsikCalistir.BackColor = Color.DimGray;
                        btnKatBirIsikCalistir.ForeColor = Color.White;
                        btnKatIkiIsikCalistir.BackColor = Color.DimGray;
                        btnKatIkiIsikCalistir.ForeColor = Color.White;
                        if (iDurum == "Çalışmıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        iDurum = "Çalışıyor";
                    }
                    else
                    {
                        katBirIsik.Text = "Çalışmıyor";
                        katIkiIsik.Text = "Çalışmıyor";
                        btnKatBirIsikCalistir.Text = "Işık Çalıştır";
                        btnKatIkiIsikCalistir.Text = "Işık Çalıştır";
                        btnKatBirIsikCalistir.BackColor = Color.WhiteSmoke;
                        btnKatBirIsikCalistir.ForeColor = Color.Black;
                        btnKatIkiIsikCalistir.BackColor = Color.WhiteSmoke;
                        btnKatIkiIsikCalistir.ForeColor = Color.Black;
                        if (iDurum == "Çalışıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        iDurum = "Çalışmıyor";
                    }
                    if (gelenArr[10].Split(' ')[1].Trim() == "1")
                    {
                        katBirFan.Text = "Çalışıyor";
                        katIkiFan.Text = "Çalışıyor";
                        btnKatBirFanCalistir.Text = "Fan Durdur";
                        btnKatIkiFanCalistir.Text = "Fan Durdur";
                        btnKatBirFanCalistir.BackColor = Color.DimGray;
                        btnKatBirFanCalistir.ForeColor = Color.White;
                        btnKatIkiFanCalistir.BackColor = Color.DimGray;
                        btnKatIkiFanCalistir.ForeColor = Color.White;
                        if (fDurum == "Çalışmıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        fDurum = "Çalışıyor";
                    }
                    else
                    {
                        katBirFan.Text = "Çalışmıyor";
                        katIkiFan.Text = "Çalışmıyor";
                        btnKatBirFanCalistir.Text = "Fan Çalıştır";
                        btnKatIkiFanCalistir.Text = "Fan Çalıştır";
                        btnKatBirFanCalistir.BackColor = Color.WhiteSmoke;
                        btnKatBirFanCalistir.ForeColor = Color.Black;
                        btnKatIkiFanCalistir.BackColor = Color.WhiteSmoke;
                        btnKatIkiFanCalistir.ForeColor = Color.Black;
                        if (fDurum == "Çalışıyor" && ilkOkuma)
                        {
                            baglanti.Open();
                            durumKayitYap();
                            baglanti.Close();
                        }
                        fDurum = "Çalışmıyor";
                    }

                    katBirSicaklik.Text = gelenArr[2].Split(' ')[1] + " C°";
                    katIkiSicaklik.Text = gelenArr[2].Split(' ')[1] + " C°";
                    katBirNem.Text = "% " + gelenArr[3].Split(' ')[1];
                    katIkiNem.Text = "% " + gelenArr[3].Split(' ')[1];

                    string kat1park1 = gelenArr[4].Split(' ')[1];
                    string kat1park2 = gelenArr[6].Split(' ')[1];
                    string kat1park3 = gelenArr[8].Split(' ')[1];
                    string kat2park1 = gelenArr[5].Split(' ')[1];
                    string kat2park2 = gelenArr[7].Split(' ')[1];
                    string kat2park3 = gelenArr[9].Split(' ')[1];

                    kat1Arac = 0;
                    if (kat1park1 == "1")
                    {
                        kat1Arac++;
                        arrBir[15] = true;
                    }
                    else arrBir[15] = false;
                    if (kat1park2 == "1")
                    {
                        kat1Arac++;
                        arrBir[56] = true;
                    }
                    else arrBir[56] = false;
                    if (kat1park3 == "1")
                    {
                        kat1Arac++;
                        arrBir[84] = true;
                    }
                    else arrBir[84] = false;

                    kat2Arac = 0;
                    if (kat2park1 == "1")
                    {
                        kat2Arac++;
                        arrIki[10] = true;
                    }
                    else arrIki[10] = false;
                    if (kat2park2 == "1")
                    {
                        kat2Arac++;
                        arrIki[11] = true;
                    }
                    else arrIki[11] = false;
                    if (kat2park3 == "1")
                    {
                        kat2Arac++;
                        arrIki[12] = true;
                    }
                    else arrIki[12] = false;

                    katBirDoluluk.Text = kat1Arac + "/100";
                    katIkiDoluluk.Text = kat2Arac + "/100";
                    otoparkDuzenGetir();
                }
                ilkOkuma = true;
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatBirFanCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatBirFanCalistir.Text == "Fan Durdur") serialPort1.WriteLine("0-0");
                else serialPort1.WriteLine("0-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatIkiFanCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatIkiFanCalistir.Text == "Fan Durdur") serialPort1.WriteLine("0-0");
                else serialPort1.WriteLine("0-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatBirIsikCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatBirIsikCalistir.Text == "Işık Durdur") serialPort1.WriteLine("1-0");
                else serialPort1.WriteLine("1-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatIkiIsikCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatIkiIsikCalistir.Text == "Işık Durdur") serialPort1.WriteLine("1-0");
                else serialPort1.WriteLine("1-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatBirYanginCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatBirYanginCalistir.Text == "Yangın Sensörü Durdur") serialPort1.WriteLine("2-0");
                else serialPort1.WriteLine("2-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKatIkiYanginCalistir_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnKatIkiYanginCalistir.Text == "Yangın Sensörü Durdur") serialPort1.WriteLine("2-0");
                else serialPort1.WriteLine("2-1");
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dbLogTimer_Tick(object sender, EventArgs e)
        {
            baglanti.Open();
            durumKayitYap();
            baglanti.Close();
        }

        void durumKayitYap()
        {
            try
            {
                cmd = new SqlCommand();
                cmd.Connection = baglanti;
                cmd.CommandText = "insert into otoparkBilgileri(derece,nem,fanDurum,yanginSensorDurum,isikDurum,dolulukKatBirDurum,dolulukKatIkiDurum,toplamDoluluk) values (@derece,@nem,@fanDurum,@yanginSensorDurum,@isikDurum,@dolulukKatBirDurum,@dolulukKatIkiDurum,@toplamDoluluk)";
                cmd.Parameters.AddWithValue("@derece", katBirSicaklik.Text);
                cmd.Parameters.AddWithValue("@nem", katBirNem.Text);
                cmd.Parameters.AddWithValue("@fanDurum", katBirFan.Text);
                cmd.Parameters.AddWithValue("@yanginSensorDurum", katBirYangin.Text);
                cmd.Parameters.AddWithValue("@isikDurum", katBirIsik.Text);
                cmd.Parameters.AddWithValue("@dolulukKatBirDurum", katBirDoluluk.Text);
                cmd.Parameters.AddWithValue("@dolulukKatIkiDurum", katIkiDoluluk.Text);
                cmd.Parameters.AddWithValue("@toplamDoluluk", lbToplamArac.Text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnArabaGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                da = new SqlDataAdapter("Select * From aracKayit where aracCikisi = @cikis", baglanti);
                da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                ds = new DataSet();
                da.Fill(ds, "aracKayit");
                if (ds.Tables["aracKayit"].Rows.Count < otoparkBoyut)
                {
                    cmd = new SqlCommand();
                    baglanti.Open();
                    cmd.Connection = baglanti;
                    da = new SqlDataAdapter("select * from aracKayit where aracCikisi = @cikis AND aracPlaka = @plaka", baglanti);
                    da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                    da.SelectCommand.Parameters.AddWithValue("@plaka", tbAracPlaka.Text);
                    ds = new DataSet();
                    da.Fill(ds, "aracKayit");
                    if (ds.Tables["aracKayit"].Rows.Count == 0)
                    {
                        serialPort1.WriteLine("3-1");
                        lbKapiGiris.Text = "Açık";
                        cmd.CommandText = "insert into aracKayit(aracPlaka,aracCikisi) values (@aracPlaka,@aracCikisi)";
                        cmd.Parameters.AddWithValue("@aracPlaka", tbAracPlaka.Text);
                        cmd.Parameters.AddWithValue("@aracCikisi", "0");
                        cmd.ExecuteNonQuery();
                        da = new SqlDataAdapter("Select * From aracKayit where aracCikisi = @cikis", baglanti);
                        da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                        ds = new DataSet();
                        da.Fill(ds, "aracKayit");
                        cmbAracNumaralari.Items.Clear();
                        for (int i = 0; i < ds.Tables["aracKayit"].Rows.Count; i++)
                        {
                            lbToplamArac.Text = ds.Tables["aracKayit"].Rows.Count + "/200";
                            cmbAracNumaralari.Items.Add(ds.Tables["aracKayit"].Rows[i]["id"]);
                        }
                        da = new SqlDataAdapter("Select * From aracKayit", baglanti);
                        da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                        ds = new DataSet();
                        da.Fill(ds, "aracKayit");
                        MessageBox.Show(this, tbAracPlaka.Text + " plakalı araç " + ds.Tables["aracKayit"].Rows.Count + " numarasıyla " + DateTime.Now.ToString() + " tarihiyle otoparka giriş yaptı.", "Araç Girişi Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tbAracPlaka.Text = "";
                        girisTimer.Stop();
                        girisTimer.Start();
                        durumKayitYap();
                    }
                    else
                    {
                        MessageBox.Show(this, "Plaka Daha Önce Giriş Yapmış!", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    baglanti.Close();
                }
                else
                {
                    MessageBox.Show(this, "Otopark Dolu!", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAracCikisYap_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbNumarayaGore.Checked)
                {
                    if (cmbAracNumaralari.Text.Length > 0)
                    {
                        cmd = new SqlCommand();
                        baglanti.Open();
                        cmd.Connection = baglanti;
                        da = new SqlDataAdapter("select * from aracKayit where aracCikisi = @cikis and id = @id", baglanti);
                        da.SelectCommand.Parameters.AddWithValue("@id", int.Parse(cmbAracNumaralari.SelectedItem.ToString()));
                        da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                        DataSet ds = new DataSet();
                        da.Fill(ds, "aracKayit");
                        if (ds.Tables["aracKayit"].Rows.Count > 0)
                        {
                            serialPort1.WriteLine("4-1");
                            lbKapiCikis.Text = "Açık";
                            cmd.CommandText = "insert into kasaKayit(aracKayitNo,durulanZaman,alinanPara) values (@aracKayitNo,@durulanZaman,@alinanPara)";
                            cmd.Parameters.AddWithValue("@aracKayitNo", int.Parse(cmbAracNumaralari.SelectedItem.ToString()));
                            DateTime a = DateTime.Parse(ds.Tables["aracKayit"].Rows[0]["aracGiris"].ToString());
                            DateTime date = new DateTime(DateTime.Now.Ticks - a.Ticks);
                            int hour = ((date.DayOfYear - 1) * 24) + date.Hour;
                            cmd.Parameters.AddWithValue("@durulanZaman", hour);
                            cmd.Parameters.AddWithValue("@alinanPara", (hour * nmSaatlikFiyat.Value));
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "update aracKayit set aracCikisi = @cikis where id = @id";
                            cmd.Parameters.AddWithValue("@cikis", "1");
                            cmd.Parameters.AddWithValue("@id", int.Parse(cmbAracNumaralari.SelectedItem.ToString()));
                            cmd.ExecuteNonQuery();
                            da = new SqlDataAdapter("Select * From aracKayit where aracCikisi = @cikis", baglanti);
                            da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                            ds = new DataSet();
                            da.Fill(ds, "aracKayit");
                            cmbAracNumaralari.Items.Clear();
                            for (int i = 0; i < ds.Tables["aracKayit"].Rows.Count; i++)
                            {
                                lbToplamArac.Text = ds.Tables["aracKayit"].Rows.Count + "/200";
                                cmbAracNumaralari.Items.Add(ds.Tables["aracKayit"].Rows[i]["id"]);
                            }
                            da = new SqlDataAdapter("Select * From kasaKayit", baglanti);
                            ds = new DataSet();
                            da.Fill(ds, "kasaKayit");
                            int sonKayitIndex = ds.Tables["kasaKayit"].Rows.Count - 1;
                            int id = int.Parse(ds.Tables["kasaKayit"].Rows[sonKayitIndex]["aracKayitNo"].ToString());
                            string durulanZaman = ds.Tables["kasaKayit"].Rows[sonKayitIndex]["durulanZaman"].ToString();
                            string alinanPara = ds.Tables["kasaKayit"].Rows[sonKayitIndex]["alinanPara"].ToString();
                            da = new SqlDataAdapter("Select * From aracKayit where id = @id", baglanti);
                            da.SelectCommand.Parameters.AddWithValue("@id", id);
                            ds = new DataSet();
                            da.Fill(ds, "aracKayit");
                            string aracplaka = ds.Tables["aracKayit"].Rows[0]["aracPlaka"].ToString();
                            MessageBox.Show(this, aracplaka + " plakalı araç " + id.ToString() + " numarası ile " + durulanZaman + " saat otaparkta kaldığı için  " + alinanPara + " ücret ödeyerek " + DateTime.Now.ToString() + " tarhiyle otoparktan çıkış yapmıştır.", "Araç Çıkışı Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnAracCikisYap.Enabled = false;
                            cikisTimer.Stop();
                            cikisTimer.Start();
                            durumKayitYap();
                        }
                        else
                        {
                            MessageBox.Show(this, "Araç Numarası Bulunamadı", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        baglanti.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, "Araç Numarası Seçilmemiş", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (rbPlakayaGore.Checked)
                {
                    if (tbCikisAracPlaka.Text.Length > 0)
                    {
                        cmd = new SqlCommand();
                        baglanti.Open();
                        cmd.Connection = baglanti;
                        da = new SqlDataAdapter("select * from aracKayit where aracCikisi = @cikis and aracPlaka = @plaka", baglanti);
                        da.SelectCommand.Parameters.AddWithValue("@plaka", tbCikisAracPlaka.Text);
                        da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                        DataSet ds = new DataSet();
                        da.Fill(ds, "aracKayit");
                        if (ds.Tables["aracKayit"].Rows.Count > 0)
                        {
                            serialPort1.WriteLine("4-1");
                            lbKapiCikis.Text = "Açık";
                            cmd.CommandText = "insert into kasaKayit(aracKayitNo,durulanZaman,alinanPara) values (@aracKayitNo,@durulanZaman,@alinanPara)";
                            cmd.Parameters.AddWithValue("@aracKayitNo", int.Parse(ds.Tables["aracKayit"].Rows[0]["id"].ToString()));
                            DateTime a = DateTime.Parse(ds.Tables["aracKayit"].Rows[0]["aracGiris"].ToString());
                            DateTime date = new DateTime(DateTime.Now.Ticks - a.Ticks);
                            int hour = ((date.DayOfYear - 1) * 24) + date.Hour;
                            cmd.Parameters.AddWithValue("@durulanZaman", hour);
                            cmd.Parameters.AddWithValue("@alinanPara", (hour * nmSaatlikFiyat.Value));
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "update aracKayit set aracCikisi = @cikis where id = @id";
                            cmd.Parameters.AddWithValue("@cikis", "1");
                            cmd.Parameters.AddWithValue("@id", int.Parse(ds.Tables["aracKayit"].Rows[0]["id"].ToString()));
                            cmd.ExecuteNonQuery();
                            da = new SqlDataAdapter("Select * From aracKayit where aracCikisi = @cikis", baglanti);
                            da.SelectCommand.Parameters.AddWithValue("@cikis", '0');
                            ds = new DataSet();
                            da.Fill(ds, "aracKayit");
                            cmbAracNumaralari.Items.Clear();
                            for (int i = 0; i < ds.Tables["aracKayit"].Rows.Count; i++)
                            {
                                lbToplamArac.Text = ds.Tables["aracKayit"].Rows.Count + "/200";
                                cmbAracNumaralari.Items.Add(ds.Tables["aracKayit"].Rows[i]["id"]);
                            }
                            da = new SqlDataAdapter("Select * From kasaKayit", baglanti);
                            ds = new DataSet();
                            da.Fill(ds, "kasaKayit");
                            int sonKayitIndex = ds.Tables["kasaKayit"].Rows.Count - 1;
                            int id = int.Parse(ds.Tables["kasaKayit"].Rows[sonKayitIndex]["aracKayitNo"].ToString());
                            string durulanZaman = ds.Tables["kasaKayit"].Rows[sonKayitIndex]["durulanZaman"].ToString();
                            string alinanPara = ds.Tables["kasaKayit"].Rows[sonKayitIndex]["alinanPara"].ToString();
                            da = new SqlDataAdapter("Select * From aracKayit where id = @id", baglanti);
                            da.SelectCommand.Parameters.AddWithValue("@id", id);
                            ds = new DataSet();
                            da.Fill(ds, "aracKayit");
                            string aracplaka = ds.Tables["aracKayit"].Rows[0]["aracPlaka"].ToString();
                            MessageBox.Show(this, aracplaka + " plakalı araç " + id.ToString() + " numarası ile " + durulanZaman + " saat otaparkta kaldığı için  " + alinanPara + " ücret ödeyerek " + DateTime.Now.ToString() + " tarhiyle otoparktan çıkış yapmıştır.", "Araç Çıkışı Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnAracCikisYap.Enabled = false;
                            tbCikisAracPlaka.Text = "";
                            cikisTimer.Stop();
                            cikisTimer.Start();
                            durumKayitYap();
                        }
                        else
                        {
                            MessageBox.Show(this, "Araç Plakası Bulunamadı", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        baglanti.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, "Araç Plakası Girilmemiş", "Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGirisKapisiAc_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("3-1");
            lbKapiGiris.Text = "Açık";
            girisTimer.Stop();
            girisTimer.Start();
        }

        private void btnCikisKapisiAc_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("4-1");
            lbKapiCikis.Text = "Açık";
            cikisTimer.Stop();
            cikisTimer.Start();
        }

        private void girisTimer_Tick(object sender, EventArgs e)
        {
            lbKapiGiris.Text = "Kapalı";
            girisTimer.Stop();
        }

        private void cikisTimer_Tick(object sender, EventArgs e)
        {
            lbKapiCikis.Text = "Kapalı";
            cikisTimer.Stop();
        }

        private void btnRaporAl_Click(object sender, EventArgs e)
        {
            RaporPage rp = new RaporPage();
            rp.mainPage = this;
            rp.Show();
            this.Hide();
        }

        private void OtoparkOtomasyon_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                dbLogTimer.Stop();
                serialPort1.Close();
                gbAracCikis.Enabled = false;
                gbAracGiris.Enabled = false;
                gbKatBir.Enabled = false;
                gbKatIki.Enabled = false;
                gbOtoparkBilgileri.Enabled = false;
                gbSistem.Enabled = false;
                cbSistem.Checked = false;
                lbSistem.Text = "Sistem Kapalı";
                lbSistem.ForeColor = Color.Red;
            }
            catch (Exception Err)
            {
                MessageBox.Show(this, Err.ToString(), "Bir hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPortSearchRefresh_Click(object sender, EventArgs e)
        {
            cmbPorts.Items.Clear();
            String[] port_names = SerialPort.GetPortNames();
            cmbPorts.Items.AddRange(port_names);
        }
    }
}
