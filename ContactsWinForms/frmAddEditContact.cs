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
using static System.Net.Mime.MediaTypeNames;

namespace ContactsWinForms
{
    public partial class frmAddEditContact : Form
    {
        public enum enMode { AddNew, Update };
        enMode _Mode;
        int _ContactID;
        clsContact _Contact;
        public frmAddEditContact(int ContactID)
        {
            InitializeComponent();

            _ContactID = ContactID;
            _Mode = (ContactID == -1) ? enMode.AddNew : enMode.Update;
        }

        void _ChangeRemoveLinkLableVisablity(bool Value)
        {
            llRemoveImage.Visible = Value;
        }
        void _ChangeModeLabel(string Text)
        {
            lblMode.Text = Text;
        }
        void _AddNewMode()
        {
            _Contact = new clsContact();
            _ChangeModeLabel("Add New Contact");
            _ChangeRemoveLinkLableVisablity(false);
        }
        void _RemoveImage()
        {        
            pbImage.ImageLocation = "";
        }
        void _ShowImage(string ImagePath)
        {
            if (ImagePath == "")
                return;

            if (ImagePath != null)
            {
                try
                {
                    pbImage.Load(ImagePath);
                    _ChangeRemoveLinkLableVisablity(true);
                }
                catch
                {
                    MessageBox.Show("Image Path is Not Correct!");
                    _ChangeRemoveLinkLableVisablity(false);
                }
            }
        }
        void _FillContactDataInFromFromObject()
        {
            lblContactID.Text = _Contact.ID.ToString();
            txtFirstName.Text = _Contact.FirstName;
            txtLastName.Text = _Contact.LastName;
            txtEmail.Text = _Contact.Email;
            txtPhone.Text = _Contact.Phone;
            dtpDateOfBirth.Value = _Contact.DateOfBirth;
            cbCountry.SelectedIndex = cbCountry.FindString(ClsCountry.Find(_Contact.CountryID).CountryName);
            txtAddress.Text = _Contact.Address;
            _ShowImage(_Contact.ImagePath);
        }
        void _UpdateMode()
        {
            _Contact = clsContact.Find(_ContactID);

            if(_Contact == null)
            {
                MessageBox.Show("Contact With ID = " + _ContactID + " Is Not Found!");
                this.Close();
                return;
            }

            _ChangeModeLabel("Update Contact ID = " + _ContactID);
            _FillContactDataInFromFromObject();
           
        }
        void _FillCountriesInComboBox()
        {
            DataTable dtCountires = ClsCountry.GetAllCountries();

            foreach(DataRow row in dtCountires.Rows)
            {
                cbCountry.Items.Add(row["CountryName"].ToString());
            }
        }
        void _LoadData()
        {
            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = 0;

            switch (_Mode)
            {
                case enMode.AddNew:
                    _AddNewMode();
                    break;

                case enMode.Update:
                    _UpdateMode();
                    break; ;
            }
        }
        void _PutDataInContactObjectFromForm()
        {
            _Contact.FirstName = txtFirstName.Text;
            _Contact.LastName = txtLastName.Text;
            _Contact.Email = txtEmail.Text;
            _Contact.Phone = txtPhone.Text;
            _Contact.DateOfBirth = dtpDateOfBirth.Value;
            _Contact.CountryID = ClsCountry.Find(cbCountry.Text).ID;
            _Contact.Address = txtAddress.Text;
            _Contact.ImagePath = (pbImage.ImageLocation != null) ? pbImage.ImageLocation : "";
        }
        void _SaveNewContact()
        {
            _PutDataInContactObjectFromForm();

            if (_Contact.Save())
            {
                _Mode = enMode.Update;
                _ChangeModeLabel("Update Contact ID = " + _Contact.ID);
                lblContactID.Text = _Contact.ID.ToString();
                MessageBox.Show("Contact Addes Successfully.");
            }
            else
            {
                MessageBox.Show("Save Failed!");
            }
        }
        void _UpdateExistContact()
        {
            _PutDataInContactObjectFromForm();

            if (_Contact.Save())
            {
                MessageBox.Show("Contact Updated Successfully.");
            }
            else
            {
                MessageBox.Show("Save Failed!");
            }
        }
        void _Save()
        {
            if (!ValidateChildren())
                return;

            switch (_Mode)
            {
                case enMode.AddNew:
                    _SaveNewContact();
                    break;

                case enMode.Update:
                    _UpdateExistContact();
                    break;
            }
        }
        void _ErrorProviderForTextBox(TextBox txt,CancelEventArgs e,string ErrorText)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                e.Cancel = true;
                txt.Focus();
                errorProvider1.SetError(txt, ErrorText);
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txt, "");
            }
        }
        private void frmAddEditContact_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _RemoveImage();
        }

        private void llOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = ("Image Files (*.jpg;*.png)|*.jpg;*.png");
            openFileDialog1.Title = "Set Image";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _ShowImage(openFileDialog1.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Save();
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            _ErrorProviderForTextBox((TextBox)sender, e, "FirstName Should have a value!");
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            _ErrorProviderForTextBox((TextBox)sender, e, "LastName Should have a value!");
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            _ErrorProviderForTextBox((TextBox)sender, e, "Email Should have a value!");
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            _ErrorProviderForTextBox((TextBox)sender, e, "Phone Should have a value!");
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            _ErrorProviderForTextBox((TextBox)sender, e, "Address Should have a value!");
        }

        private void cbCountry_Validating(object sender, CancelEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            if (string.IsNullOrWhiteSpace(cb.Text))
            {
                e.Cancel = true;
                cb.Focus();
                errorProvider1.SetError(cb, "Country Should have a value!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(cb, "");
            }
        }
    }
}
