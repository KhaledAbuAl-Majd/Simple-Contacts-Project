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
        private void _RefreshContactsList()
        {
            //automatic way form datatable
            dgvAllContacts.DataSource = clsContact.GetAllContacts();
        }

        private void frmListContacts_Load(object sender, EventArgs e)
        {
            _RefreshContactsList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditContact frmAddEditContact = new frmAddEditContact(-1);
            frmAddEditContact.ShowDialog();
            _RefreshContactsList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditContact frm = new frmAddEditContact((int)dgvAllContacts.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshContactsList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Contact [" + dgvAllContacts.CurrentRow.Cells[0].Value) == DialogResult.OK)
            {
                if (clsContact.DeleteContact((int)dgvAllContacts.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Contact Delete Successfully.");
                    _RefreshContactsList();
                }
                else
                    MessageBox.Show("Contact is not deleted!");
            }
        }
    }
}
