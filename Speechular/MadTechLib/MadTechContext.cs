using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadTechLib
{
    public class MadTechContext : DbContext
    {
        public MadTechContext(DbContextOptions<MadTechContext>options):base(options)
        {
        }
        public MadTechContext()
        {
        }

        public DbSet<User>Users { get; set; }
        public DbSet<LanguageCode> LanguageCodes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine($"Db OnConfiguring: IsConfigured={optionsBuilder.IsConfigured}");
            if(!optionsBuilder.IsConfigured)
            {
                string connectionString = @"server=(LocalDB)\mssqllocaldb;attachdbfilename=C:\Users\daure.LAPTOP-T1J55CM5\Documents\GitHub\VideoCallMadtech\Speechular\MadTechLib\MadTech.mdf;database=MadTech;integrated security=True;MultipleActiveResultSets=True;";
                Console.WriteLine($"Using connectionsString {connectionString}");
                optionsBuilder.UseSqlServer( connectionString );
            }
        }
    }
}
