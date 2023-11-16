using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Actor : Person
    {
        public Actor(string firstName, string lastName, DateOnly dateOfBirth)
            : base(firstName, lastName, dateOfBirth)
        {
        }

    }

}
