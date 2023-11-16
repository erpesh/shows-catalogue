using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Person
    {
        protected string FirstName { get; set; }
        protected string LastName { get; set; }
        protected DateOnly? DateOfBirth { get; set; }

        public Person(string firstName, string lastName, DateOnly dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }

}
