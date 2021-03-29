using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoafAndStranger.DataAccess
{
    public class StrangersRepository
    {
        AppDBContext _db;

        public StrangersRepository(AppDBContext db)
        {
            _db = db;
        }

        public IEnumerable<Stranger> GetAll()
        {
            var strangers = _db.Strangers
                .Include(s => s.Loaf)
                .Include(s =>s.Top);

            return strangers;
        }
    }
}
