using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class Visitor : Person
    {

        // Properties

        private string passportNo;
        private string nationality;
        public string PassportNo
        {
            get { return passportNo; }
            set { passportNo = value; }
        }
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        // Constructor
        public Visitor() : base() { }

        public Visitor(string n, string p, string nat) : base(n)
        {
            PassportNo = p;
            Nationality = nat;
        }

        public override double CalculateSHNCharges() // Calculate the amount the person has to pay for the respective SHN Mode
        {

            foreach (TravelEntry t in TravelEntryList)
            {
                if ((t.LastCountryOfEmbarkation == "New Zealand") || (t.LastCountryOfEmbarkation == "Vietnam") || (t.LastCountryOfEmbarkation == "Macao SAR"))
                {
                    return (80 + 200) * 1.07;   // Calculate the charges accordingly [SHN mode is none or 7-day SHN at own accomodation]
                }

                else
                {
                    return (200 + t.ShnStay.CalculateTravelCost(t.EntryMode, t.EntryDate) + 2000) * 1.07;   // Calculate charges for 14-day SHN at SDF
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return base.ToString() + "\tPassportNo: " + PassportNo + "\tNationality: " + Nationality;
        }
    }
}
