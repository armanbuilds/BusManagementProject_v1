using System;
using System.Collections.Generic;
using System.Text;

namespace Project_02_nov.Models
{
    internal class Passenger
    {
        public String passengerId;
        public String name;
        public int age;

        public Passenger(string passengerId, string name, int age)
        {
            this.passengerId = passengerId;
            this.name = name;
            this.age = age;
        }

        public override string ToString()
        {
            return $"{passengerId},{name},{age}";
        }

    }
}
