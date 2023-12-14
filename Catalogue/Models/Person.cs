using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Person
    {
        protected int id;
        protected string firstName;
        protected string lastName;
        protected DateOnly? dateOfBirth;
        protected string nationality;

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string FirstName
        {
            get => firstName;
            set => firstName = value;
        }

        public string LastName
        {
            get => lastName;
            set => lastName = value;
        }

        public DateOnly? DateOfBirth
        {
            get => dateOfBirth;
            set => dateOfBirth = value;
        }

        public string Nationality
        {
            get => nationality;
            set => nationality = value;
        }

        public Person(string _firstName, string _lastName, DateOnly _dateOfBirth, string _nationality)
        {
            FirstName = _firstName;
            LastName = _lastName;
            DateOfBirth = _dateOfBirth;
            Nationality = _nationality;
        }
    }
}
