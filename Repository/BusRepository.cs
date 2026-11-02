using Project_02_nov.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Project_02_nov.Repository
{
    internal class BusRepository
    {
        private readonly string _filePath = "D:\\2nd semester\\sir laeeq ,OOPS\\WEEK 5\\Project_02_nov\\DATA\\buses.txt";

        public void Save(Bus bus)
        {
            using (StreamWriter writer = new StreamWriter(_filePath , true))
            {
                writer.WriteLine(bus.ToString());
            }
        }

        public void SaveAll(List<Bus> buses)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
               foreach(Bus b in buses)
                {
                    writer.WriteLine(b.ToString());
                }
            }
        }

        public List<Bus> GetAll()
        {
            List<Bus> buses = new List<Bus>();
            if (!File.Exists(_filePath))
            {
                return buses;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    List<string> data = new List<string>(line.Split(','));

                    if (data.Count != 4)
                    {
                        continue;
                    }

                    int totalSeats;
                    int availableSeats;
                    if (int.TryParse(data[1], out totalSeats) && int.TryParse(data[2], out availableSeats))
                    {
                        string? routeId = string.IsNullOrWhiteSpace(data[3]) ? null : data[3].Trim();

                        Bus bus = new Bus(data[0].Trim(),totalSeats,availableSeats,routeId);

                        buses.Add(bus);
                    }
                }
            }

            return buses;

        }

        public void ClearAll()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
