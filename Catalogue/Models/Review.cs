using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Review
    {
        private string username;
        private int rating;
        private string? comment;
        private DateTime date;

        public string Username
        {
            get => username;
            set
            {
                if (value.Length > 20)
                    throw new ArgumentException("The username is too long.");
                username = value;
            }
        }
        public int Rating
        {
            get => rating;
            set
            {
                if (value < 1 || value > 10)
                    throw new ArgumentException("The rating must be between 1 and 10.");
                rating = value;
            }
        }
        public string? Comment 
        {
            get => comment;
            set
            {
                if (value != null && value.Length > 1000)
                    throw new ArgumentException("The comment is too long.");
            }
        }
        public DateTime Date
        {
            get => date;
            set => date = value;
        }

        public Review(string username, int rating, string? comment)
        {
            Rating = rating;
            Username = username;
            Comment = comment;
            Date = DateTime.Now;
        }

        public void Output()
        {
            Console.WriteLine($"  Username: {Username}");
            Console.WriteLine($"  Rating: {Rating}");
            if (Comment != null)
                Console.WriteLine($"  Comment: {Comment}");
            Console.WriteLine($"  Date: {Date.ToString("yyyy-mm-dd")}");
            Console.WriteLine();
        }
    }
}
