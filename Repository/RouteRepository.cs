using Project_02_nov.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Project_02_nov.Repository
{
    internal class RouteRepository
    {
        private readonly string _filePath = "D:\\2nd semester\\sir laeeq ,OOPS\\WEEK 5\\Project_02_nov\\DATA\\routes.txt";

        public void Save(Route route)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(route.ToString());
            }
        }

        public void SaveAll(List<Route> routes)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
                foreach (Route r in routes)
                {
                    writer.WriteLine(r.ToString());
                }
            }
        }

        public List<Route> GetAll()
        {
            List<Route> routes = new List<Route>();
            if (!File.Exists(_filePath))
            {
                return routes;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    List<string> data = new List<string>(line.Split(','));

                    if (data.Count != 4)
                    {
                        continue;
                    }

                    double fare;
                    if (double.TryParse(data[3], out fare))
                    {
                        Route route = new Route(data[0], data[1],data[2],fare);

                        routes.Add(route);
                    }
                }
            }

            return routes;

        }

        public void ClearAll()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
