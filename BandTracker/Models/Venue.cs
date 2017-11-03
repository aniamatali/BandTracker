using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace BandTracker.Models
{
  public class Venue
  {
    private string _name;
    private int _id;
    public Venue(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public override bool Equals(System.Object otherVenue)
    {
      if (!(otherVenue is Venue))
      {
        return false;
      }
      else
      {
        Venue newVenue = (Venue) otherVenue;
        return this.GetId().Equals(newVenue.GetId());
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO venues (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
    public static List<Venue> GetAll()
    {
      List<Venue> allVenue = new List<Venue> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int VenueId = rdr.GetInt32(0);
        string VenueName = rdr.GetString(1);
        Venue newVenue = new Venue(VenueName, VenueId);
        allVenue.Add(newVenue);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allVenue;
    }
    public static Venue Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM venues WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int VenueId = 0;
      string VenueName = "";

      while(rdr.Read())
      {
        VenueId = rdr.GetInt32(0);
        VenueName = rdr.GetString(1);
      }
      Venue newVenue = new Venue(VenueName, VenueId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newVenue;
    }

    public List<Band> GetBands()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT bands.* FROM venues JOIN venues_bands ON (venues.id = venues_bands.venue_id) JOIN bands ON (venues_bands.band_id = bands.id) WHERE venues.id = @VenueId;";


      MySqlParameter VenueId = new MySqlParameter();
      VenueId.ParameterName = "@VenueId";
      VenueId.Value = _id;
      cmd.Parameters.Add(VenueId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Band> bands = new List<Band>{};

      while(rdr.Read())
      {
        int bandId = rdr.GetInt32(0);
        string bandDescription = rdr.GetString(1);
        string bandDate = rdr.GetString(2);
        Band newBand = new Band(bandDescription, bandDate, bandId);
        bands.Add(newBand);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return bands;
    }


    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM venues;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteVenue()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM venues WHERE id = @VenueId; DELETE FROM venues_bands WHERE venue_id = @VenueId;", conn);
      MySqlParameter venuesIdParameter = new MySqlParameter();
      venuesIdParameter.ParameterName = "@VenueId";
      venuesIdParameter.Value = this.GetId();

      cmd.Parameters.Add(venuesIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public void AddBand(Band newBand)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO venues_bands (venue_id, band_id) VALUES (@VenueId, @BandId);";

            MySqlParameter venue_id = new MySqlParameter();
            venue_id.ParameterName = "@VenueId";
            venue_id.Value = _id;
            cmd.Parameters.Add(venue_id);

            MySqlParameter band_id = new MySqlParameter();
            band_id.ParameterName = "@BandId";
            band_id.Value = newBand.GetId();
            cmd.Parameters.Add(band_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void UpdateVenue(int id)
      {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"UPDATE FROM venues WHERE venue_id = @id;";

          MySqlParameter VenueId = new MySqlParameter();
          VenueId.ParameterName = "@id";
          VenueId.Value = id;
          cmd.Parameters.Add(VenueId);

          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
      }

      public void UpdateVenueName(string newVenueName)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE venues SET name = @newVenueName WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter venueName = new MySqlParameter();
        venueName.ParameterName = "@newVenueName";
        venueName.Value = newVenueName;
        cmd.Parameters.Add(venueName);

        cmd.ExecuteNonQuery();
        _name = newVenueName;

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
      }


  }

}
