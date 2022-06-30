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
    using CircularProgress = CircularProgressBar.CircularProgressBar;
    public partial class FrmDashBoard : Form
    {
        public FrmDashBoard()
        {
            InitializeComponent();
        }

        private void getForm(Form newForm)
        {
            this.Hide();
            newForm.ShowDialog();
            this.Show();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are u sure to quit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes) this.DialogResult = DialogResult.OK;
        }

        private void tsmiAddNewBooks_Click(object sender, EventArgs e)
        {
            FrmAddBooks frmAddBooks = new FrmAddBooks();
            getForm(frmAddBooks);
        }

        private void tsmiViewListBooks_Click(object sender, EventArgs e)
        {
            FrmViewBooks frmViewBooks = new FrmViewBooks();
            getForm(frmViewBooks);
        }

        private void tsmiNewStudent_Click(object sender, EventArgs e)
        {
            FrmAddStudent frmAddStudent = new FrmAddStudent();
            getForm(frmAddStudent);
        }

        private void tsmiStudentInfo_Click(object sender, EventArgs e)
        {
            FrmViewStudents frmViewStudents = new FrmViewStudents();
            getForm(frmViewStudents);
        }

        private void tsmiIssueBooks_Click(object sender, EventArgs e)
        {
            FrmIssueBooks frmIssueBooks = new FrmIssueBooks();
            getForm(frmIssueBooks);
        }

        private void tsmiReturnBooks_Click(object sender, EventArgs e)
        {
            FrmReturnBooks frmReturnBooks = new FrmReturnBooks();
            getForm(frmReturnBooks);
        }

        private void tsmiBooksDetail_Click(object sender, EventArgs e)
        {
            FrmBookDetail frmBookDetail = new FrmBookDetail();
            getForm(frmBookDetail);
        }

        private void tsmiLogOut_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }


        Random rd = new Random();
        private void getRandomProgress(CircularProgress cx)
        {
            int progressPercent = rd.Next(100);
            cx.Text = progressPercent + "%";
            cx.Value = progressPercent;
        }

        private void FrmDashBoard_Load(object sender, EventArgs e)
        {
            foreach (Control item in palProgress.Controls)
            {
                if (item is CircularProgress)
                    getRandomProgress((item as CircularProgress));
                else break;
            }
        }
    }
}
