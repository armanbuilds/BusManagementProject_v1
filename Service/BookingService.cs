using Project_02_nov.Models;
using Project_02_nov.Repository;
using Project_02_nov.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.Service
{
     class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly BusRepository _busRepository;
        private readonly RouteRepository _routeRepository;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
            _busRepository = new BusRepository();
            _routeRepository = new RouteRepository();
        }

        public bool BookingIdExists(string bookingId)
        {
            if (string.IsNullOrWhiteSpace(bookingId))
            {
                return false;
            }

            List<Booking> bookings = _bookingRepository.GetAll();
            foreach (Booking b in bookings)
            {
                if (b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public string GenerateNextBookingId()
        {
            List<Booking> bookings = _bookingRepository.GetAll();
            int nextNumber = bookings.Count + 101;
            string newId = $"BK-{nextNumber}";

            while (BookingIdExists(newId))
            {
                nextNumber++;
                newId = $"BK-{nextNumber}";
            }

            return newId;
        }

        public Booking? BookTicket(string passengerId, string busNumber, string? customBookingId = null)
        {
            return BookTicket(passengerId, busNumber, 1, customBookingId);
        }

        public Booking? BookTicket(string passengerId, string busNumber, int ticketsBooked, string? customBookingId = null)
        {
            if (string.IsNullOrWhiteSpace(passengerId) || string.IsNullOrWhiteSpace(busNumber) || ticketsBooked <= 0)
            {
                return null;
            }

            string bookingId = string.IsNullOrWhiteSpace(customBookingId) ? GenerateNextBookingId()  : customBookingId.Trim();

            if (BookingIdExists(bookingId))
            {
                return null;
            }

            List<Bus> buses = _busRepository.GetAll();
            Bus? targetBus = null;

            foreach (Bus b in buses)
            {
                if (b.BusNumber.Equals(busNumber, StringComparison.OrdinalIgnoreCase))
                {
                    targetBus = b;
                    break;
                }
            }

            if (targetBus == null)
            {
                return null;
            }

            if (targetBus.AvailableSeats < ticketsBooked)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(targetBus.RouteId))
            {
                return null;
            }

            List<Route> routes = _routeRepository.GetAll();
            Route? assignedRoute = null;

            foreach (Route r in routes)
            {
                if (r.RouteId.Equals(targetBus.RouteId, StringComparison.OrdinalIgnoreCase))
                {
                    assignedRoute = r;
                    break;
                }
            }

            if (assignedRoute == null)
            {
                return null;
            }

            double fareCharged = assignedRoute.Fare;

            targetBus.AvailableSeats -= ticketsBooked;
            _busRepository.SaveAll(buses);

            Booking newBooking = new Booking(bookingId, passengerId.Trim(), targetBus.BusNumber, fareCharged, "Confirmed", ticketsBooked);
            _bookingRepository.Save(newBooking);
            DeleteCancelledBookingsForPassenger(passengerId);

            return newBooking;
        }

        public bool CancelBooking(string bookingId)
        {
            if (string.IsNullOrWhiteSpace(bookingId))
            {
                return false;
            }

            List<Booking> bookings = _bookingRepository.GetAll();
            Booking? targetBooking = null;

            foreach (Booking b in bookings)
            {
                if (b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase))
                {
                    targetBooking = b;
                    break;
                }
            }

            if (targetBooking == null)
            {
                return false;
            }

            if (targetBooking.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            targetBooking.Status = "Cancelled";
            _bookingRepository.SaveAll(bookings);

            List<Bus> buses = _busRepository.GetAll();
            foreach (Bus b in buses)
            {
                if (b.BusNumber.Equals(targetBooking.BusNumber, StringComparison.OrdinalIgnoreCase))
                {
                    if (b.AvailableSeats + targetBooking.TicketsBooked <= b.TotalSeats)
                    {
                        b.AvailableSeats += targetBooking.TicketsBooked;
                        _busRepository.SaveAll(buses);
                    }
                    break;
                }
            }

            return true;
        }

        public Booking? FindBookingById(string bookingId)
        {
            if (string.IsNullOrWhiteSpace(bookingId))
            {
                return null;
            }

            List<Booking> bookings = _bookingRepository.GetAll();
            foreach (Booking b in bookings)
            {
                if (b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase))
                {
                    return b;
                }
            }
            return null;
        }

        public List<Booking> GetBookingsByPassengerId(string passengerId)
        {
            List<Booking> results = new List<Booking>();
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return results;
            }

            List<Booking> bookings = _bookingRepository.GetAll();
            foreach (Booking b in bookings)
            {
                if (b.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(b);
                }
            }
            return results;
        }


        public void DeleteCancelledBooking(string bookingId)
        {
            if (string.IsNullOrWhiteSpace(bookingId))
            {
                return;
            }

            List<Booking> bookings = _bookingRepository.GetAll();

            Booking? targetBooking = null;

            foreach (Booking b in bookings)
            {
                if (b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase))
                {
                    targetBooking = b;
                    break;
                }
            }

            if (targetBooking == null)
            {
                return;
            }

            if (targetBooking.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                bookings.Remove(targetBooking);
                _bookingRepository.SaveAll(bookings);
            }
        }



        private void DeleteCancelledBookingsForPassenger(string passengerId)
        {
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return;
            }

            List<Booking> bookings = _bookingRepository.GetAll();

            List<Booking> cancelledBookings = new List<Booking>();

            foreach (Booking b in bookings)
            {
                if (b.PassengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase))
                {
                    if (b.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                    {
                        cancelledBookings.Add(b);
                    }
                }
            }

            if (cancelledBookings.Count > 0)
            {
                foreach (Booking b in cancelledBookings)
                {
                    bookings.Remove(b);
                }

                _bookingRepository.SaveAll(bookings);
            }
        }


        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }
    }
}
