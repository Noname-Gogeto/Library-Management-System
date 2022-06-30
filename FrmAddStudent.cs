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
    public partial class FrmAddStudent : Form
    {
        public FrmAddStudent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Function to close form and connection if it open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddStudentExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Function make Price textbox only accept digit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);

        /// <summary>
        /// Function to save student infomation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            foreach (Control item in palStudentInfo.Controls)
            {
                if (item is TextBox && string.IsNullOrEmpty(item.Text))
                {
                    MessageBox.Show("Data cannot empty!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    item.Focus();
                    return;
                }
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd2 = new SqlCommand($"Insert into StudentInfos values('{txtStudentID.Text}', '{txtStudentName.Text}', '{txtDepartment.Text}' ," +
                    $"'{txtSemester.Text}', '{txtContact.Text}', '{txtEmail.Text}')", conn);
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Student info saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Function to clear all data in form
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
    }
}
