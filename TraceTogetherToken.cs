using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class TraceTogetherToken
    {
        // Properties
        private string serialNo;
        private string collectionLocation;
        private DateTime expiryDate;

        public string SerialNo
        {
            get { return serialNo; }
            set { serialNo = value; }
        }

        public string CollectionLocation
        {
            get { return collectionLocation; }
            set { collectionLocation = value; }
        }

        public DateTime ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        // Constructor
        public TraceTogetherToken() { }

        public TraceTogetherToken(string s, string cl, DateTime ed)
        {
            SerialNo = s;
            CollectionLocation = cl;
            ExpiryDate = ed;
        }

        public bool IsElligibleForReplacement() // Method to check elligibility for replacement of token
        {
            if (DateTime.Now <= ExpiryDate && ExpiryDate.AddMonths(-1) <= DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReplaceToken(string s, string cl)   // Method replace resident token
        {
            SerialNo = s;
            CollectionLocation = cl;
        }

        public override string ToString()
        {
            return base.ToString() + "\tSerial No: " + serialNo + "\tCollection Location" + CollectionLocation + "\tExpiry Date: " + ExpiryDate;
        }
    }
}
