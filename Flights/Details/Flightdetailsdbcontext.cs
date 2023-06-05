using Flights.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flights.Details
{
    public class Flightdetailsdbcontext : DbContext
    {
        public Flightdetailsdbcontext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<FlightData> FlightDatas{ get; set; }
    }
}
