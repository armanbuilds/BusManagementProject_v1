using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.Models
{
    internal class Route
    {
        public string RouteId;
        public string StartingRoute;
        public string Destination;

        public double Fare;

        public Route( string RouteId , string StartingRoute ,string Destination, double fare )
        {
            this.RouteId = RouteId;
            this.StartingRoute = StartingRoute;
            this.Destination = Destination;
            this.Fare = fare;
        }

        public override string ToString()
        {
            return $"{RouteId},{StartingRoute},{Destination},{Fare}";
        }
    }
}
