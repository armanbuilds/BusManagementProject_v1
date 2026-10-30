using Project_02_nov.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Project_02_nov.Repository
{
    internal class BookingRepository
    {
        private readonly string _filePath = "D:\\2nd semester\\sir laeeq ,OOPS\\WEEK 5\\Project_02_nov\\DATA\\bookings.txt";

        public void Save(Booking booking)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true))
            {
                writer.WriteLine(booking.ToString());
            }
        }

        public void SaveAll(List<Booking> bookings)
        {
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
                foreach (Booking b in bookings)
                {
                    writer.WriteLine(b.ToString());
                }
            }
        }

        public List<Booking> GetAll()
        {
            List<Booking> bookings = new List<Booking>();
            if (!File.Exists(_filePath))
            {
                return bookings;
            }

            using (StreamReader reader = new StreamReader(_filePath))
            {
                string? line;

                while ((line = reader.ReadLine()) != null)
                {
                    List<string> data = new List<string>(line.Split(','));

                    if (data.Count != 5 && data.Count != 6)
                    {
                        continue;
                    }

                    double fareAtBooking;
                    int ticketsBooked = 1;
                    bool validTicketCount = data.Count == 5 || int.TryParse(data[5], out ticketsBooked);
                    if (double.TryParse(data[3], out fareAtBooking) && validTicketCount && ticketsBooked > 0)
                    {
                        Booking b = new Booking(data[0], data[1],data[2], fareAtBooking,data[4],ticketsBooked);

                        bookings.Add(b);
                    }
                }
            }

            return bookings;

        }

        public void ClearAll()
        {
            File.WriteAllText(_filePath, string.Empty);
        }
    }
}
