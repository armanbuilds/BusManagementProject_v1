using Project_02_nov.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace Project_02_nov.Models
{
    internal class Booking
    {
        public string BookingId;
        public string PassengerId;
        public string BusNumber;

        public double FareAtBooking;
        public string Status;
        public int TicketsBooked;


        public Booking(string BookingId , string PassengerId , string BusNumber, double FareAtBooking , string status, int ticketsBooked = 1)
        {
            this.BookingId = BookingId;
            this.PassengerId = PassengerId;
            this.BusNumber = BusNumber;
            this.FareAtBooking  = FareAtBooking;
            this.Status = status;
            this.TicketsBooked = ticketsBooked;
        }

        public override string ToString()
        {
            return $"{BookingId},{PassengerId},{BusNumber},{FareAtBooking},{Status},{TicketsBooked}";
        }
    }
}
