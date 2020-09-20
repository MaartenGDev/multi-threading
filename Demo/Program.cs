using System;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var printer = new CityInformationPrinter();
            await printer.Print();
            
            Console.ReadKey();
        }
    }
}