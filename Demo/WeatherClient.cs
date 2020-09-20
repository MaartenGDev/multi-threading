using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Data;
using Newtonsoft.Json;

namespace Demo
{
    public class WeatherClient
    {
        private const string ApiUrl = "https://localhost:5001";

        public async Task<IEnumerable<City>> GetCities()
        {
            using (var client = new HttpClient())
            {
                var request  = await client.GetAsync($"{ApiUrl}/cities");
                var response = await request.Content.ReadAsStringAsync();
                
                var cities = JsonConvert.DeserializeObject<IEnumerable<City>>(response);

                return cities;   
            }
        }
        
        public async Task PrintStreetsOfCity(string cityName)
        {
            throw new Exception("example exception");
            using (var client = new HttpClient())
            {
                var request = await client.GetAsync($"{ApiUrl}/cities/{cityName}/streets");
                var response = await request.Content.ReadAsStringAsync();
    
                var streets = JsonConvert.DeserializeObject<IEnumerable<Street>>(response);

                foreach (var street in streets)
                {
                    Console.WriteLine($"{cityName} has the following street: " + street.Name);
                }   
            }
        }
        
        public async Task PrintBuildingsOfCity(string cityName)
        {
            throw new Exception("example exception");
            using (var client = new HttpClient())
            {
                var request = await client.GetAsync($"{ApiUrl}/cities/{cityName}/buildings");
                var response = await request.Content.ReadAsStringAsync();

                var buildings = JsonConvert.DeserializeObject<IEnumerable<Building>>(response);

                foreach (var building in buildings)
                {
                    Console.WriteLine($"{cityName} has the following building: " + building.Name);
                }    
            }
        }
    }
}