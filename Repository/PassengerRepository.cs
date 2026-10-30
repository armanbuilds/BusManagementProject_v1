using Project_02_nov.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Project_02_nov.Repository
{
    internal class PassengerRepository
    {
        private readonly string _filePath = "D:\\2nd semester\\sir laeeq ,OOPS\\WEEK 5\\Project_02_nov\\DATA\\passengers.txt";

        public void Save(Passenger passenger)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(passenger.ToString());
            }
        }

        public void SaveAll(List<Passenger> passengers)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
                foreach (Passenger p in passengers)
                {
                    writer.WriteLine(p.ToString());
                }
            }
        }

        public List<Passenger> GetAll()
        {
            List<Passenger> passengers = new List<Passenger>();
            if (!File.Exists(_filePath))
            {
                return passengers;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    List<string> data = new List<string>(line.Split(','));

                    if (data.Count != 3)
                    {
                        continue;
                    }

                    int age;
                    if (int.TryParse(data[2], out age))
                    {
                        Passenger p = new Passenger(data[0],  data[1], age);

                        passengers.Add(p);
                    }
                }
            }

            return passengers;

        }

        public void ClearAll()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
