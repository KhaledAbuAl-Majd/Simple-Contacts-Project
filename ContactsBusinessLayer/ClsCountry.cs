using System;
using System.Data;
using ContactsDataAccessLayer;

namespace ContactsBusinessLayer
{
    public class ClsCountry
    {
        public enum enMode { AddNew, Update };

        public enMode Mode = enMode.AddNew;
        public int ID { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }

        public ClsCountry()
        {
            this.ID = -1;
            this.CountryName = "";
            this.Code = "";
            this.PhoneCode = "";
            this.Mode = enMode.AddNew;
        }
        private ClsCountry(int CountryID,string CountryName,string Code,string PhoneCode)
        {
            this.ID = CountryID;
            this.CountryName = CountryName;
            this.Code = Code;
            this.PhoneCode = PhoneCode;
            this.Mode = enMode.Update;
        }

        private bool _AddNewCountry()
        {
            this.ID = ClsCountryData.AddNewCountry(this.CountryName,this.Code,this.PhoneCode);

            return (this.ID != -1);
        }
        private bool _UpdateCountry()
        {
            return ClsCountryData.UpdateCountry(this.ID, this.CountryName, this.Code, this.PhoneCode);
        }
        public static ClsCountry Find(int CountryID)
        {
            string CountryName = "", Code = "", PhoneCode = "";

            if (ClsCountryData.GetCountryInforByID(CountryID, ref CountryName,ref Code,ref PhoneCode))
                return new ClsCountry(CountryID, CountryName,Code,PhoneCode);
            else
                return null;
        }
        public static ClsCountry Find(string CountryName)
        {
            int CountryID = -1;
            string Code = "", PhoneCode = "";

            return (ClsCountryData.GetCountryInfoByName(CountryName,ref CountryID,ref Code,ref PhoneCode)) ? 
                new ClsCountry(CountryID, CountryName,Code,PhoneCode) : null;
        }
        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateCountry();
            }

            return false;
        }
        public static DataTable GetAllCountries()
        {
            return ClsCountryData.GetAllCountries();
        }
        public static bool DeleteCountry(int CountryID)
        {
            return ClsCountryData.DeleteCountry(CountryID);
        }
        public static bool IsCountryExist(int CountryID)
        {
            return ClsCountryData.IsCountryExist(CountryID);
        }
        public static bool IsCountryExist(string CountryName)
        {
            return ClsCountryData.IsCountryExist(CountryName);
        }

    }
}
