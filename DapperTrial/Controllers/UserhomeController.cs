using DapperTrial.Services.UserhomeServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperTrial.Controllers
{
    public class UserhomeController : Controller
    {
        private readonly ILogger<UserhomeController> _logger;
        private IUserHomeService _UserhomeService;
        public UserhomeController(ILogger<UserhomeController> logger, IUserHomeService UserhomeService)
        {
            _logger = logger;
            _UserhomeService = UserhomeService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AllVehicles()
        {
      
       
            return View();
        }
        [HttpPost]
        public IActionResult AllVehicles(string VehicleType)
        {
            //ViewBag.AllVehicleType = VehicleType;

            var VehicleTypeList = _UserhomeService.GetAllVehicleFromType(VehicleType);
            //ViewBag.VehicleList = VehicleTypeList;
            
            return View(VehicleTypeList);
        }

      
        public IActionResult AllStations()
        {
            ViewBag.AllVehiclesListStation = new SelectList(_UserhomeService.GetAllStation(),"Id", "StationName");
            return View();
        }
        [HttpPost]
        public IActionResult AllStations(string station)
        {
          
            ViewBag.AllVehiclesListStation = new SelectList(_UserhomeService.GetAllStation(), "Id", "StationName");
            var VehicleSationList = _UserhomeService.GetAllVehicleFromStation(station);
            ViewBag.VehicleStList = VehicleSationList;
            return View();
        }
    }
}
