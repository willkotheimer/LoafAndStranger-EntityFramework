using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;

namespace LoafAndStranger.DataAccess
{
    public class LoafRepository
    {
        const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";

        public List<Loaf> GetAll()
        {
            var loaves = new List<Loaf>();

            //create a connection
            using var connection = new SqlConnection(ConnectionString);

            //open the connection
            connection.Open();

            //create a command
            var command = connection.CreateCommand();

            //telling the command what you want to do
            var sql = @"Select * 
                        From Loaves";
            command.CommandText = sql;

            //send the command to sql
            //execute the command
            var reader = command.ExecuteReader();

            //loop over our results
            while (reader.Read()) //reader.Read pulls one row at a time from the db
            {
                //add it to the list
                loaves.Add(MapLoaf(reader));
            }

            return loaves;
        }

        public void Add(Loaf loaf)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Loaves] ([Size],[Type],[WeightInOunces],[Price],[Sliced])
                                OUTPUT inserted.Id
                                VALUES(@Size, @type, @weightInOunces, @Price, @Sliced)";

            cmd.Parameters.AddWithValue("Size", loaf.Size);
            cmd.Parameters.AddWithValue("type", loaf.Type);
            cmd.Parameters.AddWithValue("weightInOunces", loaf.WeightInOunces);
            cmd.Parameters.AddWithValue("Price", loaf.Price);
            cmd.Parameters.AddWithValue("Sliced", loaf.Sliced);

            var id = (int)cmd.ExecuteScalar();

            loaf.Id = id;

        }

        public Loaf Get(int id)
        {
            var sql = @"Select *
                        From Loaves
                        where Id = @id";

            //create a connection
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            //create a command
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("id", id);

            //execute the command
            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var loaf = MapLoaf(reader);
                return loaf;
            }

            return null;
        }

        public void Remove(int id)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"Delete 
                                from Loaves 
                                where Id = @id";

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();
        }

        Loaf MapLoaf(SqlDataReader reader)
        {
            //get each column value from the reader
            var id = (int)reader["Id"]; //explicit cast (throws exceptions)
            var size = (LoafSize)reader["Size"];
            var type = reader["Type"] as string; //implicit cast (returns null)
            var price = (decimal)reader["price"];
            var weightInOunces = (int)reader["weightInOunces"];
            var sliced = (bool)reader["sliced"];
            var createdDate = (DateTime)reader["createdDate"];

            //make a loaf
            var loaf = new Loaf
            {
                Id = id,
                Price = price,
                Size = size,
                Sliced = sliced,
                Type = type,
                WeightInOunces = weightInOunces
            };

            return loaf;
        }

    }
}
