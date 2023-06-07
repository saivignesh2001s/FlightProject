using Flights.Details;
using Flights.Models;
using Flights.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace Flights.Repository
{
    public interface IMethods //CRUD operations to Database
    {
        bool Addmethod(FlightData p);
        bool Updatemethod(UpdateEmployeeViewModel p);
        bool Deletemethod(Guid id);
        List<FlightData> GetAllmethod();
        FlightData Getmethod(Guid id);

    }
    public class crudmethods : IMethods
    {
        private readonly Flightdetailsdbcontext context;
        public crudmethods(Flightdetailsdbcontext _context)
        {
            this.context=_context;
        }
        public bool Addmethod(FlightData p)  //Add method
        {
            try
            {
                context.FlightDatas.Add(p);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Deletemethod(Guid id) //Delete method
        {
            var p=context.FlightDatas.FirstOrDefault(x => x.id == id);
            if (p != null)
            {
                context.FlightDatas.Remove(p);
                context.SaveChanges();
                return true;
              
            }
            return false;
        }

        public List<FlightData> GetAllmethod() //Get all data from sql
        {
            return context.FlightDatas.ToList();
        }

        public FlightData Getmethod(Guid id) //Get data for the id
        {
            var m=context.FlightDatas.FirstOrDefault(x => x.id == id);
            if (m!=null)
            {
                FlightData k = new FlightData();
                k.id = m.id;
                k.flightid = m.flightid;
                k.departure_destination = m.departure_destination;
                k.arrival_destination = m.arrival_destination;
                k.arrival_date = m.arrival_date;
                k.departure_date = m.departure_date;
                return k;
            }
            else
            {
                return null;
            }
        }

        public bool Updatemethod(UpdateEmployeeViewModel p) //method for editing
        {
            var k= context.FlightDatas.FirstOrDefault(x => x.id == p.id);
            if (k!=null)
            {
                k.id = p.id;
                k.flightid = p.flightid;
                k.departure_destination = p.departure_destination;
                k.arrival_destination = p.arrival_destination;
                k.arrival_date = p.arrival_date;
                k.departure_date = p.departure_date;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        
      
    }
}
