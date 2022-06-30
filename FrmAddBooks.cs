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
    public partial class FrmAddBooks : Form
    {
        public FrmAddBooks()
        {
            InitializeComponent();
        }


        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);


        /// <summary>
        /// Function to get new book infomation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            foreach (Control item in palBookInfo.Controls)
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

                SqlCommand cmd = new SqlCommand("Select * from BookInfo", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                int ID = dt.Columns.Count + 2;

                SqlCommand cmd2 = new SqlCommand($"Insert into BookInfo values('{ID}', '{txtBookName.Text}', '{txtAuthorName.Text}' ," +
                    $"'{txtPublication.Text}', '{dtpPurchaseDate.Text}', '{txtPrice.Text}', '{txtBookQuantity.Text}')",conn);
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Book info saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Function exit form and close Sql connection if it open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddBooksExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                this.DialogResult = DialogResult.OK;
            }
        }


        /// <summary>
        /// Funtion refresh form data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            foreach (Control item in palBookInfo.Controls)
            {
                if (item is TextBox)
                    item.ResetText();
            }
            dtpPurchaseDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Function make Price textbox only accept digit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

    }
}
