using DatingApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {
           
        }
        public  DbSet<AppUser>Users { get; set; } //proprietate de tipul DbSet->ia tipul clasei caruia vrei sa-i cream un DbSet, dupa dam call la tabelul  din baza de date AppUsers
    }
}
