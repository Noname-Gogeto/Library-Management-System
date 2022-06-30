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
    public partial class FrmBookDetail : Form
    {
        public FrmBookDetail()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);

        private void loadData(string strQuery, DataGridView table)
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
                MessageBox.Show("No book issued!!, input another student!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static string query = $"Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', bkAuthor as 'Author', " +
                    $"issueDate as 'Issue Date', returnDate as 'Return Date', bkQuantity as 'Quantity' from IssueBooks as IB " +
                    $"join StudentInfos as SI ON IB.stID = SI.stID " +
                    $"join BookInfo as BI ON IB.bkID = BI.bkID " +
                    $"where returnDate is";

        string issueInfo = query + " null";
        string returnInfo = query + " not null";

        private void FrmBookDetail_Load(object sender, EventArgs e)
        {
            
            loadData(issueInfo, dgvIssueBook);
            loadData(returnInfo, dgvReturnBook);
        }

        private bool isStudentIDValid()
        {
            SqlCommand cmd = new SqlCommand($"Select * from StudentInfos where stID = '{txtStudentIDSearch.Text}'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return (ds.Tables[0].Rows.Count > 0) ? true : false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentIDSearch.Text))
            {
                MessageBox.Show("Input Student ID please!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStudentIDSearch.Focus();
                return;
            }

            if (!isStudentIDValid())
            {
                MessageBox.Show("Student ID invalid, input again!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string stIssueInfo = issueInfo + $" and IB.stID = {txtStudentIDSearch.Text}";
            string stReturnInfo = returnInfo + $" and IB.stID = {txtStudentIDSearch.Text}";

            loadData(stIssueInfo, dgvIssueBook);
            loadData(stReturnInfo, dgvReturnBook);
           
        }

        private void txtStudentIDSearch_TextChanged(object sender, EventArgs e)
        {
            if(txtStudentIDSearch.Text.Length == 0)
                FrmBookDetail_Load(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtStudentIDSearch.ResetText();
        }
    }
}
