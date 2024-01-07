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
        public string Username { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        private DateTime Date = DateTime.Now;

        public Review(string username, int rating, string? comment)
        {
            Rating = rating;
            Username = username;
            Comment = comment;
        }
    }
}
