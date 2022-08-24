using Dapper;
using DapperTrial.Models.DataModel;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.UserhomeServices
{
    public class UserhomeService : IUserHomeService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserhomeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public List<Vehicle> GetAllVehicle()
        {

            var sql = "Select distinct(VehicleType) From Vehicle";
            SqlConnection connection = new SqlConnection(_connectionString);


            var vehicles = connection.Query<Vehicle>(sql).ToList();


            return vehicles;
        }

        public List<Vehicle> GetAllVehicleFromType(string VehicleType)
        {

            var sql = "Select VehileName From Vehicle where VehicleType=@VehicleType";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@VehicleType", VehicleType);
            SqlConnection connection = new SqlConnection(_connectionString);


            var vehicles = connection.Query<Vehicle>(sql, parameters).ToList();


            return vehicles;
        }

        public List<Station> GetAllStation()
        {

            var sql = "Select * From Station";
            SqlConnection connection = new SqlConnection(_connectionString);


            var vehicles = connection.Query<Station>(sql).ToList();


            return vehicles;
        }

        public List<Vehicle> GetAllVehicleFromStation(string StationName)
        {

            var sql = "select Vehicle.VehileName from Station Join Vehicle ON Vehicle.WhereStored = Station.StationName where Station.Id = @StationName ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StationName", StationName);
            SqlConnection connection = new SqlConnection(_connectionString);


            var vehicles = connection.Query<Vehicle>(sql, parameters).ToList();


            return vehicles;
        }

    }
}
