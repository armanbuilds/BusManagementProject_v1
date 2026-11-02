using Project_02_nov.Models;
using Project_02_nov.Repository;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_02_nov.Services
{
    internal class RouteService
    {
        private readonly RouteRepository _routeRepository;
    



        public RouteService()
        {
            _routeRepository = new RouteRepository();
            
        }

        public bool IsValidFare(double fare)
        {

            if (double.IsNaN(fare) || double.IsInfinity(fare))
            {
                return false;
            }
            if (fare <= 0)
            {
                return false;
            }

            return true;
        }

        public bool AreValidLocations(string startingRoute, string destination)
        {
            
            if(string.IsNullOrWhiteSpace(startingRoute) || string.IsNullOrWhiteSpace(destination))
            {
                return false;
            }

            if(startingRoute.Equals(destination , StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        public bool RouteIdExists(string routeId)
        {
               List<Route> routes =  _routeRepository.GetAll();

            if(routes.Count == 0 || routes == null)
            {
                return false;
            }
            foreach (Route r in routes)
            {
                if (r.RouteId.Equals( routeId , StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;

        }

        public bool IsValidRouteId(string routeId)
        {
           if (string.IsNullOrWhiteSpace(routeId))
            {
                return false;
            }
          
            return true;
        }

        public Route? FindRouteById(string routeId)
        {

            if (string.IsNullOrWhiteSpace(routeId))
            {
                return null;
            }
            List<Route> routes = _routeRepository.GetAll();

            if (routes == null || routes.Count == 0)
            {
                return null;
            }

            foreach (Route r in routes)
            {

                if(r.RouteId.Equals(routeId , StringComparison.OrdinalIgnoreCase))
                {
                    return r;
                }
                
            }
            return null;
        }

        public bool AddRoute(Route route)
        {

            if(route == null)
            {
                return false;
            }

            if(!IsValidRouteId(route.RouteId))
            {
                return false;
            }

            if(RouteIdExists(route.RouteId))
            {
                return false;
            }

            if(!AreValidLocations(route.StartingRoute,route.Destination))
            {
                return false;
            }

            if(!IsValidFare(route.Fare))
            {
                return false;
            }

            _routeRepository.Save(route);
            return true;

        }

        public bool UpdateRouteFare(string routeId , double fare)
        {

           if (!IsValidRouteId(routeId))
             {
            return false;
            }

           if(!IsValidFare(fare))
            {
                return false;
            }

           
            List<Route> routes = _routeRepository.GetAll();
            
            foreach(Route r in routes)
            {
                if(r.RouteId.Equals(routeId, StringComparison.OrdinalIgnoreCase))
                {
                    r.Fare =  fare;
                    _routeRepository.SaveAll(routes);
                        return true;
                }
            }

            return false;

        }



        public bool UpdateRoute(
            string routeId,
            string newStartingRoute,
            string newDestination,
            double newFare)
        {
            if (!IsValidRouteId(routeId))
            {
                return false;
            }

            if (!AreValidLocations(newStartingRoute, newDestination))
            {
                return false;
            }

            if (!IsValidFare(newFare))
            {
                return false;
            }

            List<Route> routes = _routeRepository.GetAll();

            foreach (Route r in routes)
            {
                if (r.RouteId.Equals(
                    routeId, StringComparison.OrdinalIgnoreCase))
                {
                    r.StartingRoute = newStartingRoute;
                    r.Destination = newDestination;
                    r.Fare = newFare;

                    _routeRepository.SaveAll(routes);
                    return true;
                }
            }

            return false;
        }


        public bool UpdateRouteDetails(string routeId , string starting , string ending)
        {
            if (!IsValidRouteId(routeId))
            {
                return false;
            }
            if(!AreValidLocations(starting , ending))
            {
                return false;
            }

            List<Route> routes = _routeRepository.GetAll();

            foreach (Route r in routes)
            {
                if (r.RouteId.Equals(routeId, StringComparison.OrdinalIgnoreCase))
                {
                    r.StartingRoute = starting;
                    r.Destination = ending;
                    _routeRepository.SaveAll(routes);
                    return true;
                }
            }

            return false;

        }


        public bool DeleteRoute(string routeId)
        {
            if (!IsValidRouteId(routeId))
            {
                return false;
            }

            List<Route> routes = _routeRepository.GetAll();

            Route? routeToDelete = null;

            foreach (Route r in routes)
            {
                if (r.RouteId.Equals(
                    routeId, StringComparison.OrdinalIgnoreCase))
                {
                    routeToDelete = r;
                    break;
                }
            }
            if (routeToDelete == null)
            {
                return false;
            }

            BusRepository busRepository = new BusRepository();
            List<Bus> buses = busRepository.GetAll();


            foreach (Bus b in buses)
            {
                if (b.RouteId != null &&
                    b.RouteId.Equals(
                        routeId, StringComparison.OrdinalIgnoreCase))
                {
                    b.RouteId = null;
                }
            }


            busRepository.SaveAll(buses);
            routes.Remove(routeToDelete);


            _routeRepository.SaveAll(routes);

            return true;
        }

        public List<Route> GetAllRoutes()
        {
            return _routeRepository.GetAll();
        }

        public List<Route> FindRoutesByFareRange(double minimumFare, double maximumFare)
        {
            List<Route> matchingRoutes = new List<Route>();

            if (double.IsNaN(minimumFare) || double.IsInfinity(minimumFare) ||
                double.IsNaN(maximumFare) || double.IsInfinity(maximumFare) ||
                minimumFare < 0 || maximumFare < minimumFare)
            {
                return matchingRoutes;
            }

            foreach (Route route in _routeRepository.GetAll())
            {
                if (route.Fare >= minimumFare && route.Fare <= maximumFare)
                {
                    matchingRoutes.Add(route);
                }
            }

            return matchingRoutes;
        }

        public List<Route> SearchRoutes(string query)
        {
            List<Route> matchingRoutes = new List<Route>();

            if (string.IsNullOrWhiteSpace(query))
            {
                return matchingRoutes;
            }

            query = query.Trim();
            foreach (Route route in _routeRepository.GetAll())
            {
                if (route.StartingRoute.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    route.Destination.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    route.RouteId.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    matchingRoutes.Add(route);
                }
            }

            return matchingRoutes;
        }


    }
}
