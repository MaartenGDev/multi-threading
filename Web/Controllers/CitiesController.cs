using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly City[] _cities =
        {
            new City {Name = "Arnhem", Country = "Netherlands"},
            new City {Name = "Utrecht", Country = "Netherlands"},
        };

        private readonly Dictionary<string, Building[]> _buildingsByCity = new Dictionary<string, Building[]>
        {
            {
                "Arnhem", new[]
                {
                    new Building {Name = "Station", Location = "Renssenstraat 18, 6814 CM Arnhem"},
                    new Building {Name = "HAN", Location = "Ruitenberglaan 31, 6826 CC Arnhem"}
                }
            },
            {
                "Utrecht", new[]
                {
                    new Building {Name = "Station", Location = "Stationshal 12, 3511 CE Utrecht"},
                    new Building {Name = "Hotel Karel", Location = "Geertebolwerk 1, 3511 XA Utrecht"}
                }
            }
        };
        
        private readonly Dictionary<string, Street[]> _streetsByCity = new Dictionary<string, Street[]>
        {
            {
                "Arnhem", new[]
                {
                    new Street {Name = "Renssenstraat, 6814 CM Arnhem"},
                    new Street {Name = "Ruitenberglaan, 6826 CC Arnhem"}
                }
            },
            {
                "Utrecht", new[]
                {
                    new Street {Name = "Stationshal, 3511 CE Utrecht"},
                    new Street {Name = "Geertebolwerk, 3511 XA Utrecht"}
                }
            }
        };

        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCities()
        {
            return _cities;
        }

        [HttpGet("{city}/buildings")]
        public ActionResult<IEnumerable<Building>> GetBuildsOfCity(string city)
        {
            Thread.Sleep(1000);
            return _buildingsByCity[city];
        }
        
        [HttpGet("{city}/streets")]
        public ActionResult<IEnumerable<Street>> GetStreetsOfCity(string city)
        {
            Thread.Sleep(1000);
            return _streetsByCity[city];
        }
    }
}