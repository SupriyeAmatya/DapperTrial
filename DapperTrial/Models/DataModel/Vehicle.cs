namespace DapperTrial.Models.DataModel
{
    public class Vehicle
    {

        public int Id { get; set; }
        public string VehileName { get; set; }
        public string PlateNo { get; set; }
        public string RentPerHr { get; set; }
        public string RentPerDay { get; set; }
        public string RentPerWeek { get; set; }
        public string LateFee { get; set; }
        public bool Availability { get; set; }
        public string VehicleType { get; set; }
        public string VehicleKind { get; set; }
        public string WhereStored { get; set; }
        public string Tracker { get; set; }
    }
}
