using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ConsumptionRecordService));
            host.Open();
            Console.WriteLine("Service is open, press any key to close it.");
            Console.ReadKey();
            host.Close();
            Console.WriteLine("Service is closed");
        }
    }
}
