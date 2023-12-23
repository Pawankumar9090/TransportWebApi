using System.ComponentModel.DataAnnotations;

namespace TransportWebApi.Model
{
    public class CheckPoints
    {
        [Key]
        public int checkpointid { get; set; }
        public string? checkpointcoordinates { get; set; }
    }
}
