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

      [HttpGet("/Venues/{id}")]
      public ActionResult VenueList(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue selectedVenue = Venue.Find(id);
        List<Band> venueBands = selectedVenue.GetBands();
        model.Add("venue", selectedVenue);
        model.Add("bands", venueBands);
        return View(model);
      }

      [HttpGet("/Venues/{id}/Bands/new")]
      public ActionResult AddBand(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue selectedVenue = Venue.Find(id);
        List<Band> allBands = selectedVenue.GetBands();
        model.Add("venue", selectedVenue);
        model.Add("bands", allBands);
        return View(model);
      }

      [HttpPost("/Venues/{id}")]
      public ActionResult BandDetailsPost(int id)
      {
        string BandDescription = Request.Form["inputBand"];
        Band newBand = new Band(BandDescription,(Request.Form["inputDate"]));
        newBand.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        Venue selectedVenue = Venue.Find(Int32.Parse(Request.Form["venue-id"]));
        selectedVenue.AddBand(newBand);
        List<Band> venueBands = selectedVenue.GetBands();
        model.Add("bands", venueBands);
        model.Add("venue", selectedVenue);
        return View("VenueList", model);
      }

      [HttpGet("/Bands/{id}")]
      public ActionResult BandVenues(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Band selectedBand = Band.Find(id);
        List<Venue> allVenues = selectedBand.GetVenues();
        model.Add("band", selectedBand);
        model.Add("venue", allVenues);
        return View(model);
      }

      [HttpPost("/Bands/{id}")]
      public ActionResult BandVenues2(int id)
      {

        Venue newVenue = new Venue(Request.Form["inputVenue"]);
        newVenue.Save();

        Dictionary<string, object> model = new Dictionary<string, object>();
        Band selectedBand = Band.Find(Int32.Parse(Request.Form["Band-id"]));
        selectedBand.AddVenue(newVenue);
        List<Venue> allVenues = selectedBand.GetVenues();
        model.Add("band", selectedBand);
        model.Add("venue", allVenues);
        return View("BandVenues", model);
      }

      [HttpGet("/Band/{id}/venues/new")]
      public ActionResult venuesForm(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Band selectedBand = Band.Find(id);
        List<Venue> allVenues = selectedBand.GetVenues();
        model.Add("band", selectedBand);
        model.Add("venue", allVenues);
        return View(model);
      }

    }
  }
