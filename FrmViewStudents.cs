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
    public partial class FrmViewStudents : Form
    {
        public FrmViewStudents()
        {
            InitializeComponent();
        }

        private void FrmViewStudents_Load(object sender, EventArgs e)
        {
            loadData(LoadDataQuery);
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
        string LoadDataQuery = $"Select stID as ID, stName as 'Student Name', stDepartment as Department, " +
            $"stSemester as Semester, stContact as Contact, stEmail as Email from StudentInfos";

        /// <summary>
        /// Function load data to sever to table
        /// </summary>
        /// <param name="strCommand"></param>
        private void loadData(string strCommand)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(strCommand, conn);

                //SqlDataReader dr = cmd.ExecuteReader();
                //DataTable dt = new DataTable();
                //dt.Load(dr);

                //dgvBookInfo.DataSource = dt;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvStudentInfo.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool isTextBoxEmpty()
        {
            foreach (Control item in palStudentInfo.Controls)
            {
                if (item is TextBox && string.IsNullOrEmpty(item.Text))
                {
                    MessageBox.Show("Data cannot empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    item.Focus();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Function to update from table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty()) return;

            try
            {
                if (MessageBox.Show("Data will be update.\r\nDo you want to confirm?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand($"Update StudentInfos set stName = '{txtStudentName.Text}', " +
                        $"stDepartment = '{txtDepartment.Text}', stSemester = '{txtSemester.Text}', stContact = '{txtContact.Text}', " +
                        $"stEmail = '{txtEmail.Text}' where stID = '{txtStudentID.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Row with ID = {txtStudentID.Text} was updated!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData(LoadDataQuery);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Function to delete data from sever
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteStudentInfo_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty()) return;

            try
            {
                if (MessageBox.Show("Data will be delete.\r\nDo you want to confirm?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand($"Delete from StudentInfos where stID = '{txtStudentID.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Row with ID = {txtStudentID.Text} was deleted!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData(LoadDataQuery);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        /// <summary>
        /// Function to clear all data in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            foreach (Control item in palStudentInfo.Controls)
            {
                if (item is TextBox)
                    item.ResetText();
            }
        }

        /// <summary>
        /// Function to search Student info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentIDSearch.Text))
            {
                MessageBox.Show("Input the book name you want to search please!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlSearch = "Select stID as ID, stName as 'Student Name', stDepartment as Department, " +
                $"stSemester as Semester, stContact as Contact, stEmail as Email " +
                $"from StudentInfos where stID like '%{txtStudentIDSearch.Text}%'";
            loadData(sqlSearch);
        }

        /// <summary>
        /// Function make textbox digital accept only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        /// <summary>
        /// Function to get data to textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStudentInfo_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvStudentInfo.CurrentRow.Index;
            int col = 0;
            foreach (Control item in palStudentInfo.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = dgvStudentInfo.Rows[index].Cells[col++].Value.ToString();
                }
            }
        }
        /// <summary>
        /// Function reset data when data search empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStudentIDSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudentIDSearch.Text)) loadData(LoadDataQuery);
        }
    }
}
