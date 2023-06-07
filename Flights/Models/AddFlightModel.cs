using System.ComponentModel.DataAnnotations;

namespace Flights.Models
{
    public class AddFlightModel
    {
        [Required]
        public string flightid { get; set; }
        public string departure_destination { get; set; }
        public string arrival_destination { get; set; }
        public DateTime departure_date { get; set; }
        public DateTime arrival_date { get; set; }
    }
}
