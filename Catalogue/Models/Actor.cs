using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Actor : Person
    {
        public Actor(string firstName, string lastName, DateTime dateOfBirth)
            : base(firstName, lastName, dateOfBirth)
        {
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine("Actor");
        }
    }

}
