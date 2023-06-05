namespace Flights.Models
{
    public class UpdateEmployeeViewModel
    {
        public Guid id { get; set; }
        public string flightid { get; set; }
        public string? departure_destination { get; set; }
        public string? arrival_destination { get; set; }
        public DateTime? departure_date { get; set; }
        public DateTime? arrival_date { get; set; }
    }
}
