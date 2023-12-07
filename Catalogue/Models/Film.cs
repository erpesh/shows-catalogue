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
            string _title,
            string _description,
            List<string> _genres,
            string _studio,
            Person _director,
            List<Actor> _actors,
            int _episodeLength,
            DateOnly _releaseDate
        )
            : base(_title, _description, _genres, _studio, _director, _actors, _episodeLength)
        {
            ReleaseDate = _releaseDate;
        }

        protected override void CopyProperties(Show source)
        {
            base.CopyProperties(source);
            if (source is Film filmSource)
            {
                ReleaseDate = filmSource.ReleaseDate;
            }

            UpdateFile();
        }
    }
}
