using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;

//============================================================
// Student Number : S10208432, S10205467
// Student Name : Tay Xin Ying, Loong Chor Yi
// Module Group : T09
//============================================================

namespace Prg2_T09_Team7
{
    class Program
    {
        static void Main(string[] args)
        {

            // Creating a list to load person and business location data
            List<Person> pList = new List<Person>();
            List<BusinessLocation> bList = new List<BusinessLocation>();

            // Load SHN Facility Data 
            List<SHNFacility> facilityList = new List<SHNFacility>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                responseTask.Wait();
                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<string> readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    string data = readTask.Result;
                    facilityList = JsonConvert.DeserializeObject<List<SHNFacility>>(data);
                }
            }

            // Load Person and Business Location Data
            LoadPersonFile(pList, facilityList);
            LoadBLFile(bList);

            while (true)
            {
                DisplayMainMenu(); // Call method to display menu
                int option = Convert.ToInt32(Console.ReadLine());

                if (option == 1)
                {
                    while (true)
                    {
                        DisplayGeneralMenu();
                        try
                        {
                            int choice = Convert.ToInt32(Console.ReadLine());

                            if (choice == 1) // List all visitors
                            {
                                DisplayVisitor(pList);
                            }
                            else if (choice == 2) // List person details
                            {
                                ListPersonDetail(pList);
                            }
                            else if (choice == 0) // Exit General Menu
                            {
                                break;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid option! Please try again");
                        }
                    }

                }
                else if (option == 2)
                {
                    while (true)
                    {
                        SafeEntryMenu();    // Call method to display safe entry menu
                        try
                        {
                            int choice = Convert.ToInt32(Console.ReadLine());

                            if (choice == 1) // Assign/Replace TraceTogether Token
                            {
                                Console.WriteLine();
                                AssignReplaceToken(pList);
                                Console.WriteLine();
                            }
                            else if (choice == 2) // List all Business Location
                            {
                                Console.WriteLine();
                                ListBusinessLocation(bList);
                                Console.WriteLine();
                            }
                            else if (choice == 3) // Edit Business Location Capacity
                            {
                                Console.WriteLine();
                                EditBusinessLocation(bList);
                                Console.WriteLine();
                            }

                            else if (choice == 4) // SafeEntry Check-In
                            {
                                Console.WriteLine();
                                SafeEntryCheckIn(pList, bList);
                                Console.WriteLine();
                            }

                            else if (choice == 5) // SafeEntry Check-Out
                            {
                                Console.WriteLine();
                                SafeEntryCheckOut(pList, bList);
                                Console.WriteLine();
                            }

                            else if (choice == 0) //Exit Safe Entry Menu
                            {
                                break;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid option! Please try again");
                        }
                    }
                }

                else if (option == 3)
                {
                    while (true)
                    {
                        TravelEntryMenu();  // Call method to display travel entry menu                      
                        try
                        {
                            int choice = Convert.ToInt32(Console.ReadLine());

                            if (choice == 1) // List SHN Facilities
                            {
                                Console.WriteLine();
                                ListSHNFacility(facilityList);
                                Console.WriteLine();
                            }
                            else if (choice == 2) // Create Visitor
                            {
                                Console.WriteLine();
                                CreateVisitor(pList);
                                Console.WriteLine();
                            }
                            else if (choice == 3) // Create Travel Entry
                            {
                                Console.WriteLine();
                                CreateTravelEntry(pList, facilityList);
                                Console.WriteLine();
                            }

                            else if (choice == 4) // Calculate SHN Charges
                            {
                                Console.WriteLine();
                                CalculateSHNCharges(pList);
                                Console.WriteLine();
                            }

                            else if (choice == 0) // Exit Travel Entry Menu
                            {
                                break;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid option! Please try again");
                        }
                    }
                }
                else if (option == 0) // Exit Main Menu
                {
                    Console.WriteLine("Exiting");
                    break;
                }

            }
        }

        // Methods for Basic Features

        // Method to display main menu
        static void DisplayMainMenu()
        {
            Console.WriteLine("---------- COVID-19 Monitoring System ----------");
            Console.WriteLine("[1] General");
            Console.WriteLine("[2] Safe Entry");
            Console.WriteLine("[3] Travel Entry");
            Console.WriteLine("[0] Exit");
            Console.Write("Enter option: ");
        }

        // Method to display the items inside the general option
        static void DisplayGeneralMenu()
        {
            Console.WriteLine("=============== General ===============");
            Console.WriteLine("[1] List all visitors");
            Console.WriteLine("[2] List person details");
            Console.WriteLine("[0] Return back to main menu");
            Console.Write("Enter option: ");
        }

        // Method to load person information
        static void LoadPersonFile(List<Person> pList, List<SHNFacility> facilityList)
        {
            string[] personLines = File.ReadAllLines("Person.csv"); // Read csv file
            for (int i = 1; i < personLines.Length; i++)    // Iterate through person csv file
            {
                string[] p = personLines[i].Split(",");

                if (p[0] == "resident")  // Checking for residents
                {
                    Resident resident = new Resident(p[1], p[2], DateTime.ParseExact(p[3], "dd/MM/yyyy", null));    // Resident object
                    pList.Add(resident);    // Add resident object to list

                    if (p[6] != "") // Checking if resident owns a token
                    {
                        resident.Token = new TraceTogetherToken(p[6], p[7], DateTime.ParseExact(p[8], "dd/MM/yyyy", null)); // Token object
                    }

                    if (p[9] != "") // Check for Travel Entry
                    {
                        DateTime travelEntryDate = DateTime.ParseExact(p[11], "dd/MM/yyyy H:mm", null); // Converting DateTime format
                        TravelEntry te = new TravelEntry(p[9], p[10], travelEntryDate); // Travel Entry object
                        DateTime travelShnEndDate = DateTime.ParseExact(p[12], "dd/MM/yyyy H:mm", null);
                        te.ShnEndDate = travelShnEndDate;
                        te.IsPaid = Convert.ToBoolean(p[13]);

                        foreach (SHNFacility facility in facilityList)  // Iterate through facility list
                        {
                            if (facility.FacilityName == p[14]) // Check if facility name match
                            {
                                facility.FacilityVacany -= 1;   // Person leave SHN. reducing the number of people vacancy available
                                te.AssignSHNFacility(facility);
                            }
                        }
                        resident.AddTravelEntry(te);
                    }
                }
                else if (p[0] == "visitor")
                {
                    Visitor visitor = new Visitor(p[1], p[4], p[5]);    // Visitor object
                    pList.Add(visitor); // Add visitor object to list

                    if (p[9] != "")
                    {
                        DateTime travelEntryDate = DateTime.ParseExact(p[11], "dd/MM/yyyy H:mm", null); // Converting DateTime format
                        TravelEntry te = new TravelEntry(p[9], p[10], travelEntryDate); // Travel Entry object
                        DateTime travelShnEndDate = DateTime.ParseExact(p[12], "dd/MM/yyyy H:mm", null);
                        te.ShnEndDate = travelShnEndDate;
                        te.IsPaid = Convert.ToBoolean(p[13]);

                        foreach (SHNFacility facility in facilityList)  // Iterate through facility list
                        {
                            if (facility.FacilityName == p[14]) // Check if facility name match
                            {
                                facility.FacilityVacany -= 1;   // Person leave SHN. reducing the number of people vacancy available
                                te.AssignSHNFacility(facility);
                            }
                        }
                        visitor.AddTravelEntry(te);
                    }
                }
            }
        }

        // Method to load business location information
        static void LoadBLFile(List<BusinessLocation> bList)
        {
            string[] blLines = File.ReadAllLines("BusinessLocation.csv");   // Read business location csv
            for (int i = 1; i < blLines.Length; i++)    // Iterate business location file
            {
                string[] bl = blLines[i].Split(",");
                bList.Add(new BusinessLocation(bl[0], bl[1], Convert.ToInt32(bl[2]))); // Add business location object
            }
        }

        // Method to display visitors
        static void DisplayVisitor(List<Person> pList)
        {
            Console.WriteLine();
            Console.WriteLine("Visitor details");
            Console.WriteLine("---------------");
            foreach (Person p in pList) // Iterate through the person list
            {
                if (p is Visitor) // If Person is a Visitor
                {
                    Visitor v = (Visitor)p; // Downcast   
                    Console.WriteLine(v); // Display the details of each visitor 
                }
            }
        }

        // Method to list person detail
        static void ListPersonDetail(List<Person> pList)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.WriteLine();

            Person p = SearchPerson(pList, name);   // Searching for person
            if (p != null)  // When person is located
            {
                if (p is Visitor)
                {
                    Visitor v = (Visitor)p;
                    Console.WriteLine("{0, -10} {1, -15} {2, -15}", "Name", "PassportNo", "Nationality");
                    Console.WriteLine("{0, -10} {1, -15} {2, -15}", "----", "----------", "-----------");
                    Console.WriteLine("{0, -10} {1, -15} {2, -15}", v.Name, v.PassportNo, v.Nationality);

                    if (v.SafeEntryList != null)
                    {
                        Console.WriteLine();
                        foreach (SafeEntry se in p.SafeEntryList)   //Displaying of Safe Entry List
                        {
                            Console.WriteLine("Safe Entry Record: ");
                            Console.WriteLine("{0} {1} {2}", se.Location, se.CheckIn, se.CheckOut);
                        }
                    }
                    if (v.TravelEntryList != null)
                    {
                        Console.WriteLine();
                        foreach (TravelEntry te in p.TravelEntryList)   //Displaying of Travel Entry List
                        {
                            Console.WriteLine("Travel Entry Record: ");
                            Console.WriteLine("{0} {1} {2} {3} {4}", te.LastCountryOfEmbarkation, te.EntryMode, te.EntryDate, te.ShnEndDate, te.IsPaid);
                        }
                    }
                }

                else if (p is Resident)
                {
                    Resident r = (Resident)p;   //Downcast

                    Console.WriteLine("{0, -10} {1, -15} {2, -15}", "Name", "Address", "LastLeftCountry");
                    Console.WriteLine("{0, -10} {1, -15} {2, -15}", "----", "-------", "---------------");
                    Console.WriteLine("{0, -10} {1, -20} {2, -15}", r.Name, r.Address, r.LastLeftCountry);

                    if (r.Token != null)    //If the token details loaded is not null then print details
                    {
                        Console.WriteLine();
                        Console.WriteLine("TraceTogether Token: ");
                        Console.WriteLine("{0} {1} {2}", r.Token.SerialNo, r.Token.CollectionLocation, r.Token.ExpiryDate);
                    }
                    if (r.SafeEntryList != null)    //If the safe entry list loaded is not null then print details
                    {
                        Console.WriteLine();
                        foreach (SafeEntry se in p.SafeEntryList)   //Displaying of Safe Entry List
                        {
                            Console.WriteLine("Safe Entry Record: ");
                            Console.WriteLine("{0} {1} {2}", se.Location, se.CheckIn, se.CheckOut);
                        }
                    }
                    if (r.TravelEntryList != null)  //If the travel entry list loaded is not null then print details
                    {
                        Console.WriteLine();
                        foreach (TravelEntry te in p.TravelEntryList)   // Display of Travel Entry List
                        {
                            Console.WriteLine("Travel Entry Record: ");
                            Console.WriteLine("{0} {1} {2} {3} {4}", te.LastCountryOfEmbarkation, te.EntryMode, te.EntryDate, te.ShnEndDate, te.IsPaid);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Business location is not found");
            }
        }

        // Method to search person
        static Person SearchPerson(List<Person> pList, string targetname)
        {
            foreach (Person p in pList)   // Iterate through the person list
            {
                if (targetname == p.Name) // If the input name matches the name of the person found in the person list
                {
                    return p;  // Return all details about the selected person
                }
            }
            return null;
        }


        /* Methods for SafeEntry/TraceTogether */

        // Method to display SafeEntryMenu
        static void SafeEntryMenu()
        {
            Console.WriteLine("=============== SafeEntry/TraceTogether ===============");
            Console.WriteLine("[1] Assign/Replace TraceTogether Token");
            Console.WriteLine("[2] List all Business Locations");
            Console.WriteLine("[3] Edit Business Location Capacity");
            Console.WriteLine("[4] SafeEntry Check-In");
            Console.WriteLine("[5] SafeEntry Check-Out");
            Console.WriteLine("[0] Back");
            Console.Write("Enter option: ");
        }

        // Method use to Assign/Replace TraceTogether Token
        static void AssignReplaceToken(List<Person> pList)
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            foreach (Person p in pList) // Iterate person list
            {
                if (name == p.Name) // If the input name matches the name of the person found in the person list
                {
                    if (p is Resident)
                    {
                        Resident resident = (Resident)p; // Downcast                        
                        if (resident.Token == null)
                        {
                            System.Random random = new System.Random();
                            int newSerialNo = random.Next(0, 100000);   // Randomize token serial number
                            string SerialNo = "T" + newSerialNo;

                            Console.Write("Enter the location you collected your token: ");
                            string newCLocation = Console.ReadLine();
                            DateTime dt = DateTime.Now; // Retrieve current date time
                            DateTime exipryDate = dt.AddMonths(6);
                            TraceTogetherToken ttt = new TraceTogetherToken(SerialNo, newCLocation, exipryDate);    // Trace Together Token object
                            Console.Write("The details of your TraceTogether token are ");
                            Console.WriteLine("{0} {1} {2}", SerialNo, newCLocation, exipryDate);
                            Console.WriteLine("Token has been successfully assigned.");
                            resident.Token = ttt;   // Assigning of object
                        }
                        else
                        {
                            Console.WriteLine("A token has already been assigned to " + name);
                            if (resident.Token.IsElligibleForReplacement() == true) // Checking for elligibility
                            {
                                System.Random random = new System.Random();
                                int newSerialNo = random.Next(0, 100000);   // Randomize token serial number
                                string SerialNo = "T" + newSerialNo;

                                Console.Write("Enter the location you collected your new token: ");
                                string newCLocation = Console.ReadLine();
                                resident.Token.ReplaceToken(SerialNo, newCLocation);    // Replace token

                                Console.Write("The details of your new TraceTogether token are ");
                                Console.WriteLine("{0} {1}", SerialNo, newCLocation);
                                Console.WriteLine("Token has been successfully replaced.");
                            }
                            else
                            {
                                Console.WriteLine("Not available for replacement");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unable to assign token only residents are able to participate.");
                    }
                }
            }
        }

        // Method to list all Business Locations
        static void ListBusinessLocation(List<BusinessLocation> bList)
        {
            Console.WriteLine("{0, -20} \t{1, -10} \t{2, -15}", "BusinessName", "BranchCode", "MaximumCapacity");
            Console.WriteLine("{0, -20} \t{1, -10} \t{2, -15}", "------------", "----------", "---------------");
            foreach (BusinessLocation b in bList)   // Iterate business location list
            {
                Console.WriteLine("{0, -20} \t{1, -10} \t{2, -15}", b.BusinessName, b.BranchCode, b.MaximumCapacity);
            }
        }

        // Method to edit Business Location Capacity
        static void EditBusinessLocation(List<BusinessLocation> bList)
        {
            Console.Write("Enter business name: ");
            string name = Console.ReadLine();

            BusinessLocation bl = SearchBusinessLocation(bList, name);  // Call SearchBusinessLocation() to find whether the input name of the business matches the actual name of business 
            if (bl != null)
            {
                Console.Write("Editing maximum capacity to: ");
                int mc = Convert.ToInt32(Console.ReadLine());

                bl.MaximumCapacity = mc;    // Update the affected business with the new amount
            }
            else
            {
                Console.WriteLine("Business location is not found");    // Display error message when business name is not equivalent to the targetted name
            }
        }

        // Method to search for Business Location
        static BusinessLocation SearchBusinessLocation(List<BusinessLocation> bList, string name)
        {
            foreach (BusinessLocation b in bList)   // Iterate business location list
            {
                if (name == b.BusinessName) // If the input business name matches the name of the business found in the business location list
                {
                    return b;
                }
            }
            return null;
        }

        // Method for SafeEntry Check-In
        static void SafeEntryCheckIn(List<Person> pList, List<BusinessLocation> bList)
        {
            Console.Write("Enter person name to search: ");
            string name = Console.ReadLine();
            Person p = SearchPerson(pList, name);   // Search for person
            if (p != null)  // When person is located
            {
                ListBusinessLocation(bList);    //Call method to list business locations

                Console.WriteLine();
                Console.Write("Enter business location to check-in: ");
                string l = Console.ReadLine();
                BusinessLocation b = SearchCheckInBusiness(bList, l);   // Call method to search for business
                if (b != null)
                {
                    // Create safe entry object
                    Console.Write("Enter Check-In Date: ");
                    string date = Console.ReadLine();
                    DateTime ci = DateTime.Now; // Retrieve date time

                    SafeEntry s = new SafeEntry(ci, b);
                    p.AddSafeEntry(s);
                    Console.WriteLine(name + " has completed the safe entry check-in.");
                }
                else
                {
                    Console.WriteLine("Current selected business location is full.");
                }
            }
            else
            {
                Console.WriteLine("Unable to find person.");
            }
        }

        // Method to search business (check-in)
        static BusinessLocation SearchCheckInBusiness(List<BusinessLocation> bList, string l)
        {
            foreach (BusinessLocation b in bList)   // Iterate through business list
            {
                if (l == b.BusinessName)    // If the input business name matches the name of the business found in the business location list 
                {
                    if (b.IsFull() == false)
                    {
                        b.VisitorsNow += 1; // Increase visitorsNow by 1
                        return b;
                    }
                }
            }
            return null;
        }

        // Method to search business (check-out)
        static BusinessLocation SearchCheckOutBusiness(List<BusinessLocation> bList, string l)
        {
            foreach (BusinessLocation b in bList)   // Iterate through business list
            {
                if (l == b.BusinessName)    // If the input business name matches the name of the business found in the business location list 
                {
                    if (b.IsFull() == false)
                    {
                        b.VisitorsNow -= 1; // Reduce visitorsNow by 1
                        return b;
                    }
                }
            }
            return null;
        }


        // Method for SafeEntry Check-Out
        static void SafeEntryCheckOut(List<Person> pList, List<BusinessLocation> bList)
        {
            Console.Write("Enter name to search: ");
            string targetname = Console.ReadLine();
            Person p = SearchPerson(pList, targetname);    // Search for person
            if (p != null)  // When person is located
            {
                if (p.SafeEntryList.Count > 0)  // If there is an safe entry object print
                {
                    SafeEntry se = p.SafeEntryList[0];
                    Console.WriteLine(se);

                    Console.WriteLine();
                    Console.Write("Enter business location to check-out: ");
                    string bl = Console.ReadLine();

                    BusinessLocation b = SearchCheckOutBusiness(bList, bl); // Call SearchCheckOutBusiness method

                    se.PerformCheckOut();   // Call method to perform check out
                    Console.WriteLine("You have successfully checked-out");
                }
            }
            else
            {
                Console.WriteLine("Unable to find person.");
            }
        }

        /* Travel Entry Methods */

        // Methods for travel entry 
        static void TravelEntryMenu()
        {
            Console.WriteLine("=============== TravelEntry ===============");
            Console.WriteLine("[1] View SHN facilities");
            Console.WriteLine("[2] Create Visitor");
            Console.WriteLine("[3] Create TravelEntry record");
            Console.WriteLine("[4] Calculate SHN Charges");
            Console.WriteLine("[0] Back");
            Console.Write("Enter option: ");
        }

        // List all SHN facilities
        static void ListSHNFacility(List<SHNFacility> facilityList)
        {
            Console.WriteLine("{0, -20} {1, -20} {2, 15} {3, 20} {4, 20}", "Facility Name", "Facility Capacity", "Dist from air cp", "Dist from sea cp", "Dist from land cp");
            Console.WriteLine("{0, -20} {1, -20} {2, 15} {3, 20} {4, 20}", "-------------", "-----------------", "----------------", "----------------", "-----------------");

            foreach (SHNFacility f in facilityList) // Iterate through facility list
            {
                Console.WriteLine("{0, -20} {1, -20} {2, -20} {3, -20} {4, -20}", f.FacilityName, f.FacilityCapacity, f.DistFromAirCheckpoint, f.DistFromLandCheckpoint, f.DistFromSeaCheckpoint);
            }
        }

        // Methods to create visitor object 
        static void CreateVisitor(List<Person> pList)
        {
            Console.WriteLine("Enter name: ");
            string n = Console.ReadLine();
            Console.WriteLine("Enter passportNo: ");
            string pn = Console.ReadLine();
            Console.WriteLine("Enter nationality: ");
            string nat = Console.ReadLine();

            Visitor v = new Visitor(n, pn, nat);
            pList.Add(v);   // Add visitor object to the person list
            Console.WriteLine("\nSuccessfully added visitor to person list.");
        }

        // Method for TravelEntry object
        static void CreateTravelEntry(List<Person> pList, List<SHNFacility> facilityList)   // Create travel entry method
        {
            Console.Write("Enter name to search: ");
            string targetname = Console.ReadLine();
            Person p = SearchPerson(pList, targetname);    // Call the SearchPerson method to look for the person
            if (p != null)  // if able to find person
            {
                Console.Write("Enter last country of embarkation: ");
                string lc = Console.ReadLine();
                Console.Write("\nEnter entry mode: ");
                string em = Console.ReadLine();
                DateTime ed = DateTime.Now; // Get current date and time

                TravelEntry t = new TravelEntry(lc, em, ed);    // Create travel entry object

                DateTime endDate = t.CalculateSHNDuration();    // Call CalculateSHNDuration method and assign it to a variable

                if (endDate == ed.AddDays(14))  // If the calculated end date is the same as expected end date, 
                {
                    Console.WriteLine();
                    ListSHNFacility(facilityList);  // Call ListSHNFacility method to list all the available facilities
                    Console.Write("\nChoose preferred SHN facility name: ");
                    string facilityChoice = Console.ReadLine();
                    SHNFacility s = SearchSHNFacility(facilityList, facilityChoice);    // Search for input facility name
                    if (facilityChoice != null) // If input facility name exists in the list
                    {
                        if (s.IsAvailable() == true)    // Check for vacancy in the facility
                        {
                            t.AssignSHNFacility(s); // If there's available slot, assign the selected facility to travel entry
                            Console.WriteLine("You've selected {0} as your SHN facility", facilityChoice);
                        }
                        else
                        {
                            Console.WriteLine("The Chosen facility is full.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unable to find the facility.");
                    }

                }
                p.AddTravelEntry(t); // Add travel entry object
                Console.WriteLine("Visitor has been added into travel entry list.");
            }
            else
            {
                Console.WriteLine("Unable to find person.");    // Display error message when unable to find the input person's name
            }

        }

        // Method to CalculateSHNCharges
        static void CalculateSHNCharges(List<Person> pList)
        {
            Console.Write("Enter name to search: ");
            string targetname = Console.ReadLine();
            Person p = SearchPerson(pList, targetname);    // Call the SearchPerson method to look for the person
            if (p != null)
            {
                if (p.TravelEntryList.Count > 0) // Check to see if the person has travel entry record
                {
                    foreach (TravelEntry t in p.TravelEntryList) // Iterate through travel entry list
                    {
                        double amount = p.CalculateSHNCharges();
                        if (t.ShnEndDate <= DateTime.Now && t.IsPaid == false)
                        {
                            Console.WriteLine("SHN Charges due is ${0:00.00}", amount);   // Display amount due
                            Console.WriteLine("Proceed to make payment (Yes/No): ");
                            string ans = Console.ReadLine();
                            if (ans == "Yes")   // When ans == "Yes"
                            {
                                t.IsPaid = true;    // Payment has been made by person
                                if (t.ShnStay != null)
                                {
                                    t.ShnStay.FacilityVacany += 1;  // Person left SHN, increase vacancy count
                                }
                                Console.WriteLine("Payment successful");
                            }
                            else    // When ans == "No"
                            {
                                Console.WriteLine("Payment terminated.");
                            }
                        }
                        else if (t.IsPaid == true)
                        {
                            Console.WriteLine("You've already paid the SHN charges. No outstanding charges.");
                        }
                        else if (t.ShnEndDate > DateTime.Now)
                        {
                            Console.WriteLine("Payment unavailable as SHN stay has not ended.");
                        }
                    }
                }
                else    // When TravelEntryList.Count == 0
                {
                    Console.WriteLine("No travel entry records.");
                }
            }
            else
            {
                Console.WriteLine("Unable to find person.");
            }
        }

        // Method to search for facility
        static SHNFacility SearchSHNFacility(List<SHNFacility> fList, string facilityChoice)
        {
            foreach (SHNFacility f in fList)    // Iterate through facility list
            {
                if (facilityChoice == f.FacilityName) // If input facility name matches facility name found in the facility list
                {
                    f.FacilityVacany = f.FacilityCapacity;  // Set facility vacancy to be facility capacity
                    return f;   // Return all details of that particular chosen facility
                }
            }
            return null;
        }
    }
}
