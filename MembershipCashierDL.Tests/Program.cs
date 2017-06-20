using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MembershipCashierDL.Tests
{
    public static class Program
    {
        public static void Main()
        {
            HostForServices.Instance.StartServices();

            Console.WriteLine("The service is ready at {0}", "http://localhost:9812");
            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();
        }
    }
}
