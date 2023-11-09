using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class ShowRating
    {
        private User User { get; }
        private Show Show { get; }
        private int Rating { get; set; }

        public ShowRating(User user, Show show, int rating)
        {
            User = user;
            Show = show;
            Rating = rating;
        }
    }
}
