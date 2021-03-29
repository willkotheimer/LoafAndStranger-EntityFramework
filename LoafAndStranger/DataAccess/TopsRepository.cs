using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace LoafAndStranger.DataAccess
{
    public class TopsRepository
    {
        // const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";

        AppDBContext _db;
        public TopsRepository(AppDBContext db)
        {
            _db = db;
        }
        public IEnumerable<Top> GetAll()
        {
            // return _db.Tops
            return _db.Tops
                .Include(t => t.Strangers)
                .ThenInclude(s=>s.Loaf)
                .Where(t=>t.Strangers.Any(s=>s.Loaf.Type=="Monkey Bread"))
                .AsNoTracking();
        }

        public IEnumerable<Top> GetAllOccupied()
        {

            return _db.Tops.Where(t => t.Occupied);
        }


        public Top Add(int numberOfSeats)
        {
            var top = new Top { NumberOfSeats = numberOfSeats };
            _db.Tops.Add(top);
            _db.SaveChanges();
          /*  using var db = new SqlConnection(ConnectionString);

            var sql = @"INSERT INTO [Tops] ([NumberOfSeats])
                        Output inserted.*
	                    VALUES (@numberOfSeats)";

            var top = db.QuerySingle<Top>(sql, new {numberOfSeats});*/

            return top;
        }
    }
}
