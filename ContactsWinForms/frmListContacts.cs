using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContactsBusinessLayer;

namespace ContactsWinForms
{
    public partial class frmListContacts : Form
    {
        public frmListContacts()
        {
            InitializeComponent();
        }

        void _RefereshContactList()
        {
            dgvListContacts.DataSource = clsContact.GetAllContacts();
        }
        private void frmListContacts_Load(object sender, EventArgs e)
        {
            _RefereshContactList();
        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            //-1 mean add new contact
            frmAddEditContact frm = new frmAddEditContact(-1);
            frm.ShowDialog();
            _RefereshContactList();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditContact frm = new frmAddEditContact((int)dgvListContacts.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefereshContactList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this Contact?","Confirm",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsContact.DeleteContact((int)dgvListContacts.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Contact Deleted Successfully.");
                    _RefereshContactList();
                }
                else
                    MessageBox.Show("Delete Contact Failed!");
            }
        }
    }
}
