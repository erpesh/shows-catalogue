using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    class Film : Show
    {
        private DateOnly? releaseDate;
        public DateOnly? ReleaseDate 
        { 
            get => releaseDate; 
            set => releaseDate = value; 
        }

        public Film(
            string title,
            string description,
            List<string> genres,
            List<string> studios,
            Person director,
            List<Actor> actors,
            int episodeLength,
            DateOnly releaseDate
        )
            : base(title, description, genres, studios, director, actors, episodeLength)
        {
            ReleaseDate = releaseDate;
        }

        protected override void CopyProperties(Show source)
        {
            base.CopyProperties(source);
            if (source is Film filmSource)
            {
                ReleaseDate = filmSource.ReleaseDate;
            }
        }
    }
}
