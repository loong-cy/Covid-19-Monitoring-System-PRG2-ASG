using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    abstract class Person   // Person class is an abstract class as it contains an abstract method
    {

        // Properties
        private string name;
        private List<SafeEntry> safeEntryList;
        private List<TravelEntry> travelEntryList;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<SafeEntry> SafeEntryList
        {
            get { return safeEntryList; }
            set { safeEntryList = value; }
        }

        public List<TravelEntry> TravelEntryList
        {
            get { return travelEntryList; }
            set { travelEntryList = value; }
        }

        // Constructor
        public Person() { }

        public Person(string n)
        {
            Name = n;
            TravelEntryList = new List<TravelEntry>();  // Travel entry list
        }

        public void AddTravelEntry(TravelEntry t)
        {
            TravelEntryList.Add(t);
        }

        public void AddSafeEntry(SafeEntry s)
        {
            SafeEntryList = new List<SafeEntry>();  // Safe entry list
            SafeEntryList.Add(s);
        }

        // Abstract method
        public abstract double CalculateSHNCharges();

        public override string ToString()
        {
            return "Name: " + Name;
        }
    }
}
