using CsvHelper;
using Flights.Models.Domain;
using Flights.Models;
using System.Globalization;
using Flights.Details;

namespace Flights.Repository
{
    public interface ICsvMethods  //interface for csv methods
    {
        bool IsCsv(string k);
        bool writecsvtosql(string k);

        string extractdata();
    }
    public class Csvmethods : ICsvMethods
    {
        private readonly Flightdetailsdbcontext context;
        public Csvmethods(Flightdetailsdbcontext context)
        {
            this.context = context;
        }
        public bool IsCsv(string k)   //checking for csv
        {
            string[] p = k.Split('.');
            bool k1 = false;
            foreach (var p1 in p)
            {
                if (p1 == "csv")
                {
                    k1 = true;
                    break;
                }
            }
            return k1;
        }

        public bool writecsvtosql(string fname) //writing to the sql
        {
            try
            {
                using (var reader = new StreamReader(fname))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Read();
                        csv.ReadHeader();
                        while (csv.Read())
                        {
                            var pk = csv.GetRecord<csvflight>();
                            var flightdetail = new FlightData()
                            {
                                id = Guid.NewGuid(),
                                flightid = pk.flightid.ToString(),
                                departure_destination = pk.departure_destination.ToString(),
                                departure_date = Convert.ToDateTime(pk.departure_date),
                                arrival_destination = pk.arrival_destination.ToString(),
                                arrival_date = Convert.ToDateTime(pk.arrival_date)
                            };

                            context.FlightDatas.Add(flightdetail);
                            context.SaveChanges();
                        }
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string extractdata() //extracting data from the sql
        {
            var p = context.FlightDatas.ToList();
            string[] columns = new string[] { "flightid","departure_destination","departure_date","arrival_destination","arrival_Date" };
            string csv = string.Empty;
            int i = 0;
            foreach (var ps in columns)
            {
                if (i < columns.Length-1)
                {

                    csv += ps + ',';
                    i++;
                }
                else
                    csv += ps+"\r\n";


            }
            

            foreach (var pd in p)
            {
                csv += pd.flightid.Replace(',', ';') + ',';
                csv += pd.departure_destination.Replace(',', ';') + ',';
                csv += Convert.ToString(pd.departure_date).Replace(',', ';') + ',';
                csv += pd.arrival_destination.Replace(',', ';') + ',';
                csv += pd.arrival_date.ToString().Replace(',', ';') + "\r\n";
           

            }
            return csv;

        }

        
    }
}
