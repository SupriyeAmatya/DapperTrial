using DapperTrial.Models.DataModel;
using DapperTrial.Models.DTO;
using DapperTrial.Services.VehicleServices;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DapperTrial.Controllers
{
    public class VehicleController : Controller
    {
        private IVehicleService _VehicleService;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;



        public VehicleController(IVehicleService VehicleService, IConfiguration configuration)
        {
            _VehicleService = VehicleService;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }
        public IActionResult Index()
        {
           
            var vehicles = _VehicleService.GetVehicles();
            return View(vehicles);


        }

        public IActionResult add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult add(VehicleDTO vehicle)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    _VehicleService.AddVehicleAsync(vehicle);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(vehicle);
        }

        public ActionResult Details(int id)
        {
            Vehicle model = _VehicleService.DetailsVehicle(id);
            return View(model);
        }

        //public ActionResult Edit(int id)
        //{
        //    Vehicle model = _VehicleService.GetVehicleById(id);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult Edit(Vehicle vehicle)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _VehicleService.UpdateVehicle(vehicle);
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //    }
        //    return View(vehicle);
        //}

        //public ActionResult Delete(int id, bool? saveChangesError)
        //{
        //    if (saveChangesError.GetValueOrDefault())
        //    {
        //        ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
        //    }
        //    Vehicle user = _VehicleService.GetVehicleById(id);
        //    return View(user);
        //}
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        Vehicle user = _VehicleService.GetVehicleById(id);
        //        _VehicleService.DeleteUser(id);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        //      return RedirectToAction("Delete",
        //        //      new System.Web.Routing.RouteValueDictionary {
        //        //{ "id", id },
        //        //{ "saveChangesError", true } });
        //        return View();
        //    }

        //}
    }
}
