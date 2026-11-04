using Project_02_nov.Models;
using Project_02_nov.Service;
using System;
using System.Collections.Generic;

namespace Project_02_nov.UI
{
    internal class AdminUi
    {
        public static void DisplayAdminMenu()
        {
            AdminService adminService = new AdminService();
            Console.Clear();
            Console.WriteLine("======= ADMIN LOGIN =======");
            Console.Write("Enter Admin Password: ");
            string password = Console.ReadLine() ?? "";

            if (!adminService.ValidatePassword(password))
            {
                Console.WriteLine("Incorrect password! Access denied.");
                ConsoleUtility.PauseScreen();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= ADMIN MENU =======");
                Console.WriteLine("1. MANAGE BUSES");
                Console.WriteLine("2. MANAGE ROUTES");
                Console.WriteLine("3. VIEW ALL BOOKINGS");
                Console.WriteLine("4. SEARCH BOOKING");
                Console.WriteLine("5. SEARCH ROUTES BY FARE");
                Console.WriteLine("6. SEARCH ROUTES BY KEYWORD");
                Console.WriteLine("7. VIEW ROUTES BY PAGE SIZE");
                Console.WriteLine("8. EXIT TO MAIN MENU");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    ConsoleUtility.ErrorMessage();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DisplayBusMenu();
                        break;

                    case 2:
                        DisplayRouteMenu();
                        break;

                    case 3:
                        ViewAllBookingsUI();
                        break;

                    case 4:
                        SearchBookingUI();
                        break;

                    case 5:
                        SearchRoutesByFareUI();
                        break;

                    case 6:
                        SearchRoutesByKeywordUI();
                        break;

                    case 7:
                        ViewRoutesByPageView();
                        break;

                    case 8:
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

        public static void DisplayBusMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= MANAGE BUSES =======");
                Console.WriteLine("1. VIEW ALL BUSES");
                Console.WriteLine("2. ADD BUS");
                Console.WriteLine("3. UPDATE BUS");
                Console.WriteLine("4. DELETE BUS");
                Console.WriteLine("5. RETURN TO ADMIN MENU");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    ConsoleUtility.ErrorMessage();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        DisplayAllBuses();
                        ConsoleUtility.PauseScreen();
                        break;

                    case 2:
                        AddBusUI();
                        break;

                    case 3:
                        UpdateBusUI();
                        break;

                    case 4:
                        DeleteBusUI();
                        break;

                    case 5:
                        return;

                    default:
                        ConsoleUtility.ErrorMessage();
                        break;
                }
            }
        }

        public static void DisplayAllBuses()
        {
            BusService busService = new BusService();
            List<Bus> buses = busService.GetAllBuses();

            if (buses.Count == 0)
            {
                Console.WriteLine("No buses found.");
                return;
            }

            Console.WriteLine("========== ALL BUSES ==========");
            foreach (Bus b in buses)
            {
                Console.WriteLine($"Bus Number: {b.BusNumber}");
                Console.WriteLine($"Total Seats: {b.TotalSeats}");
                Console.WriteLine($"Available Seats: {b.AvailableSeats}");

                if (string.IsNullOrEmpty(b.RouteId))
                {
                    Console.WriteLine("Assigned Route: No route assigned");
                }
                else
                {
                    Console.WriteLine($"Assigned Route: {b.RouteId}");
                }

                Console.WriteLine("-------------------------------");
            }
        }

        public static void AddBusUI()
        {
            Console.Clear();
            Console.WriteLine("======= ADD NEW BUS =======");

            BusService busService = new BusService();
            Console.Write("Enter bus number: ");
            string busNumber = (Console.ReadLine() ?? "").Trim();

            if (!busService.IsValidBusNumber(busNumber))
            {
                Console.WriteLine("Error: Bus number cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            if (busService.BusNumberExists(busNumber))
            {
                Console.WriteLine("Error: This bus number already exists.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter total seats: ");
            int totalSeats;
            if (!int.TryParse(Console.ReadLine(), out totalSeats))
            {
                Console.WriteLine("Error: Please enter a valid whole number.");
                ConsoleUtility.PauseScreen();
                return;
            }

            if (!busService.IsValidCapacity(totalSeats))
            {
                Console.WriteLine("Error: Seat capacity must be greater than zero.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter route ID (leave empty if unassigned): ");
            string routeInput = (Console.ReadLine() ?? "").Trim();
            string? routeId;

            if (string.IsNullOrWhiteSpace(routeInput))
            {
                routeId = null;
            }
            else
            {
                RouteService routeService = new RouteService();

                if (!routeService.IsValidRouteId(routeInput))
                {
                    Console.WriteLine("Error: Invalid route ID.");
                    ConsoleUtility.PauseScreen();
                    return;
                }

                if (routeService.FindRouteById(routeInput) == null)
                {
                    Console.WriteLine("Error: This route does not exist.");
                    ConsoleUtility.PauseScreen();
                    return;
                }

                routeId = routeInput;
            }

            Bus bus = new Bus(busNumber, totalSeats, totalSeats, routeId);

            if (busService.AddBus(bus))
            {
                Console.WriteLine("Bus added successfully!");
            }
            else
            {
                Console.WriteLine("Error: Bus could not be added.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void UpdateBusUI()
        {
            Console.Clear();
            Console.WriteLine("======= UPDATE BUS =======");

            Console.Write("Enter the bus number to update: ");
            string busNumber = (Console.ReadLine() ?? "").Trim();

            BusService busService = new BusService();
            if (!busService.BusNumberExists(busNumber))
            {
                Console.WriteLine("Error: Invalid bus number. This bus is not present in the system.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter the new route ID (leave empty to unassign): ");
            string routeId = (Console.ReadLine() ?? "").Trim();

            Console.Write("Enter the new total seat capacity: ");
            int newTotalSeats;
            if (!int.TryParse(Console.ReadLine(), out newTotalSeats))
            {
                Console.WriteLine("Error: Invalid seat number.");
                ConsoleUtility.PauseScreen();
                return;
            }

            bool result = busService.UpdateBus(
                busNumber,
                routeId,
                newTotalSeats
            );

            if (result)
            {
                Console.WriteLine("Bus updated successfully!");
            }
            else
            {
                Console.WriteLine("Unable to update bus. Check the bus number, route ID, and seat capacity.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void DeleteBusUI()
        {
            Console.Clear();
            Console.WriteLine("======= DELETE BUS =======");

            Console.Write("Enter the bus number to delete: ");
            string busNumber = (Console.ReadLine() ?? "").Trim();

            BusService busService = new BusService();
            bool result = busService.DeleteBus(busNumber);

            if (result)
            {
                Console.WriteLine("Bus deleted successfully!");
            }
            else
            {
                Console.WriteLine("Bus could not be deleted. Check the bus number.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void DisplayRouteMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======= MANAGE ROUTES =======");
                Console.WriteLine("1. VIEW ALL ROUTES");
                Console.WriteLine("2. ADD ROUTE");
                Console.WriteLine("3. UPDATE ROUTE");
                Console.WriteLine("4. DELETE ROUTE");
                Console.WriteLine("5. RETURN TO ADMIN MENU");
                Console.Write("Enter your choice: ");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    ConsoleUtility.ErrorMessage();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        DisplayAllRoutes();
                        ConsoleUtility.PauseScreen();
                        break;

                    case 2:
                        AddRouteUI();
                        break;

                    case 3:
                        UpdateRouteUI();
                        break;

                    case 4:
                        DeleteRouteUI();
                        break;

                    case 5:
                        return;

                    default:
                        ConsoleUtility.ErrorMessage();
                        break;
                }
            }
        }

        public static void DisplayAllRoutes()                                /////////////////////////////////////
        {
            RouteService routeService = new RouteService();
            List<Route> routes = routeService.GetAllRoutes();

            if (routes.Count == 0)
            {
                Console.WriteLine("No routes found.");
                return;
            }

            Console.WriteLine("========== ALL ROUTES ==========");
            foreach (Route r in routes)
            {
                Console.WriteLine($"Route ID: {r.RouteId}");
                Console.WriteLine($"Starting Point: {r.StartingRoute}");
                Console.WriteLine($"Destination: {r.Destination}");
                Console.WriteLine($"Fare: {r.Fare}");
                Console.WriteLine("--------------------------------");
            }
        }

        public static void SearchRoutesByFareUI()
        {
            Console.Clear();
            Console.WriteLine("======= SEARCH ROUTES BY FARE =======");
            Console.Write("Enter minimum fare: Rs. ");
            if (!double.TryParse(Console.ReadLine(), out double minimumFare))
            {
                Console.WriteLine("Please enter a valid fare.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter maximum fare: Rs. ");
            if (!double.TryParse(Console.ReadLine(), out double maximumFare) ||
                minimumFare < 0 || maximumFare < minimumFare ||
                double.IsNaN(minimumFare) || double.IsInfinity(minimumFare) ||
                double.IsNaN(maximumFare) || double.IsInfinity(maximumFare))
            {
                Console.WriteLine("Enter a valid fare range (maximum must be at least the minimum).");
                ConsoleUtility.PauseScreen();
                return;
            }

            RouteService routeService = new RouteService();
            List<Route> routes = routeService.FindRoutesByFareRange(minimumFare, maximumFare);
            if (routes.Count == 0)
            {
                Console.WriteLine("No routes found in that fare range.");
            }
            else
            {
                foreach (Route route in routes)
                {
                    Console.WriteLine($"Route ID: {route.RouteId} | From: {route.StartingRoute} | To: {route.Destination} | Fare: Rs. {route.Fare}");
                }
            }

            ConsoleUtility.PauseScreen();
        }

        public static void SearchRoutesByKeywordUI()
        {
            Console.Clear();
            Console.WriteLine("======= SEARCH ROUTES BY KEYWORD =======");
            Console.Write("Enter a word or part of a starting point or destination: ");
            string query = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Search query cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            RouteService routeService = new RouteService();
            List<Route> routes = routeService.SearchRoutes(query);
            if (routes.Count == 0)
            {
                Console.WriteLine("No routes found matching your query.");
            }
            else
            {
                foreach (Route route in routes)
                {
                    Console.WriteLine($"Route ID: {route.RouteId} | From: {route.StartingRoute} | To: {route.Destination} | Fare: Rs. {route.Fare}");
                }
            }

            ConsoleUtility.PauseScreen();
        }

        public static void AddRouteUI()
        {
            Console.Clear();
            Console.WriteLine("======= ADD NEW ROUTE =======");

            RouteService routeService = new RouteService();

            Console.Write("Enter route ID: ");
            string routeId = (Console.ReadLine() ?? "").Trim();
            if (!routeService.IsValidRouteId(routeId))
            {
                Console.WriteLine("Error: Route ID cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (routeService.RouteIdExists(routeId))
            {
                Console.WriteLine("Error: This route ID already exists.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter starting point: ");
            string startingRoute = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(startingRoute))
            {
                Console.WriteLine("Error: Starting point cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter destination: ");
            string destination = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(destination))
            {
                Console.WriteLine("Error: Destination cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (startingRoute.Equals(destination, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: Starting point and destination cannot be the same.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter fare: ");
            double fare;
            if (!double.TryParse(Console.ReadLine(), out fare))
            {
                Console.WriteLine("Error: Invalid fare. Please enter a number.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (!routeService.IsValidFare(fare))
            {
                Console.WriteLine("Error: Fare must be greater than zero.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Route route = new Route(
                routeId,
                startingRoute,
                destination,
                fare
            );

            if (routeService.AddRoute(route))
            {
                Console.WriteLine("Route added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add route. Check the route details and ensure the route ID is unique.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void UpdateRouteUI()
        {
            Console.Clear();
            Console.WriteLine("======= UPDATE ROUTE =======");

            Console.Write("Enter the route ID to update: ");
            string routeId = (Console.ReadLine() ?? "").Trim();

            RouteService routeService = new RouteService();
            Route? existingRoute = routeService.FindRouteById(routeId);
            if (existingRoute == null)
            {
                Console.WriteLine("Error: Invalid route ID. This route is not present in the system.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter the new starting point: ");
            string newStartingRoute = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newStartingRoute))
            {
                Console.WriteLine("Error: Starting point cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (newStartingRoute.Equals(existingRoute.StartingRoute, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: This is already the starting point for this route.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter the new destination: ");
            string newDestination = (Console.ReadLine() ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newDestination))
            {
                Console.WriteLine("Error: Destination cannot be empty.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (newDestination.Equals(existingRoute.Destination, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: This is already the destination for this route.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (newDestination.Equals(newStartingRoute, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Error: Starting point and destination cannot be the same.");
                ConsoleUtility.PauseScreen();
                return;
            }

            Console.Write("Enter the new fare: ");
            double newFare;
            if (!double.TryParse(Console.ReadLine(), out newFare))
            {
                Console.WriteLine("Error: Invalid fare. Please enter a number.");
                ConsoleUtility.PauseScreen();
                return;
            }
            if (!routeService.IsValidFare(newFare))
            {
                Console.WriteLine("Error: Fare must be greater than zero.");
                ConsoleUtility.PauseScreen();
                return;
            }
            bool directionReversed =
                newStartingRoute.Equals(existingRoute.Destination, StringComparison.OrdinalIgnoreCase) &&
                newDestination.Equals(existingRoute.StartingRoute, StringComparison.OrdinalIgnoreCase);

            if (!directionReversed && Math.Abs(newFare - existingRoute.Fare) < 0.000001)
            {
                Console.WriteLine("Error: This is already the fare for this route.");
                ConsoleUtility.PauseScreen();
                return;
            }

            bool result = routeService.UpdateRoute(
                routeId,
                newStartingRoute,
                newDestination,
                newFare
            );

            if (result)
            {
                Console.WriteLine("Route updated successfully!");
            }
            else
            {
                Console.WriteLine("Unable to update route. Check the route ID, starting point, destination, and fare.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void DeleteRouteUI()
        {
            Console.Clear();
            Console.WriteLine("======= DELETE ROUTE =======");

            Console.Write("Enter the route ID to delete: ");
            string routeId = (Console.ReadLine() ?? "").Trim();

            RouteService routeService = new RouteService();
            bool result = routeService.DeleteRoute(routeId);

            if (result)
            {
                Console.WriteLine("Route deleted successfully!");
                Console.WriteLine("Buses assigned to this route are now unassigned.");
            }
            else
            {
                Console.WriteLine("Route could not be deleted. Check the route ID.");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void ViewAllBookingsUI()
        {
            Console.Clear();
            Console.WriteLine("======= ALL BOOKINGS =======");

            BookingService bookingService = new BookingService();
            List<Booking> bookings = bookingService.GetAllBookings();

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
                ConsoleUtility.PauseScreen();
                return;
            }

            foreach (Booking b in bookings)
            {
                Console.WriteLine($"Booking ID:      {b.BookingId}");
                Console.WriteLine($"Passenger ID:    {b.PassengerId}");
                Console.WriteLine($"Bus Number:      {b.BusNumber}");
                Console.WriteLine($"Tickets Booked:  {b.TicketsBooked}");
                Console.WriteLine($"Fare Charged:    Rs. {b.FareAtBooking}");
                Console.WriteLine($"Status:          {b.Status}");
                Console.WriteLine("-------------------------------------");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void SearchBookingUI()
        {
            Console. Clear();
            Console.WriteLine("======= SEARCH BOOKING =======");

            Console.Write("Enter Booking ID: ");
            string bookingId = (Console.ReadLine() ?? "").Trim();

            BookingService bookingService = new BookingService();
            Booking? booking = bookingService.FindBookingById(bookingId);

            if (booking == null)
            {
                Console.WriteLine("No booking found with this ID.");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Booking Found:");
                Console.WriteLine($"Booking ID:      {booking.BookingId}");
                Console.WriteLine($"Passenger ID:    {booking.PassengerId}");
                Console.WriteLine($"Bus Number:      {booking.BusNumber}");
                Console.WriteLine($"Fare Charged:    Rs. {booking.FareAtBooking}");
                Console.WriteLine($"Status:          {booking.Status}");
            }

            ConsoleUtility.PauseScreen();
        }

        public static void ViewRoutesByPageView()
        {
            Console.Clear();
            Console.WriteLine("ENTER THE SIZE OF PAGE OR RECORD ON ONE PAGE ");
            int size = int.Parse(Console.ReadLine());

            RouteService routeService = new RouteService();
            List<Route> routes = routeService.GetAllRoutes();

            if (routes.Count == 0)
            {
                Console.WriteLine("No routes found.");
                ConsoleUtility.PauseScreen();
                return;
            }


            if(size <= 0 )
            {
                Console.WriteLine("YOU CHOOSE 0 . INVALID RECORD CAN VIEW");

                return;
            }


            Console.WriteLine("========== ALL ROUTES  ==========");
            
            
                foreach (Route r in routes)
                {


                    Console.WriteLine($"Route ID: {r.RouteId}");
                    Console.WriteLine($"Starting Point: {r.StartingRoute}");
                    Console.WriteLine($"Destination: {r.Destination}");
                    Console.WriteLine($"Fare: {r.Fare}");
                    Console.WriteLine("--------------------------------");

                }
            
           

            ConsoleUtility.PauseScreen();








        }
    }
}
