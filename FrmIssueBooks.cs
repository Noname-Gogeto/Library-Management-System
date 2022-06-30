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
    public partial class FrmIssueBooks : Form
    {
        public FrmIssueBooks()
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
        List<ObjBook> listBooks = new List<ObjBook>();

        private void FrmIssueBooks_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'libraryMangementSystemDataSet2.BookInfo' table. You can move, or remove it, as needed.
            //this.bookInfoTableAdapter.Fill(this.libraryMangementSystemDataSet2.BookInfo);
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand("Select bkID, bkName, bkQuantity from BookInfo", conn);

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    listBooks.Add(new ObjBook() { BookID = Convert.ToInt32(dr["bkId"]), BookName = dr.GetString(1), BookQuantity = Convert.ToInt32(dr["bkQuantity"]) });
                }

                foreach (var item in listBooks)
                {
                    cbxBookName.Items.Add(item);
                }

                dr.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            foreach (Control item in palIssueInfo.Controls)
            {
                if ((item is TextBox && string.IsNullOrEmpty(item.Text)) || cbxBookName.SelectedIndex == -1)
                {
                    MessageBox.Show("Data cannot empty.\r\nPlease input student information.\r\nUsing search engine to get exacly information in database!!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    item.Focus();
                    return;
                }
            }

            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                // Check student issued maximum box or not
                SqlCommand cmd = new SqlCommand($"Select count(stID) from IssueBooks where stID = {txtStudentIDSearch.Text} and returnDate is null", conn);
                SqlDataReader dr = cmd.ExecuteReader();
            
                while (dr.Read())
                {
                    if (dr.GetInt32(0) > 2)
                    {
                        MessageBox.Show("Student issued maximum book number!!\r\nReturn book to get new issue!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dr.Close();
                        return;
                    }
                }
                dr.Close();

                // Check number of book remain in library is Zero or not
                cmd.CommandText = $"Select bkQuantity from BookInfo where bkName = '{cbxBookName.Text}'";
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr.GetInt32(0) == 0)
                    {
                        MessageBox.Show("Book number remain is Zero!!\r\nPlease wait for next time!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dr.Close();
                        return;
                    }
                }
                dr.Close();

                // Issue book action and update quantity of book remain in library
                int bkID = (cbxBookName.SelectedItem as ObjBook).BookID;
                int bkQuantity = (cbxBookName.SelectedItem as ObjBook).BookQuantity;
                cmd.CommandText = $"Insert into IssueBooks values('{txtStudentIDSearch.Text}', '{bkID}', '{dtpIssueDate.Text}', null)";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"Update BookInfo set bkQuantity = {bkQuantity - 1} where bkID = {bkID}";
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Book Issued successfully.\r\nNew book quantity of {cbxBookName.Text} = {bkQuantity - 1}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStudentInfoSearch_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtStudentIDSearch.Text))
            {
                MessageBox.Show("Input Student ID please!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStudentIDSearch.Focus();
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand($"Select * from StudentInfos where stID = '{txtStudentIDSearch.Text}'", conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if(ds.Tables[0].Rows.Count > 0)
                {
                    int index = 0;
                    foreach (Control item in palIssueInfo.Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = ds.Tables[0].Rows[0][index++].ToString();
                        }
                        else break;
                    }
                }
                else
                {
                    MessageBox.Show("Student ID invalid, input again!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            foreach (Control item in palIssueInfo.Controls)
            {
                if (item is TextBox)
                    item.ResetText();
            }
            cbxBookName.SelectedIndex = -1;
            txtStudentIDSearch.ResetText();
        }

    }
}
