using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

        [JsonConstructor]
        public Actor(int id, string fullName, DateOnly dateOfBirth, string nationality, List<string> filmography)
        {
            Id = id;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Nationality = nationality;
            Filmography = filmography;
        }

        public void Output()
        {
            Console.WriteLine($"Full name: {FullName}");
            Console.WriteLine($"Date of birth: {DateOfBirth}");
            Console.WriteLine($"Nationality: {Nationality}");
            Console.WriteLine($"Filmography: {string.Join(", ", Filmography)}");
        }
    }
}
