using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;

namespace BandTracker.Tests
{

  [TestClass]
  public class BandTests : IDisposable
  {
    public BandTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=bandtracker_test;";
    }
    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }

    [TestMethod]
    public void Equals_OverrideTrueForSameDescription_Band()
    {
      //Arrange, Act
      Band firstBand = new Band("Mow the lawn", "1");
      Band secondBand = new Band("Mow the lawn", "1");

      //Assert
      Assert.AreEqual(firstBand, secondBand);
    }

    [TestMethod]
    public void Save_SavesBandToDatabase_BandList()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "1");
      testBand.Save();

      //Act
      List<Band> result = Band.GetAll();
      List<Band> testList = new List<Band>{testBand};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToObject_Id()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "1");
      testBand.Save();

      //Act
      Band savedBand = Band.GetAll()[0];

      int result = savedBand.GetId();
      int testId = testBand.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsBandInDatabase_Band()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "1");
      testBand.Save();

      //Act
      Band foundBand = Band.Find(testBand.GetId());

      //Assert
      Assert.AreEqual(testBand, foundBand);
    }

    [TestMethod]
    public void AddVenue_AddsVenueToBand_VenueList()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "2");
      testBand.Save();

      Venue testVenue = new Venue("Home stuff");
      testVenue.Save();

      //Act
      testBand.AddVenue(testVenue);

      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue>{testVenue};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetVenues_ReturnsAllBandVenues_VenueList()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "1");
      testBand.Save();

      Venue testVenue1 = new Venue("Home stuff");
      testVenue1.Save();

      Venue testVenue2 = new Venue("Work stuff");
      testVenue2.Save();

      //Act
      testBand.AddVenue(testVenue1);
      List<Venue> result = testBand.GetVenues();
      List<Venue> testList = new List<Venue> {testVenue1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Delete_DeletesBandAssociationsFromDatabase_BandList()
    {
      //Arrange
      Venue testVenue = new Venue("Home stuff");
      testVenue.Save();

      string testDescription = "Mow the lawn";
      Band testBand = new Band(testDescription, "1");
      testBand.Save();

      //Act
      testBand.AddVenue(testVenue);
      testBand.DeleteBands();

      List<Band> resultVenueBands = testVenue.GetBands();
      List<Band> testVenueBands = new List<Band> {};

      //Assert
      CollectionAssert.AreEqual(testVenueBands, resultVenueBands);
    }
  }
}
