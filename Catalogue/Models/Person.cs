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

        public Person(int _id, string _firstName, string _lastName, DateOnly _dateOfBirth)
        {
            Id = _id;
            FirstName = _firstName;
            LastName = _lastName;
            DateOfBirth = _dateOfBirth;
        }
    }

}
