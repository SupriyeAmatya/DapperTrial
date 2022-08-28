using DapperTrial.Models.DataModel;
using System.ComponentModel.DataAnnotations;

namespace DapperTrial.Models.DTO
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string PlateNo { get; set; }
        public string InitialRentPrice { get; set; }
        public string RentRatePerHr { get; set; }
        public string Penalty { get; set; }
        public bool Availability { get; set; }
        public string VehicleType { get; set; }
        public string VehicleKind { get; set; }
        public string WhereStored { get; set; }
        public string Tracker { get; set; }
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please choose at least one image.")]
        public IFormFileCollection VehicleImages { get; set; }
    }
}
