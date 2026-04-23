using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RPGManager.Data
{
    public static class DbContextFactory
    {
        private const string ConnectionString = "Server=localhost;Database=DndDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static DndDBContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DndDBContext>();
            optionsBuilder.UseSqlServer(ConnectionString);
            return new DndDBContext(optionsBuilder.Options);
        }
    }
}

