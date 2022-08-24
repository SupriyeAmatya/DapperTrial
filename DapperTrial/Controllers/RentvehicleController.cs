using DapperTrial.Services.RentvehicleServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperTrial.Controllers
{
    public class RentvehicleController : Controller
    {

        private readonly ILogger<RentvehicleController> _logger;
        private IRentvehicleService _rentService;
        public RentvehicleController(ILogger<RentvehicleController> logger, IRentvehicleService rentService)
        {
            _logger = logger;
            _rentService = rentService;
        }
        public IActionResult Index()
        {
            ViewBag.AllVehiclesList = _rentService.RentVehicleList();
            return View();
        }
    }
}
