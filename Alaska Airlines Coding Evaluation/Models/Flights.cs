using Alaska_Airlines_Coding_Evaluation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alaska_Airlines_Coding_Evaluation.Models
{
    
    public class Flights
    {
        public string arrivingAirport { get; set; }
        public string departingAirport { get; set; }
        public List<Flight> flights { get; set; }
       

        public Flights()
        {
            flights = new List<Flight>();
        }
    }

    
}