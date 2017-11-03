using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Band
  {
    private string _description;
    private int _id;
    private string _performanceDate;

    public Band(string description, string performanceDate, int id = 0)
    {
      _description = description;
      _id = id;
      _performanceDate = performanceDate;
    }

    public override bool Equals(System.Object otherBand)
    {
      if (!(otherBand is Band))
      {
        return false;
      }
      else
      {
        Band newBand = (Band) otherBand;
        bool idEquality = this.GetId() == newBand.GetId();
        bool descriptionEquality = this.GetDescription() == newBand.GetDescription();
        bool performancedateEquality = this.GetPerformanceDate() == newBand.GetPerformanceDate();
        return (idEquality && descriptionEquality && performancedateEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }

    public string GetDescription()
    {
      return _description;
    }
    public int GetId()
    {
      return _id;
    }

    public string GetPerformanceDate()
    {
      return _performanceDate;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO bands (description, performancedate) VALUES (@description, @performanceDate);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter performancedate = new MySqlParameter();
      performancedate.ParameterName = "@performanceDate";
      performancedate.Value = this._performanceDate;
      cmd.Parameters.Add(performancedate);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Band> GetAll()
    {
      List<Band> allBands = new List<Band> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands ORDER BY performancedate;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandDescription = rdr.GetString(1);
        string bandPerformanceDate = rdr.GetString(2);
        Band newBand = new Band(bandDescription, bandPerformanceDate, bandId);
        allBands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBands;
    }
    public static Band Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM bands WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bandId = 0;
      string bandName = "";
      string bandPerformanceDate = "";

      while(rdr.Read())
      {
        bandId = rdr.GetInt32(0);
        bandName = rdr.GetString(1);
        bandPerformanceDate = rdr.GetString(2);
      }
      Band newBand = new Band(bandName, bandPerformanceDate, bandId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newBand;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM bands;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteBands()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("DELETE FROM bands WHERE id = @BandId; DELETE FROM venues_bands WHERE band_id = @BandId;", conn);
        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@BandId";
        bandIdParameter.Value = this.GetId();

        cmd.Parameters.Add(bandIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }

      public void AddVenue(Venue newVenue)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

        MySqlParameter venue_id = new MySqlParameter();
        venue_id.ParameterName = "@VenueId";
        venue_id.Value = newVenue.GetId();
        cmd.Parameters.Add(venue_id);

        MySqlParameter band_id = new MySqlParameter();
        band_id.ParameterName = "@BandId";
        band_id.Value = _id;
        cmd.Parameters.Add(band_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn !=null)
        {
          conn.Dispose();
        }
      }

      public List<Venue> GetVenues()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT venue_id FROM venues_Bands WHERE band_id = @bandId;";

        MySqlParameter bandIdParameter = new MySqlParameter();
        bandIdParameter.ParameterName = "@bandId";
        bandIdParameter.Value = _id;
        cmd.Parameters.Add(bandIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> venueIds =new List<int> {};
        while(rdr.Read())
        {
          int venueId = rdr.GetInt32(0);
          venueIds.Add(venueId);
        }
        rdr.Dispose();

        List<Venue> venues = new List<Venue> {};
        foreach (int venueId in venueIds)
        {
          var venueQuery = conn.CreateCommand() as MySqlCommand;
          venueQuery.CommandText = @"SELECT * FROM venues WHERE id = @VenueId;";

          MySqlParameter venueIdParameter = new MySqlParameter();
          venueIdParameter.ParameterName = "@VenueId";
          venueIdParameter.Value = venueId;
          venueQuery.Parameters.Add(venueIdParameter);

          var venueQueryRdr = venueQuery.ExecuteReader() as MySqlDataReader;
          while(venueQueryRdr.Read())
          {
            int thisVenueId = venueQueryRdr.GetInt32(0);
            string venueName = venueQueryRdr.GetString(1);
            Venue foundVenue = new Venue(venueName, thisVenueId);
            venues.Add(foundVenue);
          }
          venueQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return venues;
      }

    }
  }
