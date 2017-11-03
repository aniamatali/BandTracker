using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BandTracker.Models;
using BandTracker;

namespace BandTracker.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [HttpGet("/Venues")]
      public ActionResult ViewAllVenues()
      {
        return View("AddVenue",Venue.GetAll());
      }

      [HttpPost("/Venues")]
      public ActionResult AddVenue()
      {
        Venue newVenue = new Venue (Request.Form["inputVenue"]);
        newVenue.Save();
        return View (Venue.GetAll());
      }

      [HttpPost("/Venues/Delete")]
      public ActionResult DeletePage()
      {
        Venue.DeleteAll();
        return View();
      }
      

    }
  }
