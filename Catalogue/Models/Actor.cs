using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Actor : Person
    {
        public Actor(int _id, string _firstName, string _lastName, DateOnly _dateOfBirth)
            : base(_id, _firstName, _lastName, _dateOfBirth)
        {
        }

    }

}
