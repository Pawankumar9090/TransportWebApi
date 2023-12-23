using System.ComponentModel.DataAnnotations;

namespace TransportWebApi.Model
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }
        public string? Name { get; set; }
        public string? LicenseLevel { get; set; }
        public int status { get; set; }
    }
}
