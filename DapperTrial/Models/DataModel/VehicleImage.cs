using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DapperTrial.Models.DataModel
{
    public partial class VehicleImage
    {

        [Key]
        [Column("VehicleID")]
        public int Id { get; set; }
        public int Vehiclenumber { get; set; }
        [Column("Vehicle Image")]


        public string VehicleImage1 { get; set; }

        //[ForeignKey("Vehiclenumber")]
        [InverseProperty("Vehicleimages")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
