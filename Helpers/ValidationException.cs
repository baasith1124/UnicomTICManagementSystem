using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Helpers
{
    // class for custom validation exceptions
    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }
    }
}
