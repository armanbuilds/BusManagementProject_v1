using Project_02_nov.Models;
using Project_02_nov.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.Service
{
    internal class PassengerService
    {
        private readonly PassengerRepository _passengerRepository;

        public PassengerService()
        {
            _passengerRepository = new PassengerRepository();
        }

        public bool IsValidPassengerId(string passengerId)
        {
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return false;
            }
            return true;
        }

        public bool PassengerIdExists(string passengerId)
        {
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return false;
            }

            List<Passenger> passengers = _passengerRepository.GetAll();
            foreach (Passenger p in passengers)
            {
                if (p.passengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddPassenger(Passenger passenger)
        {
            if (passenger == null)
            {
                return false;
            }

            if (!IsValidPassengerId(passenger.passengerId))
            {
                return false;
            }

            if (PassengerIdExists(passenger.passengerId))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(passenger.name))
            {
                return false;
            }

            if (passenger.age <= 0)
            {
                return false;
            }

            _passengerRepository.Save(passenger);
            return true;
        }

        public Passenger? FindPassengerById(string passengerId)
        {
            if (string.IsNullOrWhiteSpace(passengerId))
            {
                return null;
            }

            List<Passenger> passengers = _passengerRepository.GetAll();
            foreach (Passenger p in passengers)
            {
                if (p.passengerId.Equals(passengerId, StringComparison.OrdinalIgnoreCase))
                {
                    return p;
                }
            }
            return null;
        }

        public List<Passenger> GetAllPassengers()
        {
            return _passengerRepository.GetAll();
        }
    }
}
