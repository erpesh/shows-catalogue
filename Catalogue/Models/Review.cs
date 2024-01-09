﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Review
    {
        public string Username { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }

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
