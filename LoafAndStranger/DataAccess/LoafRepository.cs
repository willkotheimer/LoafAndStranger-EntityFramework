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
    public class LoafRepository
    {
        // const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";

        AppDBContext _db;
        public LoafRepository(AppDBContext db)
        {
            _db = db;
        }
        public List<Loaf> GetAll() => _db.Loaves.ToList();
       

        public void Add(Loaf loaf)
        {
            _db.Loaves.Add(loaf);
            _db.SaveChanges();
        }

        public Loaf Get(int id)
        {
            return _db.Loaves.Find(id);
        }

        public void Remove(int id)
        {
            _db.Loaves.Remove(Get(id));
            
        }

        public void Update(Loaf loaf)
        {
            /*var existingLoaf = Get(loaf.Id);
            existingLoaf.Sliced = loaf.Sliced;

            _db.SaveChanges();*/

            // this tells ef to take the load from the outside world and treat it like something it knew about
            _db.Loaves.Attach(loaf).State = EntityState.Modified;
            _db.SaveChanges();

        }

        public void Slice(int id)
        {
            var loaf = Get(id);
            loaf.Sliced = true;
            _db.SaveChanges();
        }
    }
}
