using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class Resident : Person
    {
        // Properties
        private string address;
        private DateTime lastLeftCountry;
        private TraceTogetherToken token;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public DateTime LastLeftCountry
        {
            get { return lastLeftCountry; }
            set { lastLeftCountry = value; }
        }

        public TraceTogetherToken Token
        {
            get { return token; }
            set { token = value; }
        }

        // Constructor
        public Resident() { }

        public Resident(string n, string a, DateTime lc) : base(n)
        {
            Address = a;
            LastLeftCountry = lc;
        }

        public override double CalculateSHNCharges()    // Method to calculate SHN charges
        {
            foreach (TravelEntry t in TravelEntryList)
            {
                if ((t.LastCountryOfEmbarkation == "New Zealand") || (t.LastCountryOfEmbarkation == "Vietnam"))
                {
                    return 200 * 1.07;
                }

                else if (t.LastCountryOfEmbarkation == "Macao SAR")
                {
                    return (200 + 20) * 1.07;
                }

                else
                {
                    return (200 + 20 + 1000) * 1.07;
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return base.ToString() + "\tAddress: " + Address + "\tLast left country: " + LastLeftCountry;
        }
    }
}
