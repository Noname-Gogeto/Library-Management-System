using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Windows.Forms;

namespace Library_Management_System
{
    public partial class FrmUserLogin : Form
    {
        public FrmUserLogin()
        {
            InitializeComponent();
        }

        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty(htxtStudentName, "Username")) return;
            if (isTextBoxEmpty(htxtID, "Password")) return;

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand($"Select * from StudentInfos where stID = '{htxtID.Text}'" +
                                            $" and stName = '{htxtStudentName.Text}'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    conn.Close();
                    FrmStudentDashboard frmStudentDashboard = new FrmStudentDashboard();
                    this.Hide();
                    frmStudentDashboard.Tag = htxtID.Text;
                    frmStudentDashboard.ShowDialog();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Invalid username or ID!!\r\nPlease input again!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoginExit_Click(object sender, EventArgs e)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            this.DialogResult = DialogResult.No;
        }

        private void FrmUserLogin_Load(object sender, EventArgs e)
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
