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


namespace Library_Management_System
{
    public partial class FrmStudentDashboard : Form
    {
        public FrmStudentDashboard()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);
        static string query = $"Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', bkAuthor as 'Author', " +
                    $"issueDate as 'Issue Date', returnDate as 'Return Date', bkQuantity as 'Quantity' from IssueBooks as IB " +
                    $"join StudentInfos as SI ON IB.stID = SI.stID " +
                    $"join BookInfo as BI ON IB.bkID = BI.bkID " +
                    $"where returnDate is";
        private void loadData(string strQuery, DataGridView table, string status)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            SqlCommand cmd = new SqlCommand(strQuery, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
                table.DataSource = ds.Tables[0];
            else
                MessageBox.Show($"Student no {status} book!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void getStudentInfo()
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            SqlCommand cmd = new SqlCommand($"Select * from StudentInfos where stID = '{this.Tag}'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            
            int index = 0;
            foreach (Control item in palStudentInfo.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = ds.Tables[0].Rows[0][index++].ToString();
                }
                else
                    break;
            }
            
        }

        private void FrmStudentDashboard_Load(object sender, EventArgs e)
        {
            string issueQuery = query + $" null and IB.stID = {this.Tag}";
            string returnQuery = query + $" not null and IB.stID = {this.Tag}";

            loadData(issueQuery, dgvIssueBook, "issued");
            loadData(returnQuery, dgvReturnBook, "returned");
            getStudentInfo();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
