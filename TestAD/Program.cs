using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAD
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ADHelper.GetUserData("kapsch", "goweznia");

            Console.WriteLine(data.FirstName);
            Console.WriteLine(data.LastName);
            Console.WriteLine(data.Email);

            Console.ReadLine();
        }
    }
}
