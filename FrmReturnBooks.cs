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
    public partial class FrmReturnBooks : Form
    {
        public FrmReturnBooks()
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
        ObjBook objBook = new ObjBook();

        private bool isStudentIDValid()
        {
            SqlCommand cmd = new SqlCommand($"Select * from StudentInfos where stID = '{txtStudentIDSearch.Text}'", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return (ds.Tables[0].Rows.Count > 0) ? true : false;
        }

        private void loadData()
        {
            SqlCommand cmd = new SqlCommand($"" +
                    $"Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', " +
                    $"issueDate as 'Issue Date', returnDate as 'Return Date' from IssueBooks as IB " +
                    $"join StudentInfos as SI ON IB.stID = SI.stID " +
                    $"join BookInfo as BI ON IB.bkID = BI.bkID " +
                    $"where IB.stID = {txtStudentIDSearch.Text} and returnDate is null", conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
                dgvBooksIssueInfo.DataSource = ds.Tables[0];
            else
                MessageBox.Show("Student return all books!!, input another student!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtStudentIDSearch.Text))
                {
                    MessageBox.Show("Input Student ID please!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtStudentIDSearch.Focus();
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                if(!isStudentIDValid())
                {
                    MessageBox.Show("Student ID invalid, input again!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SqlCommand cmd = new SqlCommand($"" +
                    $"Select IB.stID as ID, stName as 'Student Name', bkName as 'Book Name', " +
                    $"issueDate as 'Issue Date', returnDate as 'Return Date' from IssueBooks as IB " +
                    $"join StudentInfos as SI ON IB.stID = SI.stID " +
                    $"join BookInfo as BI ON IB.bkID = BI.bkID " +
                    $"where IB.stID = {txtStudentIDSearch.Text} and returnDate is null", conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                    dgvBooksIssueInfo.DataSource = ds.Tables[0];
                else
                    MessageBox.Show("No book issued!!, input another student!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isTextBoxEmpty(TextBox txt)
        {
            if (string.IsNullOrEmpty(txt.Text))
            {
                MessageBox.Show("Data cannot empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt.Focus();
                return true;
            }
            return false;
        }

        private void btnReturnBook_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty(txtBookName)) return;
            if (isTextBoxEmpty(txtStudentIDSearch)) return;


            try
            {
                int ID = objBook.BookID;
                int Quantity = objBook.BookQuantity;

                // Set return date of issue book
                if (ID != 0)
                {
                    SqlCommand cmd = new SqlCommand($"Update IssueBooks set returnDate = '{dtpReturnDate.Value}' " +
                                        $"where stID = {txtStudentIDSearch.Text} and bkID = {ID}", conn);
                    cmd.ExecuteNonQuery();

                    // update number of book in library
                    cmd.CommandText = $"Update BookInfo set bkQuantity = {Quantity + 1} where bkID = '{ID}'";
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Book Returned successfully.\r\nNew book quantity of {txtBookName.Text} = {Quantity + 1}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                
                    // reset book ID
                    objBook.BookID = 0;
                    txtBookName.ResetText();
                }
                else
                {
                    MessageBox.Show("Data invalid!\r\nInput another data please!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBooksIssueInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvBooksIssueInfo.CurrentRow.Index;
            if (dgvBooksIssueInfo.Rows[index].Cells[4].Value.ToString() == "" && dgvBooksIssueInfo.Rows[index].Cells[3].Value.ToString() != "")
            {
                txtBookName.Text = dgvBooksIssueInfo.Rows[index].Cells[2].Value.ToString();
                dtpIssueDate.Value = Convert.ToDateTime(dgvBooksIssueInfo.Rows[index].Cells[3].Value.ToString());
            }
            else return;

            SqlCommand cmd = new SqlCommand($"Select bkID, bkQuantity from BookInfo where bkName = '{txtBookName.Text}'", conn);
            SqlDataReader dr = cmd.ExecuteReader();

            
            objBook.BookName = txtBookName.Text;
            while(dr.Read())
            {
                objBook.BookID = Convert.ToInt32(dr["bkID"]);
                objBook.BookQuantity = dr.GetInt32(1);
            }
            dr.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtBookName.Text = txtStudentIDSearch.Text = "";
            dtpIssueDate.Value = dtpReturnDate.Value = DateTime.Now;
        }
    }
}
