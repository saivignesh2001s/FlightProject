using Flights.Details;
using Flights.Models;
using Flights.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Grpc.Core;
using CsvHelper;
using System.Globalization;

namespace Flights.Controllers
{
    public class FlightController : Controller
    {
        private readonly Flightdetailsdbcontext context;
        public FlightController(Flightdetailsdbcontext dbcontext)
        {
            this.context = dbcontext;
        }


        public IActionResult Upload() {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormCollection c){
            string k =c["filename"].ToString();
             string[] p = k.Split('.');
             bool k1 = false;
             foreach(var p1 in p)
             {
                 if (p1 == "csv")
                 {
                     k1 = true;
                     break;
                 }
             }
            /*if (k1)
             {
                 String hd = "D:\\Downloads\\" + k;
                 FileStream fs = new FileStream(hd,FileMode.Open,FileAccess.Read);
                 StreamReader rs=new StreamReader(fs);
                 try
                 {
                     while (!rs.EndOfStream)
                     {
                         string line = rs.ReadLine();
                         string[] pk = line.Split(',');
                         var flightdetail = new FlightData()
                         {
                             id=Guid.NewGuid(),
                             flightid = pk[0],
                             departure_destination = pk[1].ToString(),
                             departure_date =Convert.ToDateTime(pk[3]),
                             arrival_destination = pk[2].ToString(),
                             arrival_date = Convert.ToDateTime(pk[4])
                         };

                         await context.FlightDatas.AddAsync(flightdetail);
                         await context.SaveChangesAsync();



                     }

                     return RedirectToAction("Flightlist");
                 }
                 catch
                 {
                     ViewBag.Message10 = "Check datas and connections all again";
                     return View();

                 }
                 finally
                 {
                     rs.Close();
                     rs.Dispose();
                     fs.Close();
                     fs.DisposeAsync();

             }*/

            if (k1)
            {
                try
                {
                  /*  if (!Directory.Exists("~//Uploads"))
                    {
                        Directory.CreateDirectory("//Uploads");
                    }*/
                    string fname = @"D://Downloads//" + k;

                    /*using (FileStream fileStream = System.IO.File.Create(fname))
                    {
                        k.CopyTo(fileStream);
                        fileStream.Flush();
                    }*/

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

                                await context.FlightDatas.AddAsync(flightdetail);
                                await context.SaveChangesAsync();
                            }
                        }

                    }
                    return RedirectToAction("Flightlist");
                }
                catch
                {
                    ViewBag.Message1 = "Check Data,Sql connection and file name again";
                    return View();
                }

            }
            else
            {
                ViewBag.Message2 = "Select csv files only";
                return View();
            }
          
           
        
        
        
        }
        public async Task<IActionResult> Flightlist()
        {
            var Details = await context.FlightDatas.ToListAsync();
            return View(Details);
        }

        public IActionResult AddFlight()
        {

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AddFlight(IFormCollection c)
        {

            var fdata = new FlightData()
            {
                id = Guid.NewGuid(),
                flightid = c["flightid"].ToString(),
                departure_destination = c["departure_destination"].ToString(),
                arrival_destination = c["arrival_destination"].ToString(),
                arrival_date = Convert.ToDateTime(c["arrival_date"]),
                departure_date = Convert.ToDateTime(c["departure_date"])
            };

            try
            {
                await context.FlightDatas.AddAsync(fdata);
                await context.SaveChangesAsync();
                ViewBag.Message1 = "Saved successfully";
                return RedirectToAction("Flightlist");
            }
            catch
            {
                ViewBag.Message2 = "Check connections again";
                return View(fdata);
            }




        }
        public async Task<IActionResult> Viewflight(Guid id)
        {
            var p = await context.FlightDatas.FirstOrDefaultAsync(x => x.id == id);
            if (p != null)
            {
                var newmodel = new UpdateEmployeeViewModel()
                {
                    id = p.id,
                    flightid = p.flightid,
                    arrival_date = Convert.ToDateTime(p.arrival_date),
                    arrival_destination = p.arrival_destination,
                    departure_date = Convert.ToDateTime(p.departure_date),
                    departure_destination = p.departure_destination

                };
                return await Task.Run(() => (View("Viewflight", newmodel)));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Viewflight(UpdateEmployeeViewModel p)
        {

            FlightData k = await context.FlightDatas.FindAsync(p.id);
            if (k != null)
            {
                k.flightid = p.flightid;
                k.arrival_date = Convert.ToDateTime(p.arrival_date);
                k.arrival_destination = p.arrival_destination;
                k.departure_destination = p.departure_destination;
                k.departure_date = p.departure_date;
                if (k.departure_date > k.arrival_date)
                {
                    return View(p);
                    ViewBag.Message4 = "Departure datetime must be less than the Arrival datetime";
                }
                else
                {
                    try
                    {

                        await context.SaveChangesAsync();
                        return RedirectToAction("Flightlist");
                    }
                    catch
                    {
                        ViewBag.Message5 = "Check Datas again";
                        return View(k);
                    }
                }
            }
            else
            {
                ViewBag.Message6 = "Check again";
                return View(k);
            }



        }
        [HttpPost]
        public async Task<IActionResult> Deleteflight(Guid id)
        {
            var p = await context.FlightDatas.FirstOrDefaultAsync(x => x.id == id);
            if (p != null)
            {
                context.FlightDatas.Remove(p);
                await context.SaveChangesAsync();
                return RedirectToAction("Flightlist");
            }
            return View(p);

        }

     
        public FileResult Export1()
        {

            var p = context.FlightDatas.ToList();
            string[] columns = new string[] { "Flight id", "Departure_Destination", "Arrival_Destination", "Departure_Date", "Arrival_Date" };
            string csv = string.Empty;

            foreach (var ps in columns)
            {
                csv += ps + ',';


            }
            csv += "\r\n";

            foreach (var pd in p)
            {
                csv += pd.flightid.Replace(',', ';') + ',';
                csv += pd.departure_destination.Replace(',', ';') + ',';
                csv += pd.arrival_destination.Replace(',', ';') + ',';
                csv += Convert.ToString(pd.departure_date).Replace(',', ';') + ',';
                csv += pd.arrival_date.ToString().Replace(',', ';') + ",";
                csv += "\r\n";

            }
            byte[] bytes = Encoding.ASCII.GetBytes(csv);
            return File(bytes, "text/csv", "Flights.csv");



        }
    }

}