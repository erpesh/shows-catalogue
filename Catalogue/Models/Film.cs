using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Film : Show
    {
        private DateOnly? releaseDate;
        public DateOnly? ReleaseDate 
        { 
            get => releaseDate; 
            set => releaseDate = value; 
        }

        public Film(
            string _title,
            string? _description,
            List<string> _genres,
            string _studio,
            string _director,
            List<string> _actors,
            int? _episodeLength,
            DateOnly? _releaseDate
        )
            : base(_title, _description, _genres, _studio, _director, _actors, _episodeLength)
        {
            ReleaseDate = _releaseDate;
        }

        [JsonConstructor]
        public Film(
            int id,
            string title,
            string? description,
            List<string> genres,
            string studio,
            string director,
            List<string> actors,
            int? episodeLength,
            DateOnly? releaseDate,
            List<Review> reviews,
            double? avgRating
        )
            : base(title, description, genres, studio, director, actors, episodeLength)
        {
            Id = id;
            ReleaseDate = releaseDate;
            Reviews = reviews ?? new List<Review>();
            AvgRating = avgRating;
        }

        public override void Output()
        {
            base.Output();

            Console.WriteLine($"Release date: {ReleaseDate}");

            OutputReviews();
        }
    }
}
