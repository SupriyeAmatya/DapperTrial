using DapperTrial.Models.DataModel;
using DapperTrial.Models.DTO;

namespace DapperTrial.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task AddVehicleAsync(VehicleDTO vehicle);
        IEnumerable<Vehicle> GetVehicles();
        public Vehicle DetailsVehicle(int VehicleId);
        //      Vehicle DetailsVehicle(int VehicleId);
        //Vehicle GetVehicleById(int VehicleId);
        //void UpdateVehicle(Vehicle Vehicle);
        //void DeleteUser(int VehicleId);
    }
}
