using Project_02_nov.Models;
using Project_02_nov.Service;
using Project_02_nov.Services;
using System;
using System.Collections.Generic;

namespace Project_02_nov.UI
{
    internal class PassengerUi
    {
        public static void DisplayPassengerMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= PASSENGER MENU =======");
                Console.WriteLine("1. View Routes");
                Console.WriteLine("2. Search Routes");
                Console.WriteLine("3. Book Ticket");
                Console.WriteLine("4. Cancel Booking");
                Console.WriteLine("5. View My Booking");
                Console.WriteLine("6. Exit to Main Menu");
                Console.Write("Choose your option: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    ConsoleUtility.ErrorMessage();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ViewRoutesUI();
                        break;

                    case 2:
                        SearchRoutesUI();
                        break;

                    case 3:
                        BookTicketUI();
                        break;

                    case 4:
                        CancelBookingUI();
                        break;

                    case 5:
                        ViewMyBookingUI();
                        break;

                    case 6:
                        Console.Clear();
                        Console.WriteLine("EXITING TO MAIN MENU");
                        ConsoleUtility.PauseScreen();
                        return;

                    default:
                        ConsoleUtility.ErrorMessage();
                        break;
                }
            }
        }

        public static void ViewRoutesUI()
        {
            Console.Clear();
            Console.WriteLine("======= AVAILABLE ROUTES & BUSES =======");

            RouteService routeService = new RouteService();
            BusService busService = new BusService();

            List<Route> routes = routeService.GetAllRoutes();
            List<Bus> buses = busService.GetAllBuses();

            if (routes.Count == 0)
            {
                Console.WriteLine("No routes currently available.");
                ConsoleUtility.PauseScreen();
                return;
            }

            foreach (Route r in routes)
            {
                Console.WriteLine($"Route ID:        {r.RouteId}");
                Console.WriteLine($"Starting Point:  {r.StartingRoute}");
                Console.WriteLine($"Destination:     {r.Destination}");
                Console.WriteLine($"Fare:            Rs. {r.Fare}");

                Console.WriteLine("Available Buses for this route:");
                bool busFound = false;
                foreach (Bus b in buses)
                {
                    if (b.RouteId != null && b.RouteId.Equals(r.RouteId, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"   - Bus {b.BusNumber} | Available Seats: {b.AvailableSeats} / {b.TotalSeats}");
                        busFound = true;
                    }
                }

                if (!busFound)
                {
                    Console.WriteLine("   (No buses currently assigned)");
                }

                Console.WriteLine("-----------------------------------------------");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void SearchRoutesUI()
        {
            Console.Clear();
            Console.WriteLine("======= SEARCH ROUTES =======");
            Console.Write("Enter destination or starting city: ");
            string query = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Search query cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            RouteService routeService = new RouteService();
            BusService busService = new BusService();

            List<Route> routes = routeService.GetAllRoutes();
            List<Bus> buses = busService.GetAllBuses();

            bool found = false;
            foreach (Route r in routes)
            {
                if (r.StartingRoute.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    r.Destination.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    r.RouteId.Equals(query, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    Console.WriteLine($"\nRoute ID:        {r.RouteId}");
                    Console.WriteLine($"From:            {r.StartingRoute}");
                    Console.WriteLine($"To:              {r.Destination}");
                    Console.WriteLine($"Fare:            Rs. {r.Fare}");

                    Console.WriteLine("Buses:");
                    foreach (Bus b in buses)
                    {
                        if (b.RouteId != null && b.RouteId.Equals(r.RouteId, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"   - Bus: {b.BusNumber} (Available Seats: {b.AvailableSeats}/{b.TotalSeats})");
                        }
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("No routes found matching your query.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void BookTicketUI()
        {
            Console.Clear();
            Console.WriteLine("======= BOOK A TICKET =======");

            BusService busService = new BusService();
            List<Bus> buses = busService.GetAllBuses();

            if (buses.Count == 0)
            {
                Console.WriteLine("No buses available for booking.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.WriteLine("Currently active buses:");
            foreach (Bus b in buses)
            {
                if (!string.IsNullOrEmpty(b.RouteId) && b.AvailableSeats > 0)
                {
                    Console.WriteLine($"Bus Number: {b.BusNumber} | Route: {b.RouteId} | Available Seats: {b.AvailableSeats}");
                }
            }
            Console.WriteLine("-----------------------------------------------");

            Console.Write("Enter Passenger ID (e.g. P101): ");
            string passengerId = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                Console.WriteLine("Passenger ID cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            PassengerService passengerService = new PassengerService();
            Passenger? passenger = passengerService.FindPassengerById(passengerId);

            if (passenger == null)
            {
                Console.Write("Enter Passenger Name: ");
                string name = (Console.ReadLine() ?? "").Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Passenger Name cannot be empty.");
                    ConsoleUtility.PauseScreen();
                    return;
                }

                Console.Write("Enter Age: ");
                int age;
                if (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
                {
                    Console.WriteLine("Invalid age.");
                    ConsoleUtility.PauseScreen();
                    return;
                }

                passenger = new Passenger(passengerId, name, age);
                passengerService.AddPassenger(passenger);
            }
            else
            {
                Console.WriteLine($"Welcome back, {passenger.name}!");
            }

            Console.Write("Enter Bus Number to book: ");
            string busNumber = (Console.ReadLine() ?? "").Trim();

            Console.Write("How many tickets do you want to book? ");
            int ticketsBooked;
            if (!int.TryParse(Console.ReadLine(), out ticketsBooked) || ticketsBooked <= 0)
            {
                Console.WriteLine("Please enter a valid ticket quantity greater than zero.");
                ConsoleUtility.PauseScreen();
                return;
            }

            BookingService bookingService = new BookingService();
            Booking? booking = bookingService.BookTicket(passengerId, busNumber, ticketsBooked);

            if (booking == null)
            {
                Console.WriteLine("Booking failed! Check that the bus exists, has an assigned route, and has seats available.");
            }
            else
            {
                Console.WriteLine("\n================ TICKET CONFIRMED ================");
                Console.WriteLine($"Booking ID:      {booking.BookingId}");
                Console.WriteLine($"Passenger ID:    {booking.PassengerId}");
                Console.WriteLine($"Passenger Name:  {passenger.name}");
                Console.WriteLine($"Bus Number:      {booking.BusNumber}");
                Console.WriteLine($"Tickets Booked:  {booking.TicketsBooked}");
                Console.WriteLine($"Fare per Ticket: Rs. {booking.FareAtBooking}");
                Console.WriteLine($"Total Fare:      Rs. {booking.FareAtBooking * booking.TicketsBooked}");
                Console.WriteLine($"Status:          {booking.Status}");
                Console.WriteLine("==================================================");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void CancelBookingUI()
        {
            Console.Clear();
            Console.WriteLine("======= CANCEL BOOKING =======");

            Console.Write("Enter Booking ID to cancel: ");
            string bookingId = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(bookingId))
            {
                Console.WriteLine("Booking ID cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            BookingService bookingService = new BookingService();
            bool result = bookingService.CancelBooking(bookingId);

            if (result)
            {
                Console.WriteLine("Booking cancelled successfully! Seat has been returned to the bus.");
            }
            else
            {
                Console.WriteLine("Cancellation failed. Check that the Booking ID is correct and has not already been cancelled.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void ViewMyBookingUI()
        {
            Console.Clear();
            Console.WriteLine("======= VIEW BOOKING =======");
            Console.Write("Enter Booking ID or Passenger ID: ");
            string input = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            BookingService bookingService = new BookingService();
            Booking? singleBooking = bookingService.FindBookingById(input);

            if (singleBooking != null)
            {
                PrintBookingDetails(singleBooking);
                if (singleBooking.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                {
                    bookingService.DeleteCancelledBooking(singleBooking.BookingId);
                }
                ConsoleUtility.PauseScreen();
                return;
            }

            List<Booking> list = bookingService.GetBookingsByPassengerId(input);
            if (list.Count > 0)
            {
                Console.WriteLine($"\nFound {list.Count} booking(s) for Passenger ID {input}:");
                foreach (Booking b in list)
                {
                    PrintBookingDetails(b);
                }

                foreach (Booking b in list)
                {
                    if (b.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                    {
                        bookingService.DeleteCancelledBooking(b.BookingId);
                    }
                }
            }
            else
            {
                Console.WriteLine("No bookings found with that Booking ID or Passenger ID.");
            }

            ConsoleUtility.PauseScreen();
        }

        private static void PrintBookingDetails(Booking b)
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"Booking ID:   {b.BookingId}");
            Console.WriteLine($"Passenger ID: {b.PassengerId}");
            Console.WriteLine($"Bus Number:   {b.BusNumber}");
            Console.WriteLine($"Tickets:      {b.TicketsBooked}");
            Console.WriteLine($"Fare/Ticket:  Rs. {b.FareAtBooking}");
            Console.WriteLine($"Total Fare:   Rs. {b.FareAtBooking * b.TicketsBooked}");
            Console.WriteLine($"Status:       {b.Status}");
            Console.WriteLine("---------------------------------------------");
        }
    }
}
