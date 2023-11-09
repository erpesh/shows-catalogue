using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Film : Show
    {
        private DateOnly? ReleaseDate { get; }

        public Film(
            string title,
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            int ratings,
            double avgRating,
            List<Actor> actors,
            int episodeLength,
            DateOnly releaseDate
        )
            : base(title, description, genres, studios, director, actors, episodeLength)
        {
            ReleaseDate = releaseDate;
        }
    }
}
