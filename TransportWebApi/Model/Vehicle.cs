using System.ComponentModel.DataAnnotations;

namespace TransportWebApi.Model
{
    public class Vehicle
    {
        [Key] public int VehicleID { get; set; }
        public int Capacity { get; set; }
        public string? LicenseLevel { get; set; }
    }
}
