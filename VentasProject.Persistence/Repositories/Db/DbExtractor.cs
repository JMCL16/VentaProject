using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VentasProject.Persistence.Repositories.Db
{
    public class DbExtractor : DbContext
    {
        private readonly string _connString;

        public DbExtractor(string connString)
        {
            _connString = connString;
        }


    }
}
