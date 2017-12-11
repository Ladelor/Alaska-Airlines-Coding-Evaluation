using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Alaska_Airlines_Coding_Evaluation.Models;
using Alaska_Airlines_Coding_Evaluation.Helpers;

namespace Alaska_Airlines_Coding_Evaluation.Controllers
{
    public class FlightsController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Find()
        {
            return View(new Flights());
        }


        [HttpPost]
        public ActionResult Find(string arriving, string departing, int sortMode = 0)
        {
            var flightModel = new Flights();
            if (checkAirports(Server.MapPath("~/App_Data/airports.csv"), arriving, departing))
            {
                flightModel.arrivingAirport = arriving;
                flightModel.departingAirport = departing;

                flightModel.flights = getFlights(Server.MapPath("~/App_Data/flights.csv"), arriving, departing);
            }

            if(sortMode == 1)
            {
                flightModel.flights.Sort(delegate (Flight X, Flight Y)
                {
                    return X.mainCabinPrice.CompareTo(Y.mainCabinPrice);
                });
            }

            if (sortMode == 2)
            {
                flightModel.flights.Sort(delegate (Flight X, Flight Y)
                {
                    return X.departTime.CompareTo(Y.departTime);
                });
            }

            return View(flightModel);
            
        }

        
        // Method to get valid Flights from CSV file
        // Parameters: Path to CSV File, arriving airport, departing airport
        // Return: List of Class Flight's
        private List<Flight> getFlights(string filePath, string arrive, string depart)
        {
            List<Flight> flights = new List<Flight>();
            string value;
            using (TextReader fileReader = System.IO.File.OpenText(filePath))
            {
                //Skip first line with headers
                fileReader.ReadLine();
                var csv = new CsvReader(fileReader);
                while (csv.Read())
                { 
                    for (int i = 0; csv.TryGetField<string>(i, out value); i += 7)
                    {
                        if(value == depart)
                        {
                            if(csv.GetField<string>(i + 1) == arrive)
                            {
                                var validFlight = new Flight();
                                validFlight.departAirport = depart;
                                validFlight.arrvivingAiport = arrive;
                                validFlight.flightNumber = csv.GetField<string>(i + 2);
                                validFlight.departTime = csv.GetField<string>(i + 3);
                                validFlight.arrivalTime = csv.GetField<string>(i + 4);
                                validFlight.mainCabinPrice = csv.GetField<string>(i + 5);
                                validFlight.firstClassPrice = csv.GetField<string>(i + 6);
                                flights.Add(validFlight);
                            }
                        }
                    }
                }
            }

            return flights;
        }

        // Method to check if arriving and departing airports are in Excel
        // Parameters: String with CSV Airport file path, Strings with arriving and departing airport codes
        // Return Value: Bool for valid entry
        private bool checkAirports(string filePath, string arriving, string departing)
        {
            bool validArrive = false, validDepart = false;
            string value;
            using (TextReader fileReader = System.IO.File.OpenText(filePath))
            {
                //Skip first line with headers
                fileReader.ReadLine();
                var csv = new CsvReader(fileReader);
                while(csv.Read())
                {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i += 2)
                    {
                        if (value == arriving)
                            validArrive = true;
                        //This should gurantee arriving == departing returns false
                        //i.e. Same depart airport as arrive airport is invalid entry
                        else if (value == departing)
                            validDepart = true;
                    }
                }
            }
            return (validDepart && validArrive);
        }
    }
}