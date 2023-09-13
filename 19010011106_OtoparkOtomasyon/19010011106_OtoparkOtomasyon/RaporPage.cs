using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.IO;

namespace _19010011106_OtoparkOtomasyon
{
    public partial class RaporPage : Form
    {
        public RaporPage()
        {
            InitializeComponent();
        }

        public OtoparkOtomasyon mainPage;
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-M7GIO33;Initial Catalog=otoparkOtomasyon;Integrated Security=True");
        SqlDataAdapter da;
        SqlCommand cmd;
        DataSet ds;

        private void RaporPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainPage.Show();
        }

        private void btnAracKayit_Click(object sender, EventArgs e)
        {
            ReportDocument rapor = new ReportDocument();
            //rapor.Load("E:\\GitHub\\VisualStudioProjects\\19010011106_OtoparkOtomasyon\\19010011106_OtoparkOtomasyon\\aracKayit.rpt");
            rapor.Load(Path.Combine(Environment.CurrentDirectory, @"../../aracKayit.rpt"));
            if (rbCYapmis.Checked) rapor.RecordSelectionFormula = "{aracKayit.aracCikisi} = '1'";
            else if (rbCYapmamis.Checked) rapor.RecordSelectionFormula = "{aracKayit.aracCikisi} = '0'";
            crystalReportViewer1.ReportSource = rapor;
        }

        private void btnkasaKayitlari_Click(object sender, EventArgs e)
        {
            ReportDocument rapor = new ReportDocument();
            //rapor.Load("E:\\GitHub\\VisualStudioProjects\\19010011106_OtoparkOtomasyon\\19010011106_OtoparkOtomasyon\\kasaKayit.rpt");
            rapor.Load(Path.Combine(Environment.CurrentDirectory, @"../../kasaKayit.rpt"));
            crystalReportViewer1.ReportSource = rapor;
        }

        private void btnDurumKayitlari_Click(object sender, EventArgs e)
        {
            ReportDocument rapor = new ReportDocument();
            //rapor.Load("E:\\GitHub\\VisualStudioProjects\\19010011106_OtoparkOtomasyon\\19010011106_OtoparkOtomasyon\\durumKayit.rpt");
            rapor.Load(Path.Combine(Environment.CurrentDirectory, @"../../durumKayit.rpt"));
            crystalReportViewer1.ReportSource = rapor;
        }

        private void btnLogKayitlari_Click(object sender, EventArgs e)
        {
            ReportDocument rapor = new ReportDocument();
            //rapor.Load("E:\\GitHub\\VisualStudioProjects\\19010011106_OtoparkOtomasyon\\19010011106_OtoparkOtomasyon\\logKayit.rpt");
            rapor.Load(Path.Combine(Environment.CurrentDirectory, @"../../logKayit.rpt"));
            crystalReportViewer1.ReportSource = rapor;
        }

        private void btnTarihAralikAracKayitlari_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from aracKayit where aracGiris >= @baslangic AND aracGiris <= @bitis", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value);
            da.SelectCommand.Parameters.AddWithValue("@bitis", dtpBitis.Value);
            ds = new DataSet();
            da.Fill(ds, "aracKayit");
            baglanti.Close();

            aracKayit ak = new aracKayit();
            ak.SetDataSource(ds.Tables["aracKayit"]);
            crystalReportViewer1.ReportSource = ak;
        }

        private void btnTarihAralikKasaKayitlari_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from kasaKayit where aracCikis >= @baslangic AND aracCikis <= @bitis", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value);
            da.SelectCommand.Parameters.AddWithValue("@bitis", dtpBitis.Value);
            ds = new DataSet();
            da.Fill(ds, "kasaKayit");
            baglanti.Close();

            kasaKayit kk = new kasaKayit();
            kk.SetDataSource(ds.Tables["kasaKayit"]);
            crystalReportViewer1.ReportSource = kk;
        }

        private void btnTariheGoreDurumKayitGetir_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where tarih >= @baslangic AND tarih <= @bitis", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@baslangic", dtpBaslangic.Value);
            da.SelectCommand.Parameters.AddWithValue("@bitis", dtpBitis.Value);
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnPlakayaGoreAracKayit_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from aracKayit where aracPlaka = @plaka", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@plaka", tbPlaka.Text);
            ds = new DataSet();
            da.Fill(ds, "aracKayit");
            baglanti.Close();

            aracKayit ak = new aracKayit();
            ak.SetDataSource(ds.Tables["aracKayit"]);
            crystalReportViewer1.ReportSource = ak;
        }

        private void btnPlakayaGoreKasaKayit_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from kasaKayit inner join aracKayit on kasaKayit.aracKayitNo = aracKayit.id where aracKayit.aracPlaka = @plaka", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@plaka", tbPlaka.Text);
            ds = new DataSet();
            da.Fill(ds, "kasaKayit");
            baglanti.Close();

            kasaKayit kk = new kasaKayit();
            kk.SetDataSource(ds.Tables["kasaKayit"]);
            crystalReportViewer1.ReportSource = kk;
        }

        private void btnFanDurumunaKayitGetir_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where fanDurum = @durum", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@durum", "Çalışıyor");
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnIsikDurumuKayitGetir_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where isikDurum = @durum", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@durum", "Çalışıyor");
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnYanginDurumuKayitGetir_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where yanginSensorDurum = @durum", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@durum", "Çalışıyor");
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnToplamDolulukBuyukturDeger_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where toplamDoluluk > @deger", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@deger", nmudBuyukturDeger.Value.ToString());
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnKatBirBuyukturDeger_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where dolulukKatBirDurum > @deger", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@deger", nmudBuyukturDeger.Value.ToString());
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }

        private void btnKatIkiBuyukturDeger_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand();
            baglanti.Open();
            cmd.Connection = baglanti;
            da = new SqlDataAdapter("select * from otoparkBilgileri where dolulukKatIkiDurum > @deger", baglanti);
            da.SelectCommand.Parameters.AddWithValue("@deger", nmudBuyukturDeger.Value.ToString());
            ds = new DataSet();
            da.Fill(ds, "otoparkBilgileri");
            baglanti.Close();

            durumKayit dk = new durumKayit();
            dk.SetDataSource(ds.Tables["otoparkBilgileri"]);
            crystalReportViewer1.ReportSource = dk;
        }
    }
}
