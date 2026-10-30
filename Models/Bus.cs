using Project_02_nov.UI;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Project_02_nov.Models
{
    internal class Bus
    {

        public string BusNumber;
        public int TotalSeats;
        public int AvailableSeats;
        public string? RouteId;


        public Bus(string busNumber, int totalSeats, int availableSeats, string? routeId)
        {
            BusNumber = busNumber;
            TotalSeats = totalSeats;
            AvailableSeats = availableSeats;
            RouteId = routeId;
        }

        public override string ToString()
        {
            return $"{BusNumber},{TotalSeats},{AvailableSeats},{RouteId??""}";
        }
    }
}
