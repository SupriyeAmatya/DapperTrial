using Dapper;
using DapperTrial.Models.DataModel;
using DapperTrial.Models.ViewModel;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.RentvehicleServices
{
    public class RentvehicleService: IRentvehicleService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public RentvehicleService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public IEnumerable<Vehicle> RentVehicleList()
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            IList<Vehicle> List = new List<Vehicle>();
            var data = connection.Query<Vehicle>("select * from Vehicle");

            foreach (var vehicle in data)
            {
                List.Add(new Vehicle()
                {
                 VehicleName = vehicle.VehicleName
                });
            }
            return List;

        }


        //public void rentvehicle(RentVehicleViewModel vehicle)
        //{
        //    SqlConnection connection = new SqlConnection(_connectionString);
        //    var userData = new RentVehicle()
        //    {
        //        VehicleName = vehicle.VehicleName,
        //        PlateNo = vehicle.PlateNo,
        //        RentPerHr = vehicle.RentPerHr,
        //        RentPerDay = vehicle.RentPerDay,
        //        RentPerWeek = vehicle.RentPerWeek,
        //        LateFee = vehicle.LateFee,
        //        Availability = vehicle.Availability,
        //        VehicleType = vehicle.VehicleType,
        //        VehicleKind = vehicle.VehicleKind,
        //        WhereStored = vehicle.WhereStored,
        //        Tracker = vehicle.Tracker,

        //    };
        //    string sqlQuery = "Insert Into Vehicle ( VehileName, PlateNo, RentPerHr, RentPerDay, RentPerWeek, LateFee, Availability, VehicleType, VehicleKind, WhereStored, Tracker ) Values( @VehileName, @PlateNo, @RentPerHr, @RentPerDay, @RentPerWeek, @LateFee, @Availability, @VehicleType, @VehicleKind, @WhereStored, @Tracker)";
        //    DynamicParameters parameters = new DynamicParameters();
        //    parameters.Add("@VehileName", userData.VehileName);
        //    parameters.Add("@PlateNo", userData.PlateNo);
        //    parameters.Add("@RentPerHr", userData.RentPerHr);
        //    parameters.Add("@RentPerDay", userData.RentPerDay);
        //    parameters.Add("@RentPerWeek", userData.RentPerWeek);
        //    parameters.Add("@LateFee", userData.LateFee);
        //    parameters.Add("@Availability", userData.Availability);
        //    parameters.Add("@VehicleType", userData.VehicleType);
        //    parameters.Add("@VehicleKind", userData.VehicleKind);
        //    parameters.Add("@WhereStored", userData.WhereStored);
        //    parameters.Add("@Tracker", userData.Tracker);
        //    var rowsAffected = connection.Execute(sqlQuery, parameters);
        //    connection.Close();
        //}
    }
}
