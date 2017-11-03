using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using BandTracker.Models;

namespace BandTracker.Tests
{
  [TestClass]
  public class VenueTests : IDisposable
  {
        public VenueTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=bandtracker_test;";
        }

       [TestMethod]
       public void GetAll_CategoriesEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Venue.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Venue()
      {
        //Arrange, Act
        Venue firstVenue = new Venue("Household chores");
        Venue secondVenue = new Venue("Household chores");

        //Assert
        Assert.AreEqual(firstVenue, secondVenue);
      }

      [TestMethod]
      public void Save_SavesVenueToDatabase_VenueList()
      {
        //Arrange
        Venue testVenue = new Venue("Household chores");
        testVenue.Save();

        //Act
        List<Venue> result = Venue.GetAll();
        List<Venue> testList = new List<Venue>{testVenue};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }


     [TestMethod]
     public void Save_DatabaseAssignsIdToVenue_Id()
     {
       //Arrange
       Venue testVenue = new Venue("Household chores");
       testVenue.Save();

       //Act
       Venue savedVenue = Venue.GetAll()[0];

       int result = savedVenue.GetId();
       int testId = testVenue.GetId();

       //Assert
       Assert.AreEqual(testId, result);
    }


    [TestMethod]
    public void Find_FindsVenueInDatabase_Venue()
    {
      //Arrange
      Venue testVenue = new Venue("Household chores");
      testVenue.Save();

      //Act
      Venue foundVenue = Venue.Find(testVenue.GetId());

      //Assert
      Assert.AreEqual(testVenue, foundVenue);
    }

    [TestMethod]
    public void Delete_DeletesVenueAssociationsFromDatabase_VenueList()
    {
      //Arrange
      Band testBand = new Band("Mow the lawn", "1");
      testBand.Save();

      string testName = "Home stuff";
      Venue testVenue = new Venue(testName);
      testVenue.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.DeleteVenue();

      List<Venue> resultBandCategories = testBand.GetVenues();
      List<Venue> testBandCategories = new List<Venue> {};

      //Assert
      CollectionAssert.AreEqual(testBandCategories, resultBandCategories);
    }

    [TestMethod]
    public void Test_AddBand_AddsBandToVenue()
    {
      //Arrange
      Venue testVenue = new Venue("Household chores");
      testVenue.Save();

      Band testBand = new Band("Mow the lawn", "1-1-1");
      testBand.Save();

      Band testBand2 = new Band("Water the garden", "1-2-3");
      testBand2.Save();

      //Act
      testVenue.AddBand(testBand);
      testVenue.AddBand(testBand2);

      List<Band> result = testVenue.GetBands();
      List<Band> testList = new List<Band>{testBand, testBand2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    public void Dispose()
    {
      Band.DeleteAll();
      Venue.DeleteAll();
    }
  }
}
