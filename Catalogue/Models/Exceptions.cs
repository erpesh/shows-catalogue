using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Models
{
    public class MediaTypeException : Exception
    {
        public MediaTypeException(string[] validTypes)
            : base($"Please specify a valid type ({string.Join(", ", validTypes)}).")
        {
        }
    }
    public class InvalidCommandException : Exception
    {
        public InvalidCommandException(string message) : base(message)
        {
        }
    }
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message)
        {
        }
    }
}
