using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class ShowRating
    {
        public string Username { get; set; }
        public int Rating { get; set; }

        public ShowRating(string username, int rating)
        {
            Rating = rating;
            Username = username;
        }
    }
}
