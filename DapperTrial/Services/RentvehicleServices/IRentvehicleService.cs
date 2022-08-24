using DapperTrial.Models.DataModel;

namespace DapperTrial.Services.RentvehicleServices
{
    public interface IRentvehicleService
    {
        public IEnumerable<Vehicle> RentVehicleList();
    }
}
