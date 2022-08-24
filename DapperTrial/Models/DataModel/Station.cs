namespace DapperTrial.Models.DataModel
{
    public class Station
    {

        public int Id { get; set; }
        public string StationName { get; set; }

        public string City { get; set; }
        public string Province { get; set; }
        public bool CanRent { get; set; }
        public bool CanReturn { get; set; }
        public int NumberOfVehicles { get; set; }
    }
}
