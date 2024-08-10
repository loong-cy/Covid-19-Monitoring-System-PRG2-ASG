using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class BusinessLocation
    {
        // Properties
        private string businessName;
        private string branchCode;
        private int maximumCapacity;
        private int visitorsNow;

        public string BusinessName
        {
            get { return businessName; }
            set { businessName = value; }
        }

        public string BranchCode
        {
            get { return branchCode; }
            set { branchCode = value; }
        }

        public int MaximumCapacity
        {
            get { return maximumCapacity; }
            set { maximumCapacity = value; }
        }

        public int VisitorsNow
        {
            get { return visitorsNow; }
            set { visitorsNow = value; }
        }

        // Constructor
        public BusinessLocation() { }

        public BusinessLocation(string n, string c, int mc)
        {
            BusinessName = n;
            BranchCode = c;
            MaximumCapacity = mc;
        }

        public bool IsFull()    // Method to check capacity of business
        {
            if (VisitorsNow >= MaximumCapacity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "Business Name: " + BusinessName + "\tBranch Code: " + BranchCode + "\tMax Capacity: " + MaximumCapacity + "\tVisitors Now: " + VisitorsNow;
        }
    }
}