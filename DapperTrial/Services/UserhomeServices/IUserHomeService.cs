using DapperTrial.Models.DataModel;

namespace DapperTrial.Services.UserhomeServices
{
    public interface IUserHomeService

    {
        public List<Vehicle> GetAllVehicle();
        public List<Vehicle> GetAllVehicleFromType(string VehicleType);
        public List<Station> GetAllStation();
        public List<Vehicle> GetAllVehicleFromStation(string StationName);
    }
}
