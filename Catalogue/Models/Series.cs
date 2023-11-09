using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Series : Show
    {
        private int? Seasons { get; set; }
        private int? Episodes { get; set; }
        private DateOnly? StartDate { get; set; }
        private DateOnly? EndDate { get; set; }

        public Series(
            string title,
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            int ratings,
            double avgRating,
            List<Actor> actors,
            int episodeLength,
            int seasons, 
            int episodes, 
            DateOnly startDate, 
            DateOnly endDate
        )
            : base(title, description, genres, studios, director, actors, episodeLength)
        {
            Seasons = seasons;
            Episodes = episodes;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
