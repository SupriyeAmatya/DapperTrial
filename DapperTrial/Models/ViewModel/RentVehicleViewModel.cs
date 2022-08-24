namespace DapperTrial.Models.ViewModel
{
    public class RentVehicleViewModel
    {
       
        public string Email { get; set; }
        public string VehicleName { get; set; }
        //public string PlateNo { get; set; }
        public string LicenseNo { get; set; }

        //public string Status { get; set; }
        //public int RentedFromLocation { get; set; }

        //public string RentedToLocation { get; set; }
        public string RentType { get; set; }
        public DateTime RentedFromTime { get; set; }
        public DateTime RentedToTime { get; set; }
        //public bool ReturnApproved { get; set; }

        //public string Bill { get; set; }
        //public string Fine { get; set; }
    }
}
