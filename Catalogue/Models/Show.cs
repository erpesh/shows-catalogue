using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Show
    {
        protected string Title { get; }
        protected string? Description { get; }
        protected List<string> Genres { get; }
        protected List<string> Studios { get; }
        protected Person Director { get; }
        protected List<ShowRating> Ratings { get; }
        protected double? AvgRating { get; set; }
        protected List<Actor> Actors { get; }
        private int? EpisodeLength { get; }

        public Show(
            string title, 
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            List<Actor> actors,
            int episodeLength
        )
        {
            Title = title;
            Description = description;
            Genres = genres;
            Studios = studios;
            Director = director;
            Actors = actors;
            EpisodeLength = episodeLength;
        }

        protected void UpdateFile()
        {

        }

        public void AddRating(User user, int rating)
        {
            var showRating = new ShowRating(user, this, rating);
            Ratings.Add(showRating);
        }
    }
}
