using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentSomeWheels.Models;

    public class RentSomeWheelsContext : DbContext
    {
        public RentSomeWheelsContext (DbContextOptions<RentSomeWheelsContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<Client> Clients { get; set; } = default!;
        public DbSet<RentalContract> RentalContracts { get; set; } = default!;
    }
