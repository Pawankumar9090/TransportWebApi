﻿using Microsoft.EntityFrameworkCore;
using TransportWebApi.Model;

namespace TransportWebApi.Data
{
    public class TransPortDbContext:DbContext
    {
        public TransPortDbContext(DbContextOptions options) : base(options){   }
        public DbSet<Driver>Driver { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<CheckPoints> CheckPoints { get; set; }



    }
}
