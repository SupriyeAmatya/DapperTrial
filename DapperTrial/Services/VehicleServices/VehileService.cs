using Dapper;
using DapperTrial.Models.DataModel;
using DapperTrial.Models.DTO;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.VehicleServices
{
    public class VehileService : IVehicleService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehileService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Vehicle> GetVehicles()
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            IList<Vehicle> List = new List<Vehicle>();
            string query = "Select v.Id, v.VehicleName, v.PlateNo, v.InitialPrice, v.RentRatePerHr, v.Penalty, v.Availability, v.VehicleType, v.VehicleKind, v.WhereStored, v.Tracker, i.Vehicleimage from Vehicle v Join VehicleImages i on v.PlateNo = i.Vehiclenumber  ";
            var data = connection.Query<Vehicle>(query).ToList();
            //var data = connection.Query<Vehicle>(
            //    query,
            //    (p, u) => { p = u.Vehicle; return p; }
            //    );


            foreach (var vehicle in data)
            {
                List.Add(new Vehicle()
                {
                    Id = vehicle.Id,
                    VehicleImage = vehicle.VehicleImage,
                    VehicleName = vehicle.VehicleName,
                    PlateNo = vehicle.PlateNo,
                    RentRatePerHr = vehicle.RentRatePerHr,
                    InitialRentPrice = vehicle.InitialRentPrice,
                    Penalty = vehicle.Penalty,
                    Availability = vehicle.Availability,
                    VehicleType = vehicle.VehicleType,
                    VehicleKind = vehicle.VehicleKind,
                    WhereStored = vehicle.WhereStored,
                    Tracker = vehicle.Tracker,
                });
            }
            return List;

        }
        public async Task AddVehicleAsync(VehicleDTO vehicle)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            using (connection.BeginTransaction())
            {
                var userData = new Vehicle()
                {
                    VehicleName = vehicle.VehicleName,
                    PlateNo = vehicle.PlateNo,
                    RentRatePerHr = vehicle.RentRatePerHr,
                    InitialRentPrice = vehicle.InitialRentPrice,
                    Penalty = vehicle.Penalty,
                    Availability = vehicle.Availability,
                    VehicleType = vehicle.VehicleType,
                    VehicleKind = vehicle.VehicleKind,
                    WhereStored = vehicle.WhereStored,
                    Tracker = vehicle.Tracker,

                };
                // Save the images to the folder in server.
                var filePaths = await SaveImages(vehicle.VehicleImages, userData);

                foreach (string path in filePaths)
                {
                    string sqlQuery2 = "Insert Into VehicleImages ( Vehiclenumber, Vehicleimage) Values( @Vehiclenumber, @Vehicleimage)";
                    DynamicParameters parameters2 = new DynamicParameters();
                    parameters2.Add("@Vehiclenumber", userData.PlateNo);
                    parameters2.Add("@Vehicleimage", path);
                    var rowsAffected2 = connection.Execute(sqlQuery2, parameters2);
                }

                string sqlQuery = "Insert Into Vehicle ( VehicleName, PlateNo, InitialPrice, RentRatePerHr, Penalty, Availability, VehicleType, VehicleKind, WhereStored, Tracker ) Values( @VehileName, @PlateNo, @InitialPrice, @RentRatePerHr, @Penalty, @Availability, @VehicleType, @VehicleKind, @WhereStored, @Tracker)";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VehileName", userData.VehicleName);
                parameters.Add("@PlateNo", userData.PlateNo);
                parameters.Add("@InitialPrice", userData.InitialRentPrice);
                parameters.Add("@RentRatePerHr", userData.RentRatePerHr);
                parameters.Add("@Penalty", userData.Penalty);
                parameters.Add("@Availability", userData.Availability);
                parameters.Add("@VehicleType", userData.VehicleType);
                parameters.Add("@VehicleKind", userData.VehicleKind);
                parameters.Add("@WhereStored", userData.WhereStored);
                parameters.Add("@Tracker", userData.Tracker);
                var rowsAffected = connection.Execute(sqlQuery, parameters);
            }

            
            connection.Close();
        }

        public Vehicle DetailsVehicle(int Id)
        {
            Vehicle vehicle = new Vehicle();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                vehicle = connection.Query<Vehicle>("Select v.VehicleName, v.PlateNo, v.InitialPrice, v.RentRatePerHr, v.Penalty, v.Availability, v.VehicleType, v.VehicleKind, v.WhereStored, v.Tracker, i.Vehicleimage from Vehicle v Join VehicleImages i on v.PlateNo = i.Vehiclenumber WHERE v.Id =" + Id, new { Id }).SingleOrDefault(); //only takes single value or default value
            }
            Vehicle veh = new Vehicle()
            {
                Id = Id,
                VehicleImage = vehicle.VehicleImage,
                VehicleName = vehicle.VehicleName,
                PlateNo = vehicle.PlateNo,
                RentRatePerHr = vehicle.RentRatePerHr,
                InitialRentPrice = vehicle.InitialRentPrice,
                Penalty = vehicle.Penalty,
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
                VehicleName = vehicle.VehicleName,
                PlateNo = vehicle.PlateNo,
                RentRatePerHr = vehicle.RentRatePerHr,
                InitialRentPrice = vehicle.InitialRentPrice,
                Penalty = vehicle.Penalty,
                Availability = vehicle.Availability,
                VehicleType = vehicle.VehicleType,
                VehicleKind = vehicle.VehicleKind,
                WhereStored = vehicle.WhereStored,
                Tracker = vehicle.Tracker
            };
            return veh;
        }

        //public void UpdateVehicle(Vehicle vehicle)
        //{
        //    try
        //    {
        //        SqlConnection db = new SqlConnection(_connectionString);

        //        string sqlQuery = "UPDATE Vehicle set VehileName = @VehicleName, PlateNo=@PlateNo,  RentPerHr = vehicle.RentPerHr, RentPerDay = @RentPerDay,RentPerWeek = @RentPerWeek, LateFee = @LateFee, Availability = @Availability, VehicleType = @VehicleType, VehicleKind = @VehicleKind, WhereStored = @WhereStored,Tracker = @Tracker where Id = @Id";
        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@VehicleName", vehicle.VehileName);
        //        parameters.Add("@PlateNo", vehicle.PlateNo);
        //        parameters.Add("RentPerHr", vehicle.RentPerHr);
        //        parameters.Add("@RentPerDay" , vehicle.RentPerDay);
        //        parameters.Add("@RentPerWeek" , vehicle.RentPerWeek);
        //        parameters.Add("@LateFee" , vehicle.LateFee);
        //        parameters.Add("@Availability" , vehicle.Availability);
        //        parameters.Add("@VehicleType" , vehicle.VehicleType);
        //        parameters.Add("@VehicleKind" , vehicle.VehicleKind);
        //        parameters.Add("@WhereStored" , vehicle.WhereStored);
        //        parameters.Add("@Tracker" , vehicle.Tracker);
        //        parameters.Add("@Id", vehicle.Id);

        //        int rowsAffected = db.Execute(sqlQuery, parameters);



        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void DeleteUser(int VehicleId)
        //{
        //    SqlConnection db = new SqlConnection(_connectionString);
        //    string sqlQuery = "Delete From Vehicle WHERE Id = " + VehicleId;

        //    int rowsAffected = db.Execute(sqlQuery);
        //}

        private async Task<List<string>> SaveImages(IFormFileCollection files, Vehicle vehicle)
        {

            string uploadFolder = Path.Combine("uploads", $"{vehicle.VehicleName}_{vehicle.PlateNo}");
            string contentPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadFolder);

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            List<string> filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);

                    // File paths that will be saved to the database.
                    filePaths.Add(Path.Combine(uploadFolder, InputFileName));

                    var ServerSavePath = Path.Combine(contentPath, InputFileName);
                    //Save file to uploads folder  
                    using (Stream fileStream = new FileStream(ServerSavePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return filePaths;
        }
    }
}