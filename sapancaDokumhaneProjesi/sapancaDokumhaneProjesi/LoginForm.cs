using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace sapancaDokumhaneProjesi
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            if(txtKullaniciAdi.Text=="hazer" && txtSifre.Text=="123")
            {
                MainForm f2 = new MainForm(this);
                f2.Show();
                this.Close();
            }
            

        }

  

        
    }
}
