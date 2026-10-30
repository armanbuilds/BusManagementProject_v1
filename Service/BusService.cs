using Project_02_nov.Models;
using Project_02_nov.Repository;
using Project_02_nov.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Project_02_nov.Service
{
    internal class BusService
    {
        private readonly BusRepository _busRepository;

        public BusService()
        {
            _busRepository = new BusRepository();
        }

        public bool IsValidBusNumber(string busnumber)
        {
            if (string.IsNullOrWhiteSpace(busnumber))
            {
                return false;
            }
            return true;
        }

        public bool BusNumberExists(string busnumber)
        {
            List<Bus> buses = _busRepository.GetAll();
            if(buses == null || buses.Count == 0)
            {
                return false;
            }

            foreach(Bus b in buses)
            {
                if (b.BusNumber.Equals(busnumber, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsValidCapacity(int capacity)
        {
            if(capacity <= 0)
            {
                return false;
            }
            return true;
        }

        public bool AddBus(Bus bus)
        {
            if (bus == null)
            {
                return false;
            }

            if (!IsValidBusNumber(bus.BusNumber))
            {
                return false;
            }

            if (BusNumberExists(bus.BusNumber))
            {
                return false;
            }

            if (!IsValidCapacity(bus.TotalSeats))
            {
                return false;
            }

            if (bus.AvailableSeats != bus.TotalSeats)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(bus.RouteId))
            {
                RouteService routeService = new RouteService();
                if (!routeService.IsValidRouteId(bus.RouteId) || routeService.FindRouteById(bus.RouteId) == null)
                {
                    return false;
                }
            }

            _busRepository.Save(bus);
            return true;
        }

        public Bus? FindBusByNumber(string busNumber)
        {
            if (string.IsNullOrWhiteSpace(busNumber))
            {
                return null;
            }
            List<Bus> buses = _busRepository.GetAll();

            if (buses == null || buses.Count == 0)
            {
                return null;
            }

            foreach (Bus b in buses)
            {
                if (b.BusNumber.Equals(busNumber, StringComparison.OrdinalIgnoreCase))
                {
                    return b;
                }
            }
            return null;
        }

        public bool UpdateBus(string busNumber, string? newRouteId, int newTotalSeats)
        {
            if (!IsValidBusNumber(busNumber))
            {
                return false;
            }
            if (!IsValidCapacity(newTotalSeats))
            {
                return false;
            }

            string? routeToAssign = null;
            if (!string.IsNullOrWhiteSpace(newRouteId))
            {
                RouteService routeService = new RouteService();
                if (!routeService.IsValidRouteId(newRouteId) || routeService.FindRouteById(newRouteId) == null)
                {
                    return false;
                }
                routeToAssign = newRouteId.Trim();
            }

            List<Bus> buses = _busRepository.GetAll();

            foreach(Bus b in buses)
            {
                if (b.BusNumber.Equals(busNumber, StringComparison.OrdinalIgnoreCase))
                {
                    int bookedSeats = b.TotalSeats - b.AvailableSeats;

                    if (bookedSeats > newTotalSeats)
                    {
                        return false;
                    }

                    b.TotalSeats = newTotalSeats;
                    b.AvailableSeats = newTotalSeats - bookedSeats;
                    b.RouteId = routeToAssign;

                    _busRepository.SaveAll(buses);
                    return true;
                }
            }

            return false;
        }

        public bool DeleteBus(string busNumber)
        {
            if (!IsValidBusNumber(busNumber))
            {
                return false;
            }

            List<Bus> buses = _busRepository.GetAll();

            for (int i = 0; i < buses.Count; i++)
            {
                if (buses[i].BusNumber.Equals(busNumber, StringComparison.OrdinalIgnoreCase))
                {
                    buses.RemoveAt(i);
                    _busRepository.SaveAll(buses);
                    return true;
                }
            }
            return false;
        }

        public List<Bus> GetAllBuses()
        {
            return _busRepository.GetAll();
        }
    }
}
