using CarInsurance.Models;
using CarInsurance.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsurance.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetQuote(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear, string carMake, string carModel, bool hasDUI, int speedingTickets, bool wantsCoverage)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (CarInsuranceEntities db = new CarInsuranceEntities())
                {
                    var quote = new Quote();
                    quote.FirstName = firstName;
                    quote.LastName = lastName;
                    quote.EmailAddress = emailAddress;
                    quote.DateOfBirth = dateOfBirth;
                    quote.CarYear = carYear;
                    quote.CarMake = carMake;
                    quote.CarModel = carModel;
                    quote.HasDUI = hasDUI;
                    quote.SpeedingTickets = speedingTickets;
                    quote.WantsCoverage = wantsCoverage;


                    int insuranceQuote = 50;
                    if (Convert.ToInt32(DateTime.Now.Year - (dateOfBirth.Year)) < 18)
                    {
                        insuranceQuote += 100;
                    }
                    else if (Convert.ToInt32(DateTime.Now.Year - (dateOfBirth.Year)) < 25)
                    {
                        insuranceQuote += 25;
                    }
                    else if (Convert.ToInt32(DateTime.Now.Year - (dateOfBirth.Year)) > 100)
                    {
                        insuranceQuote += 25;
                    }

                    if (carYear < 2000)
                    {
                        insuranceQuote += 25;
                    }
                    else if (carYear > 2015)
                    {
                        insuranceQuote += 25;
                    }

                    if (carMake.ToLower() == "porsche")
                    {
                        insuranceQuote += 25;
                    }

                    if (carMake.ToLower() == "porsche" && carModel.ToLower() == "911 carrera")
                    {
                        insuranceQuote += 25;
                    }

                    if (speedingTickets > 0)
                    {
                        insuranceQuote += speedingTickets * 10;
                    }

                    if (hasDUI)
                    {
                        insuranceQuote += Convert.ToInt32(insuranceQuote * .25);
                    }

                    if (wantsCoverage)
                    {
                        insuranceQuote += Convert.ToInt32(insuranceQuote * .50);
                    }

                    quote.InsuranceQuote = insuranceQuote;
                    db.Quotes.Add(quote);
                    db.SaveChanges();

                    
                }

                return View("Success");

            }
        }

        public ActionResult Admin()
        {
            using (CarInsuranceEntities db = new CarInsuranceEntities())
            {
                var quotes = db.Quotes;
                var quoteVms = new List<QuoteVm>();
                foreach (var quote in quotes)
                {
                    var quoteVm = new QuoteVm();
                    quoteVm.FirstName = quote.FirstName;
                    quoteVm.LastName = quote.LastName;
                    quoteVm.EmailAddress = quote.EmailAddress;
                    quoteVm.InsuranceQuote = Convert.ToInt32(quote.InsuranceQuote);
                    quoteVms.Add(quoteVm);
                }
                return View(quoteVms);
                
            }
        }

    }
}