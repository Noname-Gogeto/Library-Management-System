using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Forms;
using System.Data.SqlClient;
namespace Library_Management_System
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
        
        private void btnLoginExit_Click(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            this.DialogResult = DialogResult.No;
        }
        private bool isTextBoxEmpty(HintTextBox txt, string text)
        {
            if (txt.Text == text)
            {
                MessageBox.Show("Data cannot empty!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt.Focus();
                return true;
            }
            return false;
        }

        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (isTextBoxEmpty(htxtUserName, "Username")) return;
            if (isTextBoxEmpty(htxtPass, "Password")) return;
          
            try
            {
                if(conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand($"Select * from Adlogin where adUserName = '{htxtUserName.Text}'" +
                                            $" and adPassword = '{htxtPass.Text}'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    conn.Close();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Incorrect Username or Password!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked) htxtPass.UseSystemPasswordChar = false;
            else htxtPass.UseSystemPasswordChar = true;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter) btnLogin.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
