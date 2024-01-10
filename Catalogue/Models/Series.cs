using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Series : Show
    {
        private int? seasons;
        private int? episodes;
        private DateOnly? startDate;
        private DateOnly? endDate;

        public int? Seasons
        {
            get => seasons;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Seasons can be only a positive number in minutes.");
                if (value > 100)
                    throw new ArgumentException("Seasons can't be over 100.");
                seasons = value;
            }
        }
        public int? Episodes
        {
            get => episodes;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Episodes can be only a positive number in minutes.");
                if (value > 1000)
                    throw new ArgumentException("Episodes can't be longer than 1000 minutes.");
                episodes = value;
            }
        }
        public DateOnly? StartDate
        {
            get => startDate;
            set => startDate = value;
        }
        public DateOnly? EndDate
        {
            get => endDate;
            set => endDate = value;
        }

        public Series(
            string _title,
            string? _description,
            List<string> _genres,
            string _studio,
            string _director,
            List<string> _actors,
            int? _episodeLength,
            int? _seasons, 
            int? _episodes, 
            DateOnly? _startDate, 
            DateOnly? _endDate
        )
            : base(_title, _description, _genres, _studio, _director, _actors, _episodeLength)
        {
            Seasons = _seasons;
            Episodes = _episodes;
            StartDate = _startDate;
            EndDate = _endDate;
        }

        [JsonConstructor]
        public Series(
            int id,
            string title,
            string? description,
            List<string> genres,
            string studio,
            string director,
            List<string> actors,
            int? episodeLength,
            int? seasons,
            int? episodes,
            DateOnly? startDate,
            DateOnly? endDate,
            List<Review> reviews,
            double? avgRating
        )
            : base(title, description, genres, studio, director, actors, episodeLength)
        {
            Id = id;
            Seasons = seasons;
            Episodes = episodes;
            StartDate = startDate;
            EndDate = endDate;
            Reviews = reviews ?? new List<Review>();
            AvgRating = avgRating;
        }
        public override void Output()
        {
            base.Output();

            Console.WriteLine($"Seasons: {Seasons}");
            Console.WriteLine($"Episodes: {Episodes}");
            Console.WriteLine($"Start date: {StartDate}");
            Console.WriteLine($"End date: {EndDate}");

            OutputReviews();
        }
    }
}
