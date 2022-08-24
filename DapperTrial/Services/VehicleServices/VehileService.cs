using Dapper;
using DapperTrial.Models.DataModel;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.VehicleServices
{
    public class VehileService : IVehicleService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public VehileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public IEnumerable<Vehicle> GetVehicles()
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            IList<Vehicle> List = new List<Vehicle>();
            var data = connection.Query<Vehicle>("select * from Vehicle");

            foreach (var vehicle in data)
            {
                List.Add(new Vehicle()
                {
                    Id = vehicle.Id,

                    VehileName = vehicle.VehileName,
                    PlateNo = vehicle.PlateNo,
                    RentPerHr = vehicle.RentPerHr,
                    RentPerDay = vehicle.RentPerDay,
                    RentPerWeek = vehicle.RentPerWeek,
                    LateFee = vehicle.LateFee,
                    Availability = vehicle.Availability,
                    VehicleType = vehicle.VehicleType,
                    VehicleKind = vehicle.VehicleKind,
                    WhereStored = vehicle.WhereStored,
                    Tracker = vehicle.Tracker,
                });
            }
            return List;

        }
        public void AddVehicle(Vehicle vehicle)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var userData = new Vehicle()
            {
                VehileName = vehicle.VehileName,
                PlateNo = vehicle.PlateNo,
                RentPerHr = vehicle.RentPerHr,
                RentPerDay = vehicle.RentPerDay,
                RentPerWeek = vehicle.RentPerWeek,
                LateFee = vehicle.LateFee,
                Availability = vehicle.Availability,
                VehicleType = vehicle.VehicleType,
                VehicleKind = vehicle.VehicleKind,
                WhereStored = vehicle.WhereStored,
                Tracker = vehicle.Tracker,

            };
            string sqlQuery = "Insert Into Vehicle ( VehileName, PlateNo, RentPerHr, RentPerDay, RentPerWeek, LateFee, Availability, VehicleType, VehicleKind, WhereStored, Tracker ) Values( @VehileName, @PlateNo, @RentPerHr, @RentPerDay, @RentPerWeek, @LateFee, @Availability, @VehicleType, @VehicleKind, @WhereStored, @Tracker)";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@VehileName", userData.VehileName);
            parameters.Add("@PlateNo", userData.PlateNo);
            parameters.Add("@RentPerHr", userData.RentPerHr);
            parameters.Add("@RentPerDay", userData.RentPerDay);
            parameters.Add("@RentPerWeek", userData.RentPerWeek);
            parameters.Add("@LateFee", userData.LateFee);
            parameters.Add("@Availability", userData.Availability);
            parameters.Add("@VehicleType", userData.VehicleType);
            parameters.Add("@VehicleKind", userData.VehicleKind);
            parameters.Add("@WhereStored", userData.WhereStored);
            parameters.Add("@Tracker", userData.Tracker);
            var rowsAffected = connection.Execute(sqlQuery, parameters);
            connection.Close();
        }

        public Vehicle DetailsVehicle(int VehicleId)
        {
            Vehicle vehicle = new Vehicle();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                vehicle = connection.Query<Vehicle>("Select * From Vehicle WHERE Id =" + VehicleId, new { VehicleId }).SingleOrDefault();//only takes single value or default value
            }
            Vehicle veh = new Vehicle()
            {
                Id = VehicleId,
                VehileName = vehicle.VehileName,
                PlateNo = vehicle.PlateNo,
                RentPerHr = vehicle.RentPerHr,
                RentPerDay = vehicle.RentPerDay,
                RentPerWeek = vehicle.RentPerWeek,
                LateFee = vehicle.LateFee,
                Availability = vehicle.Availability,
                VehicleType = vehicle.VehicleType,
                VehicleKind = vehicle.VehicleKind,
                WhereStored = vehicle.WhereStored,
                Tracker = vehicle.Tracker,
            };
            return veh;
        }

        public Vehicle GetVehicleById(int VehicleId)
        {
            Vehicle vehicle = new Vehicle();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                vehicle = connection.Query<Vehicle>("Select * From Vehicle WHERE Id =" + VehicleId, new { VehicleId }).SingleOrDefault();//only takes single value or default value
            }
            Vehicle veh = new Vehicle()
            {
                Id = VehicleId,
                VehileName = vehicle.VehileName,
                PlateNo = vehicle.PlateNo,
                RentPerHr = vehicle.RentPerHr,
                RentPerDay = vehicle.RentPerDay,
                RentPerWeek = vehicle.RentPerWeek,
                LateFee = vehicle.LateFee,
                Availability = vehicle.Availability,
                VehicleType = vehicle.VehicleType,
                VehicleKind = vehicle.VehicleKind,
                WhereStored = vehicle.WhereStored,
                Tracker = vehicle.Tracker
            };
            return veh;
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                SqlConnection db = new SqlConnection(_connectionString);

                string sqlQuery = "UPDATE Vehicle set VehileName = @VehicleName, PlateNo=@PlateNo,  RentPerHr = vehicle.RentPerHr, RentPerDay = @RentPerDay,RentPerWeek = @RentPerWeek, LateFee = @LateFee, Availability = @Availability, VehicleType = @VehicleType, VehicleKind = @VehicleKind, WhereStored = @WhereStored,Tracker = @Tracker where Id = @Id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VehicleName", vehicle.VehileName);
                parameters.Add("@PlateNo", vehicle.PlateNo);
                parameters.Add("RentPerHr", vehicle.RentPerHr);
                parameters.Add("@RentPerDay" , vehicle.RentPerDay);
                parameters.Add("@RentPerWeek" , vehicle.RentPerWeek);
                parameters.Add("@LateFee" , vehicle.LateFee);
                parameters.Add("@Availability" , vehicle.Availability);
                parameters.Add("@VehicleType" , vehicle.VehicleType);
                parameters.Add("@VehicleKind" , vehicle.VehicleKind);
                parameters.Add("@WhereStored" , vehicle.WhereStored);
                parameters.Add("@Tracker" , vehicle.Tracker);
                parameters.Add("@Id", vehicle.Id);

                int rowsAffected = db.Execute(sqlQuery, parameters);



            }
            catch (Exception ex)
            {

            }
        }

        public void DeleteUser(int VehicleId)
        {
            SqlConnection db = new SqlConnection(_connectionString);
            string sqlQuery = "Delete From Vehicle WHERE Id = " + VehicleId;

            int rowsAffected = db.Execute(sqlQuery);
        }
    }
}