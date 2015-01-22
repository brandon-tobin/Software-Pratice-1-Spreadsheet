using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9\(\)]\A\d{1}$");
            if (!rgx.IsMatch(firstToken))
                throw new FormatException("Formula does not start with a valid char");
        }
    }
}
