using BholaCattleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client; 

namespace BholaCattleApp.Services
{
    public class AppDbContext : DbContext
    {
        //public DbSet<Animal> Animal { get; set; }
        // Add other DbSets...

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseOracle("User Id=youruser;Password=yourpass;Data Source=//localhost/XE;");
        }
    }

    public class DataService
    {
        private readonly AppDbContext _context = new AppDbContext();

        //public async Task<List<Animal>> GetAnimalsAsync() => await _context.Animals.ToListAsync();
        //public async Task AddAnimalAsync(Animal animal) { _context.Animals.Add(animal); await _context.SaveChangesAsync(); }
        // Similar for other entities...
    }
}
