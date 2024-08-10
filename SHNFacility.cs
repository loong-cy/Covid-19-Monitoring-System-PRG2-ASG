using System;
using System.Collections.Generic;
using System.Text;

namespace Prg2_T09_Team7
{
    class SHNFacility
    {
        // Proprties
        private string facilityName;
        private int facilityCapacity;
        private int facilityVacancy;
        private double distFromAirCheckpoint;
        private double distFromSeaCheckpoint;
        private double distFromLandCheckpoint;
        public string FacilityName
        {
            get { return facilityName; }
            set { facilityName = value; }
        }
        public int FacilityCapacity
        {
            get { return facilityCapacity; }
            set { facilityCapacity = value; }
        }
        public int FacilityVacany
        {
            get { return facilityVacancy; }
            set { facilityVacancy = value; }
        }
        public double DistFromAirCheckpoint
        {
            get { return distFromAirCheckpoint; }
            set { distFromAirCheckpoint = value; }
        }
        public double DistFromSeaCheckpoint
        {
            get { return distFromSeaCheckpoint; }
            set { distFromSeaCheckpoint = value; }
        }
        public double DistFromLandCheckpoint
        {
            get { return distFromLandCheckpoint; }
            set { distFromLandCheckpoint = value; }
        }

        // Constructor
        public SHNFacility() { }

        public SHNFacility(string fn, int fc, double ac, double sc, double lc)
        {
            FacilityName = fn;
            FacilityCapacity = fc;
            DistFromAirCheckpoint = ac;
            DistFromSeaCheckpoint = sc;
            DistFromLandCheckpoint = lc;
        }

        // Method to calculate travel cost based on the entry mode
        public double CalculateTravelCost(string EntryMode, DateTime EntryDate)
        {
            if (EntryMode == "Air")
            {
                double fare = 50 + (DistFromAirCheckpoint * 0.22);  // base fare
                if ((EntryDate.Hour >= 6 && EntryDate.Hour < 9) || (EntryDate.Hour >= 18 && EntryDate.Hour < 0))   // Between 6 am to 8.59 am or 6 pm to 11.59 pm
                {
                    return fare * 1.25; // Additional 25% surchase from base fare
                }
                else if (EntryDate.Hour >= 0 && EntryDate.Hour < 6) // Midnight to 5.59am
                {
                    return fare * 1.50; // Additional 50% surchase from base fare
                }
                else
                {
                    return fare;    // Base fare
                }
            }
            else if (EntryMode == "Land")
            {
                double fare = 50 + (DistFromLandCheckpoint * 0.22);
                if ((EntryDate.Hour >= 6 && EntryDate.Hour < 9) || (EntryDate.Hour >= 18 && EntryDate.Hour < 0))
                {
                    return fare * 1.25;
                }
                else if (EntryDate.Hour >= 0 && EntryDate.Hour < 6)
                {
                    return fare * 1.50;
                }
                else
                {
                    return fare;
                }
            }
            else if (EntryMode == "Sea")
            {
                double fare = 50 + (DistFromSeaCheckpoint * 0.22);
                if ((EntryDate.Hour >= 6 && EntryDate.Hour < 9) || (EntryDate.Hour >= 18 && EntryDate.Hour < 0))
                {
                    return fare * 1.25;
                }
                else if (EntryDate.Hour >= 0 && EntryDate.Hour < 6)
                {
                    return fare * 1.50;
                }
                else
                {
                    return fare;
                }
            }
            else
            {
                return 0;
            }
        }

        // Method to check if the selected facility has enough space for visitors
        public bool IsAvailable()
        {
            if (FacilityVacany == 0)
            {
                return false;
            }
            else
            {
                FacilityCapacity -= 1;  // Reduce the Capacity count by 1
                return true;
            }
        }

        public override string ToString()
        {
            return "Facility Name: " + FacilityName + "\tFacility capacity: " + FacilityCapacity + "\tFacility vacancy: " + FacilityVacany + "\tDistFromAirCheckPoint: "
                + DistFromAirCheckpoint + "\tDistFromSeaCheckpoint: " + DistFromSeaCheckpoint + "\tDistFromLandCheckpoint: " + DistFromLandCheckpoint;
        }

    }
}
