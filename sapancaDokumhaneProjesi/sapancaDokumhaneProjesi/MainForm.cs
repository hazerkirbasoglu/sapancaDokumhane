using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Common;
using System.Data.Odbc;

namespace sapancaDokumhaneProjesi
{
    public partial class MainForm : Form
    {
        public MainForm(Form f)
        {
            InitializeComponent();
            f.Close();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        int labelDegeri = 1;
        OleDbConnection conn;
       


        private void txtAnalizNo_KeyDown(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            // sadece sayısal girişe izin veren kod bloğu
            if (e.KeyChar == 13)
            {
              try
                {

                    CleanPanel(); // paneli diğer analiz sonuçlarından temizleyen kod
                    CreateBarcodOrQr(); // Barkod basan kod
                    DataFiller();  // girilen analiz nosuna göre panel2' yi dolduran kod

                    // Bağlantılı sorgu ile veri çekilmesi (Analiz no ile max-min değerler) Bir önceki sorgu , sorgunun version 2 'si aşağıdaki metodda.
                    /*           string sorgu = "SELECT AV.ItemName, AV.ItemValue, AH.AnalysisNo, AH.Quality, QN.QualityNo, QD.Dimension, QD.MaxIntern, QD.MinIntern, QD.MinExtern, QD.MaxExtern FROM(((dia_ANALYSISVALUES as AV INNER JOIN  dia_ANALYSISHEADER as AH ON AV.AnalysisNo = AH.AnalysisNo) INNER JOIN dia_QUALITYNAME as QN ON AH.Quality = QN.QualityName) INNER JOIN dia_QUALITYDETAILS as QD ON QN.QualityNo = QD.QualityNo) WHERE QD.ItemName = AV.ItemName AND AH.AnalysisNo =" + txtAnalizNo.Text + "and QD.QualityVersion=1";
                               da = new OleDbDataAdapter(sorgu, conn);

                               DataTable tablo = new DataTable();
                               da.Fill(tablo);
                               dataGridView1.DataSource = tablo;
                               */

                }
                catch(System.FormatException)
                {
                    MessageBox.Show("Sadece sayısal değer giriniz...");
                }
            }
        }

// panel2 alanına labelleri dolduran (çekilen datalara göre) kod parçası geriye bool karışım sonucu döndürür
        public bool  FillPage(long analizNo , List<string> ItemName, List<string> ItemValue, List<string> Quality, List<string> QualityNo, List<string> Dimension, List<string> MaxIntern, List<string> MinIntern, List<string> MinExtern, List<string> MaxExtern, List<string> SampleNo, List<string> DateTime, ArrayList IdentName, ArrayList IdentValue)
        {
            bool karisim_sonucu = true;
            int kacDeger = ItemName.Count;
            int sutunSayisi = (kacDeger / 12)+1;
            int i = 0, j = 0, k = 0, deger=0;             
            int sayi1 = 0, sayi2 = 25;
            int katsayi = 0;
            int loop_Dimension = 0, loop_ItemName = 0, loop_MaxExtern = 0, loop_MaxIntern = 0, loop_Value = 0, loop_MinIntern = 0, loop_MinExtern = 0;
            labelDegeri = 1;

            for (i=0; i<sutunSayisi; i++)
            {
                for(j=0; j<12; j++)
                {
                    if(deger<ItemName.Count)
                    { 
                    for (k = 0; k < 7; k++)
                    {                       
                            Label lb = new Label();
                            lb.Width = 70;
                            lb.Name = "label" + labelDegeri.ToString(); //kontrolün adı gerekli.
                            lb.Location = new Point((100 + sayi1), ((k+katsayi) * sayi2)); // form üzerinde yeri gerekli
                             if(k%7==0)
                            {
                                lb.Text = ItemName[loop_ItemName].ToString();
                                lb.Font = new Font(lb.Font, FontStyle.Bold);
                                loop_ItemName++;
                            }
                            else if (k%7==1)
                            {
                                lb.Text = Dimension[loop_Dimension].ToString();
                                loop_Dimension++;
                            }
                            else if (k%7==2)
                            {
                                lb.Text = MaxExtern[loop_MaxExtern].ToString();
                                loop_MaxExtern++;
                            }
                            else if (k%7==3)
                            {
                                lb.Text = MaxIntern[loop_MaxIntern].ToString();
                                loop_MaxIntern++;
                            }
                            else if (k%7==4)
                            {
                                lb.Text = ItemValue[loop_Value].ToString();
                                if (ItemValue[loop_Value] != "" && MaxIntern[loop_Value] != "" && MinIntern[loop_Value] != "")
                                {
                                    if (Convert.ToDouble(ItemValue[loop_Value]) > Convert.ToDouble(MaxIntern[loop_Value]) || Convert.ToDouble(ItemValue[loop_Value]) < Convert.ToDouble(MinIntern[loop_Value]))
                                    {
                                        lb.Font = new Font(lb.Font, FontStyle.Bold);
                                        lb.ForeColor = Color.Red;
                                        karisim_sonucu = false;
                                    }
                                }
                                lb.Font = new Font(lb.Font, FontStyle.Bold);
                                loop_Value++;
                            }
                            else if (k%7==5)
                            {
                                lb.Text = MinIntern[loop_MinIntern].ToString();
                                loop_MinIntern++;
                            }
                            else if (k%7==6)
                            {
                                lb.Text = MinExtern[loop_MinExtern].ToString();
                                loop_MinExtern++;
                            }

                            // Bu kodu tablodan gelen değerlerle değiştir !
                            panel2.Controls.Add(lb); // son olarak ekleme
                            labelDegeri++;                      
                    }

                    deger++;
                    sayi1 = sayi1 + 70;
                    }
                }
                katsayi = katsayi + 7;
                sayi1 = 0;
            }
            if(Quality.Count>0 && SampleNo.Count>0 && DateTime.Count>0)
              { 
            lblQualite.Text = Quality[0].ToString();
            lblQualite.Font = new Font(lblQualite.Font, FontStyle.Bold);

            lblSiparisNo.Text = SampleNo[0].ToString();
            lblSiparisNo.Font = new Font(lblSiparisNo.Font, FontStyle.Bold);

            lblTarih.Text = DateTime[0].ToString();
            lblTarih.Font = new Font(lblTarih.Font, FontStyle.Bold);
              }

            int indexName;
            if(IdentName.Count>0 && IdentValue.Count>0)
            {
                // FOSFIT AGI İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("FOSFIT A");
                if(indexName!=-1)
                {
                    lblFosfitAgi.Text = IdentValue[indexName].ToString();
                    lblFosfitAgi.Font = new Font(lblFosfitAgi.Font, FontStyle.Bold);
                }
                // Grafit yapısı İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("GRAFIT Y");
                if (indexName != -1)
                {
                    lblGrafitYapisi.Text = IdentValue[indexName].ToString();
                    lblGrafitYapisi.Font = new Font(lblGrafitYapisi.Font, FontStyle.Bold);
                }
                // Şarj No İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("ID-3");
                if (indexName != -1)
                {
                    lblSarjNo.Text = IdentValue[indexName].ToString();
                    lblSarjNo.Font = new Font(lblSarjNo.Font, FontStyle.Bold);
                }
                // Numune No İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("NUMUNE N");
                if (indexName != -1)
                {
                    lblNumuneNo.Text = IdentValue[indexName].ToString();
                    lblNumuneNo.Font = new Font(lblNumuneNo.Font, FontStyle.Bold);
                }
                // Tesis İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("TESIS");
                if (indexName != -1)
                {
                    lblTesis.Text = IdentValue[indexName].ToString();
                    lblTesis.Font = new Font(lblTesis.Font, FontStyle.Bold);
                }
                // Malzeme İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("ID-4");
                if (indexName != -1)
                {
                    lblMalzeme.Text = IdentValue[indexName].ToString();
                    lblMalzeme.Font = new Font(lblMalzeme.Font, FontStyle.Bold);
                }
                // Sertlik İÇİN DOLAN ALANLAR
                // SERTLİK BİLGİSİ ELLE GİRİLİR  SON ÖLÇÜMDEN SONRA
               
                // Matrix yapisi İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("MATRIX Y");
                if (indexName != -1)
                {
                    lblMatrixYapisi.Text = IdentValue[indexName].ToString();
                    lblMatrixYapisi.Font = new Font(lblMatrixYapisi.Font, FontStyle.Bold);
                }
                // Operator İÇİN DOLAN ALANLAR
                indexName = IdentName.IndexOf("OPERATOR");
                if (indexName != -1)
                {
                    lblOperator.Text = IdentValue[indexName].ToString();
                    lblOperator.Font = new Font(lblOperator.Font, FontStyle.Bold);
                }

            }
          
            return karisim_sonucu; 
        }
        
 //Barkod veya QR oluşturan Kod--değer ve barkod büyüklüğü ister      
        public void CreateBarcodOrQr()
        {
            // BARKOD OLUŞTURMA              
            /*       Zen.Barcode.Code39BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code39WithChecksum;
                   pictureBox1.Image = barcode.Draw(txtAnalizNo.Text, 50);  */

            // QR CODE oluşturma
            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            pictureBox1.Image = qrcode.Draw(txtAnalizNo.Text, 40);
            // bağlantı kapama ve textbox temizleme
            
        }

//panel2'yi temizleyen method
        public void CleanPanel()
        {
            for (int i = 1; i <= labelDegeri; i++)
            {
                panel2.Controls.RemoveByKey("label" + i.ToString());
            }
            lblSiparisNo.Text = "";
            lblTarih.Text = "";
            lblQualite.Text = "";
            lblGrafitYapisi.Text = "";
            lblFosfitAgi.Text = "";
            lblSarjNo.Text = "";
            lblTesis.Text = "";
            lblNumuneNo.Text = "";
            lblMalzeme.Text = "";
            lblMatrixYapisi.Text = "";
            lblOperator.Text = "";
            lblSertlik.Text = "";
        }

// veritabanından ilgili sorguya göre veriyi düzenli çeken kod parçası
//cevap olarak bool değer döner olumlu-olumsuz
        public void DataFiller()
        {
            long analizNo = Convert.ToInt64(txtAnalizNo.Text);


            // Cloud için connection string
            /*
            SqlConnection conn_Cloud = new SqlConnection();
            conn_Cloud = new SqlConnection("Data Source=hazerkirbasoglu.database.windows.net;Initial Catalog=diaDB;User ID=hazerkirbasoglu;Password=Iphone!9932001;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            

            SqlCommand cmd_Cloud = new SqlCommand();
            cmd_Cloud.Connection = conn_Cloud;
            cmd_Cloud.CommandType = CommandType.Text;
            cmd_Cloud.CommandText = "insert into dia_Properties ([AnalysisNo]) values (@analysisNo)";
   
            cmd_Cloud.Parameters.AddWithValue("@analysisNo", Convert.ToDouble(txtAnalizNo.Text));
            conn_Cloud.Open();

            cmd_Cloud.ExecuteNonQuery();
            conn_Cloud.Close();
            // Cloud için insert_into çalışması
            */
// TRY CATCH yapısı koyalım !
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=DIA.accdb");
            conn.Open();

            OleDbCommand cmd_MainQuery = new OleDbCommand("SELECT DD.LfdNummer, DD.DisplayNo, AH.Methode, AV.ItemName, AV.ItemValue, AH.AnalysisNo, AH.Quality, QN.QualityNo, QD.Dimension, QD.MaxIntern, QD.MinIntern, QD.MinExtern, QD.MaxExtern, QD.QualityVersion, AH.SampleNo, AH.DIADateTime FROM(((((dia_ANALYSISVALUES as AV INNER JOIN  dia_ANALYSISHEADER as AH ON AV.AnalysisNo = AH.AnalysisNo) INNER JOIN dia_QUALITYNAME as QN ON AH.Quality = QN.QualityName) INNER JOIN dia_QUALITYDETAILS as QD ON QN.QualityNo = QD.QualityNo) INNER JOIN dia_DISPLAYHEADER as DH ON DH.DisplayName = AH.Methode) INNER JOIN dia_DISPLAYDETAIL as DD ON DH.DisplayNo = DD.DisplayNo) WHERE QD.ItemName = AV.ItemName AND DD.ItemName = AV.ItemName AND AV.AnalysisNo ="+analizNo+" AND QualityVersion = ( SELECT MAX(QualityVersion) FROM(((dia_ANALYSISVALUES as AV INNER JOIN  dia_ANALYSISHEADER as AH ON AV.AnalysisNo = AH.AnalysisNo) INNER JOIN dia_QUALITYNAME as QN ON AH.Quality = QN.QualityName) INNER JOIN dia_QUALITYDETAILS as QD ON QN.QualityNo = QD.QualityNo) where AV.AnalysisNo ="+analizNo+" ) ORDER BY LfdNummer asc", conn);

            OleDbDataReader dr_MainQuery = cmd_MainQuery.ExecuteReader();

            //dizi yerine generic koleksiyon
            List<string> LfdNummer  = new List<string>();
            List<string> DisplayNo  = new List<string>();
            List<string> Methode    = new List<string>();
            List<string> ItemName   = new List<string>();
            List<string> ItemValue  = new List<string>();
            List<string> Quality    = new List<string>();
            List<string> QualityNo  = new List<string>();
            List<string> Dimension  = new List<string>();
            List<string> MaxIntern  = new List<string>();
            List<string> MinIntern  = new List<string>();
            List<string> MinExtern  = new List<string>();
            List<string> MaxExtern  = new List<string>();
            List<string> SampleNo = new List<string>();
            List<string> DateTime = new List<string>();

            while (dr_MainQuery.Read())
            {
                ItemName.Add(dr_MainQuery["ItemName"].ToString());
                Quality.Add(dr_MainQuery["Quality"].ToString());
                QualityNo.Add(dr_MainQuery["QualityNo"].ToString());
                Dimension.Add(dr_MainQuery["Dimension"].ToString());
                ItemValue.Add(dr_MainQuery["ItemValue"].ToString());
                MaxIntern.Add(dr_MainQuery["MaxIntern"].ToString());
                MinIntern.Add(dr_MainQuery["MinIntern"].ToString());
                MinExtern.Add(dr_MainQuery["MinExtern"].ToString());
                MaxExtern.Add(dr_MainQuery["MaxExtern"].ToString());
                SampleNo.Add(dr_MainQuery["SampleNo"].ToString());
                DateTime.Add(dr_MainQuery["DIADateTime"].ToString());
            }

// Burada diğer sorgu çalışır ! Belgede olan bilgiler burada dolar

            OleDbCommand cmd_Ident = new OleDbCommand("SELECT * from  dia_ANALYSISIDENT where AnalysisNo ="+analizNo, conn);
            OleDbDataReader dr_Ident = cmd_Ident.ExecuteReader();

            ArrayList IdentName = new ArrayList();
            ArrayList IdentValue = new ArrayList();

            while (dr_Ident.Read())
            {
                IdentName.Add(dr_Ident["IdentName"].ToString());
                IdentValue.Add(dr_Ident["IdentValue"].ToString());
            }

  // Doldurulcak bilgiler gönderilir
                bool cevap = FillPage(analizNo, ItemName, ItemValue, Quality, QualityNo, Dimension, MaxIntern, MinIntern, MinExtern, MaxExtern, SampleNo, DateTime, IdentName, IdentValue);
  // FillPage Methodu burdaki değerler ile panel2'ye label basar
            if (cevap == false)
            {
                MessageBox.Show("SIKINTILI DURUMLAR VAR !!!");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

       
    }
}
