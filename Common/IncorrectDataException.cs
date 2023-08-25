using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class IncorrectDataException : System.Exception
    {
        public IncorrectDataException(string message) : base(message)
        {

        }

        public IncorrectDataException() : base("Nevalidni podaci pronadjeni!")
        {

        }
    }
}
