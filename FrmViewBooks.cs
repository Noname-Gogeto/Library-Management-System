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
    public partial class FrmViewBooks : Form
    {
        public FrmViewBooks()
        {
            InitializeComponent();
        }

        private void FrmViewBooks_Load(object sender, EventArgs e)
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
        string LoadDataQuery = "Select bkID as ID, bkName as 'Book Name', bkAuthor as 'Author', " +
                "bkPublication as 'Publication', bkDate as 'Date Publication',bkPrice as Price , bkQuantity as Quantity " +
                $"from BookInfo";
        int ID;


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
                dgvBookInfo.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool isTextBoxEmpty()
        {
            foreach (Control item in palBookInfo.Controls)
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

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty()) return;

            try
            {
                if(MessageBox.Show("Data will be update.\r\nDo you want to confirm?","Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
                {  
                    SqlCommand cmd = new SqlCommand($"Update BookInfo set bkName = '{txtBookName.Text}', bkAuthor = '{txtAuthorName.Text}', " +
                    $"bkPublication = '{txtPublication.Text}', bkDate = '{dtpPurchaseDate.Text}', bkPrice = '{txtPrice.Text}', " +
                    $"bkQuantity = '{txtBookQuantity.Text}' where bkName = '{txtBookName.Text}' and bkID = '{ID}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Row with ID = {ID} was updated!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData(LoadDataQuery);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            if (isTextBoxEmpty()) return;

            try
            {
                if (MessageBox.Show("Data will be delete.\r\nDo you want to confirm?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand($"Delete from BookInfo where bkID = '{ID}' and bkName = '{txtBookName.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Row with ID = {ID} was deleted!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData(LoadDataQuery);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Function to get book datas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBookInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dgvBookInfo.CurrentRow.Index;
            int col = 1;
            foreach (Control item in palBookInfo.Controls)
            {
                if(item is TextBox || item is DateTimePicker)
                {
                    item.Text = dgvBookInfo.Rows[index].Cells[col++].Value.ToString();
                }
            }           
            
            ID = Convert.ToInt32(dgvBookInfo.Rows[index].Cells[0].Value.ToString());
        }

        private void btnRefreshBookInfo_Click(object sender, EventArgs e)
        {
            foreach (Control item in palBookInfo.Controls)
            {
                if (item is TextBox)
                    item.ResetText();
            }
            dtpPurchaseDate.Value = DateTime.Now;
        }

        private void txtSearchInfo_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchInfo.Text)) loadData(LoadDataQuery);
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

        private void btnSearchBooks_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearchInfo.Text))
            {
                MessageBox.Show("Input the book name you want to search please!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlSearch = "Select bkID as ID, bkName as 'Book Name', bkAuthor as 'Author', " +
                "bkPublication as 'Publication', bkDate as 'Date Publication',bkPrice as Price , bkQuantity as Quantity " +
                $"from BookInfo where BkName like '%{txtSearchInfo.Text}%'";
            loadData(sqlSearch);
        }
    }
}
