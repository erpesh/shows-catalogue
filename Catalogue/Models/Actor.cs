using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Actor
    {
        protected int id;
        protected string fullName;
        protected DateOnly dateOfBirth;
        protected string nationality;
        protected List<string> filmography;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string FullName
        {
            get => fullName;
            set => fullName = value;
        }

        public DateOnly DateOfBirth
        {
            get => dateOfBirth;
            set => dateOfBirth = value;
        }

        public string Nationality
        {
            get => nationality;
            set => nationality = value;
        }

        public List<string> Filmography
        {
            get => filmography; 
            set => filmography = value; 
        }

        public Actor(string _fullName, DateOnly _dateOfBirth, string _nationality, List<string> filmography)
        {
            FullName = _fullName;
            DateOfBirth = _dateOfBirth;
            Nationality = _nationality;
            Filmography = filmography;
        }
    }
}
