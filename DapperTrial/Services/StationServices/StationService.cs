using Dapper;
using DapperTrial.Models.DataModel;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.StationServices
{
    public class StationService : IStationService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }
        public void AddStation(Station station)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var userData = new Station()
            {



                StationName = station.StationName,

                City = station.City,
                Province = station.Province,
                CanRent = station.CanRent,
                CanReturn = station.CanReturn,
                NumberOfVehicles = station.NumberOfVehicles
            };
            string sqlQuery = "Insert Into Station (StationName, City, Province, CanRent, CanReturn, NumberOfVehicles ) Values(@StationName, @City, @Province, @CanRent, @CanReturn, @NumberOfVehicles  )";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@StationName", userData.StationName);
            parameters.Add("@City", userData.City);
            parameters.Add("@Province", userData.Province);
            parameters.Add("@CanRent", userData.CanRent);
            parameters.Add("@CanReturn", userData.CanReturn);
            parameters.Add("@NumberOfVehicles", userData.NumberOfVehicles);

            var rowsAffected = connection.Execute(sqlQuery, parameters);
            connection.Close();
        }

        public void DeleteStation(int Id)
        {
            SqlConnection db = new SqlConnection(_connectionString);
            string sqlQuery = "Delete From Station WHERE Id = " + Id;

            int rowsAffected = db.Execute(sqlQuery);
        }

        public Station DetailsStation(int Id)
        {
            Station stations = new Station();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                stations = connection.Query<Station>("Select * From Station WHERE Id =" + Id, new { Id }).SingleOrDefault();//only takes single value or default value
            }
            Station veh = new Station()
            {



                StationName = stations.StationName,

                City = stations.City,
                Province = stations.Province,
                CanRent = stations.CanRent,
                CanReturn = stations.CanReturn,
                NumberOfVehicles = stations.NumberOfVehicles

            };
            return veh;
        }

        public Station GetStationById(int Id)
        {
            Station stations = new Station();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                stations = connection.Query<Station>("Select * From Station WHERE Id =" + Id, new { Id }).SingleOrDefault();//only takes single value or default value
            }
            Station veh = new Station()
            {
                StationName = stations.StationName,

                City = stations.City,
                Province = stations.Province,
                CanRent = stations.CanRent,
                CanReturn = stations.CanReturn,
                NumberOfVehicles = stations.NumberOfVehicles
            };
            return veh;
        }

        public IEnumerable<Station> GetStations()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            IList<Station> List = new List<Station>();
            var data = connection.Query<Station>("select * from Station");

            foreach (var stations in data)
            {
                List.Add(new Station()
                {
                    Id = stations.Id,
                    StationName = stations.StationName,

                    City = stations.City,
                    Province = stations.Province,
                    CanRent = stations.CanRent,
                    CanReturn = stations.CanReturn,
                    NumberOfVehicles = stations.NumberOfVehicles
                });
            }
            return List;

        }

        public void UpdateStation(Station station)
        {
            try
            {
                SqlConnection db = new SqlConnection(_connectionString);

                string sqlQuery = "UPDATE Station set StationName = @StationName, City= @City, Province= @Province, CanRent=@CanRent, CanReturn=@CanReturn, NumberOfVehicles=@NumberOfVehicles where Id = @Id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StationName", station.StationName);
                parameters.Add("@City", station.City);
                parameters.Add("@Province", station.Province);
                parameters.Add("@CanRent", station.CanRent);
                parameters.Add("@CanReturn", station.CanReturn);
                parameters.Add("@NumberOfVehicles", station.NumberOfVehicles);
                parameters.Add("@Id", station.Id);

                int rowsAffected = db.Execute(sqlQuery, parameters);



            }
            catch (Exception ex)
            {

            }
        }
    }
}
