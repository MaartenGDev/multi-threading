using System;
using System.Threading.Tasks;

namespace Demo
{
    public class CityInformationPrinter
    {
        public async Task Print()
        {
            var client = new WeatherClient();
            var cities = await client.GetCities();
            
            Console.WriteLine($"Retrieved cities");

            foreach (var city in cities)
            {
                Console.WriteLine($"Retrieving data for city: {city.Name}");

                try
                {
                    await client.PrintStreetsOfCity(city.Name);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error occured by streets");
                }
                try
                {
                    await client.PrintBuildingsOfCity(city.Name);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error occured by city");
                }
            }
            
            Console.WriteLine("Thread can run other Tasks!");
        }
    }
}