﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class Actor : Person
    {
        public Actor(string _firstName, string _lastName, DateOnly _dateOfBirth, string _nationality)
            : base(_firstName, _lastName, _dateOfBirth, _nationality)
        {
        }

    }

}
