using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppLearn.Models;

namespace WpfAppLearn
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql("server=localhost;username=root;password=root;port=3306;database=wpfusers");
        }
    }
}
