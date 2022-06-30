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
    public partial class FrmBookSearch : Form
    {
        public FrmBookSearch()
        {
            InitializeComponent();
        }

        private void FrmBookSearch_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'libraryMangementSystemDataSet1.BookInfo' table. You can move, or remove it, as needed.
            //this.bookInfoTableAdapter.Fill(this.libraryMangementSystemDataSet1.BookInfo);
            loadData(sqlLoad);
        }

        /// <summary>
        /// 3 atribute to get connection off form
        /// </summary>
        static String ConnectStr = @"Data Source=LAPTOP-FD9VR33M\EMANONSQLSEVER;Initial Catalog=LibraryMangementSystem;Integrated Security=True";
        SqlConnection conn = new SqlConnection(ConnectStr);
        string sqlLoad = "Select bkName as 'Book Name', bkAuthor as 'Author', " +
                "bkPublication as 'Publication', bkDate as 'Date Publication', bkQuantity as Quantity " +
                $"from BookInfo";

        /// <summary>
        /// Function to load data to form with condition is strCommand
        /// </summary>
        /// <param name="strCommand"></param>
        private void loadData(string strCommand)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(strCommand, conn);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvBooksInfo.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
                    
        }
        
        /// <summary>
        /// Exit click close connection open and form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exit_Click(object sender, EventArgs e)
        {
            if(conn.State == ConnectionState.Open)
                conn.Close();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// button Search to get result of book user want
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBookName.Text))
            {
                MessageBox.Show("Input the book name you want to search please!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string sqlSearch = "Select bkName as 'Book Name', bkAuthor as 'Author'," +
                "bkPublication as 'Publication', bkDate as 'Date Publication', bkQuantity as Quantity " +
                $"from BookInfo where BkName like '%{txtBookName.Text}%'";
            loadData(sqlSearch);
        }

        private void txtBookName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBookName.Text)) loadData(sqlLoad);
        }

        /// <summary>
        /// Function to show suggest when mouse hover to textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBookName_MouseHover(object sender, EventArgs e)
        {
            ttpBookSuggest.SetToolTip(txtBookName, "Input book name please!!");
        }
    }
}
