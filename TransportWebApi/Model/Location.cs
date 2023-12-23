using System;
using System.ComponentModel.DataAnnotations;
namespace TransportWebApi.Model
{
    public class Location
    {
        [Key] public int locationid { get; set; }
        public int locationtype { get; set; }
        public string? coordinates { get; set; }
        public int? capacity { get; set; }
        public int? availamount { get; set; }
    }
}

