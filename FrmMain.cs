using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            this.Hide();
            if(frmLogin.ShowDialog() == DialogResult.OK)
            {
                FrmDashBoard dashBoard = new FrmDashBoard();
                dashBoard.ShowDialog();
            }
            this.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes) Application.Exit();
        }

        private void bookSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBookSearch frmBookSearch = new FrmBookSearch();
            this.Hide();
            frmBookSearch.ShowDialog();
            this.Show();
        }

        private void tsmiStudentLogin_Click(object sender, EventArgs e)
        {
            FrmUserLogin frmUserLogin = new FrmUserLogin();
            this.Hide();
            frmUserLogin.ShowDialog();
            this.Show();
        }
    }
}
