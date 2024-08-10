using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class SafeEntry
    {
        // Properties
        private DateTime checkIn;
        private DateTime checkOut;
        private BusinessLocation location;

        public DateTime CheckIn
        {
            get { return checkIn; }
            set { checkIn = value; }
        }

        public DateTime CheckOut
        {
            get { return checkOut; }
            set { checkOut = value; }
        }

        public BusinessLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        // Constructor
        public SafeEntry() { }

        public SafeEntry(DateTime ci, BusinessLocation l)
        {
            CheckIn = ci;
            Location = l;
        }

        public void PerformCheckOut()   // Method to perform check-out
        {
            checkOut = DateTime.Now;
        }

        public override string ToString()
        {
            return "Check In: " + CheckIn + "\tCheck Out: " + CheckOut + "\tLocation: " + Location;
        }
    }
}
