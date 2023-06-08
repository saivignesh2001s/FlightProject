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
using Flights.Repository;
using Microsoft.IdentityModel.Tokens;

namespace Flights.Controllers
{
    public class FlightController : Controller  //Controller for flights
    {
        private readonly IMethods context1;
        private readonly ICsvMethods context;
        public FlightController(ICsvMethods context,IMethods context1) //Importing Two methods from Repository folder
        {
            this.context = context;
            this.context1=context1;
        }


        public IActionResult Upload() {   //Function for uploading csv file

            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
            {
            if (file.FileName!= null)
            {

                string k = file.FileName;



                if (context.IsCsv(k))
                {


                    string fname = Path.GetFileName(file.FileName);

                    bool k1 = context.writecsvtosql(fname);
                    if (k1)
                    {

                        return RedirectToAction("Flightlist");
                    }
                    else
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
            else
            {
                ViewBag.Message3 = "Select file";
                return View();
            }

            }
        public async Task<IActionResult> Flightlist() //Function for listing flight details
        {
            var Details = context1.GetAllmethod();
            return View(Details);
        }

        public IActionResult AddFlight()   //Function for adding flights
        {

            return View();
        }
        [HttpPost]

        public async Task<IActionResult> AddFlight(AddFlightModel c)
        {
            try { 
            var fdata = new FlightData()
            {
                id =new Guid(),
                flightid = c.flightid,
                arrival_date = Convert.ToDateTime(c.arrival_date),
                arrival_destination = c.arrival_destination,
                departure_date = Convert.ToDateTime(c.departure_date),
                departure_destination = c.departure_destination

            };

            if (fdata.departure_date <= fdata.arrival_date)
                {
                    bool k1 = context1.Addmethod(fdata);
                    if (k1)
                    {


                        return RedirectToAction("Flightlist");
                    }
                    else
                    {
                        ViewBag.Message2 = "Check data and sql given";
                        return View(c);
                    }
                }
                else
                {
                    ViewBag.Message3 = "Departure datetime cannot be greater than arrival date time";

                    return View(c);
                }
            }
            catch
            {
                ViewBag.Message1 = "Check the formats of the data  given";
                return View(c);
            }

            }
            


        
        public async Task<IActionResult> Viewflight(Guid id) //context for Editing flight details
        {
            var p = context1.Getmethod(id);
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
            if (p.departure_date < p.arrival_date)
            {
                bool k1 = context1.Updatemethod(p);
                if (k1)
                {
                    return RedirectToAction("Flightlist");
                  
                }
                else
                {
                    ViewBag.Message5 = "Check Datas again";
                    return View(p);
                }
            }
            else
            {
                ViewBag.Message4 = "Departure datetime must be less than the Arrival datetime";
                return View(p);
            }



        }
        [HttpPost]
        public async Task<IActionResult> Deleteflight(Guid id) //Deleting flight detail
        {
            bool k = context1.Deletemethod(id);
            if (k)
            {
                return RedirectToAction("Flightlist");
            }
            else
            {
                var p = context1.Getmethod(id);
                var newmodel = new UpdateEmployeeViewModel()
                {
                    id = p.id,
                    flightid = p.flightid,
                    arrival_date = Convert.ToDateTime(p.arrival_date),
                    arrival_destination = p.arrival_destination,
                    departure_date = Convert.ToDateTime(p.departure_date),
                    departure_destination = p.departure_destination

                };
                ViewBag.Message6 = "Can't Delete this";
                return View(newmodel);
            }

        }

     
        public FileResult Export1() //Exporting csv 
        {

            string csv = context.extractdata();
            
            byte[] bytes = Encoding.ASCII.GetBytes(csv);
            return File(bytes, "text/csv", "Flights.csv");



        }
    }

}