using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class TravelEntry
    {
        // Properties
        private string lastCountryOfEmbarkation;
        private string entryMode;
        private DateTime entryDate;
        private DateTime shnEndDate;
        private SHNFacility shnStay;
        private bool isPaid;
        public string LastCountryOfEmbarkation
        {
            get { return lastCountryOfEmbarkation; }
            set { lastCountryOfEmbarkation = value; }
        }
        public string EntryMode
        {
            get { return entryMode; }
            set { entryMode = value; }
        }
        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }
        public DateTime ShnEndDate
        {
            get { return shnEndDate; }
            set { shnEndDate = value; }
        }
        public SHNFacility ShnStay
        {
            get { return shnStay; }
            set { shnStay = value; }
        }

        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; }
        }

        // Constructor
        public TravelEntry() { }

        public TravelEntry(string l, string em, DateTime ed)
        {
            LastCountryOfEmbarkation = l;
            EntryMode = em;
            EntryDate = ed;
        }

        // Method use in assigning person to SHNFacility
        public void AssignSHNFacility(SHNFacility f)
        {
            ShnStay = f;
        }

        // Method use to calculate the period of time person stay in SHN Facility
        public DateTime CalculateSHNDuration() // DateTime is used instead of void as it is returning datetime value
        {
            if ((LastCountryOfEmbarkation == "New Zealand") || (LastCountryOfEmbarkation == "Vietnam"))
            {
                return EntryDate.AddDays(0);
            }
            else if (LastCountryOfEmbarkation == "Macao SAR")
            {
                return EntryDate.AddDays(7);
            }
            else
            {
                return EntryDate.AddDays(14);
            }
        }

        public override string ToString()
        {
            return "Last country of embarkation:" + LastCountryOfEmbarkation + "\tEntry Mode: " + EntryMode + "\tEntryDate" + EntryDate + "\tSHN End date: "
                + ShnEndDate + "\tSHN stay: " + ShnStay;
        }
    }
}
