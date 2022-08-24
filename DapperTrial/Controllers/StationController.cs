using DapperTrial.Models.DataModel;
using DapperTrial.Services.StationServices;
using Microsoft.AspNetCore.Mvc;

namespace DapperTrial.Controllers
{
    public class StationController : Controller
    {

        private IStationService _stationServices;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;



        public StationController(IStationService stationServices, IConfiguration configuration)
        {
            _stationServices = stationServices;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }
        public IActionResult Index()
        {
            var vehicles = _stationServices.GetStations();
            return View(vehicles);

        }

        public IActionResult add()
        {
            return View(new Station());
        }
        [HttpPost]
        public IActionResult add(Station station)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _stationServices.AddStation(station);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(station);
        }

        public ActionResult Details(int id)
        {
            Station model = _stationServices.DetailsStation(id);
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Station model = _stationServices.GetStationById(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Station station)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _stationServices.UpdateStation(station);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(station);
        }

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Station user = _stationServices.GetStationById(id);
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Station user = _stationServices.GetStationById(id);
                _stationServices.DeleteStation(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //      return RedirectToAction("Delete",
                //      new System.Web.Routing.RouteValueDictionary {
                //{ "id", id },
                //{ "saveChangesError", true } });
                return View();
            }

        }
    }
}
