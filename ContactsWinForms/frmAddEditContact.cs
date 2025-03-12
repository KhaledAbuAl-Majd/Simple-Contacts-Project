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
    public partial class frmAddEditContact : Form
    {
        public enum enMode { AddNew, Update };
        private enMode _Mode;
        int _ContactID;
        clsContact _Contact;

        public frmAddEditContact(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;
            _Mode = (ContactID == -1) ? enMode.AddNew : enMode.Update;
        }
        void _FillCountriesInComboBox()
        {
            DataTable dtCountires = ClsCountry.GetAllCountries();
            
            foreach (DataRow row in dtCountires.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }
        private void _LoadDate()
        {
            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = 0;

            if(_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add new Contact";
               _Contact = new clsContact();
                return;
            }

            _Contact = clsContact.Find(_ContactID);

            if(_Contact == null)
            {
                MessageBox.Show("This form will be closed because No Cntact With This ID");
                this.Close();
                return;
            }

            lblMode.Text = "Edit Contact ID = " + _ContactID;
            lblContactID.Text = _ContactID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text = _Contact.Phone;
            txtAddress.Text = _Contact.Address;
            dtpDateOfBirth.Value= _Contact.DateOfBirth;

            //there
            if(_Contact.ImagePath != "")
            {
                pbImage.Load(_Contact.ImagePath);
            }

            llRemoveImage.Visible = (_Contact.ImagePath != "");
            //cbCountry.SelectedItem = ClsCountry.Find(_Contact.CountryID).CountryName; 
            cbCountry.SelectedIndex = cbCountry.FindString(ClsCountry.Find(_Contact.CountryID).CountryName);
        }
        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadDate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = ClsCountry.Find(cbCountry.Text).ID ;

            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.Address = txtAddress.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = CountryID;

            //there
            if (pbImage.ImageLocation != null)
                _Contact.ImagePath = pbImage.ImageLocation;
            else
                _Contact.ImagePath = "";

            if (_Contact.Save())
                MessageBox.Show("Data Saved Successfully.");
            else
                MessageBox.Show("Error: Data Is not saved Successfully.");

            _ContactID = _Contact.ID;
            _Mode = enMode.Update;
            lblMode.Text = "Edit Contact ID = " + _ContactID;
            lblContactID.Text = _ContactID.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
