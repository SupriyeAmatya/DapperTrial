using DapperTrial.Models.DataModel;

namespace DapperTrial.Services.VehicleServices
{
    public interface IVehicleService
    {
        public void AddVehicle(Vehicle vehicle);
        IEnumerable<Vehicle> GetVehicles();

              Vehicle DetailsVehicle(int VehicleId);
        Vehicle GetVehicleById(int VehicleId);
        void UpdateVehicle(Vehicle Vehicle);
        void DeleteUser(int VehicleId);
    }
}
