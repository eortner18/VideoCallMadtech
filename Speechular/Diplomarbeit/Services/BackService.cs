using System.Formats.Asn1;
using System;
using MadTechLib;

namespace Diplomarbeit.Services
{
    public class BackService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private MadTechContext _context;

        public BackService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MadTechContext>();

            //_context.Database.EnsureDeleted();
            //_context.Database.EnsureCreated();

            FillDb();

            return Task.Run(() => { },stoppingToken);
        }

        public void FillDb()
        {


            _context.Users.RemoveRange();
            _context.LanguageCodes.RemoveRange();

            _context.LanguageCodes.Add(new LanguageCode
            {
                CountryCode = "en-US",
                CountryName = "United States of Amerika"
            });

            _context.LanguageCodes.Add(new LanguageCode
            {
                CountryCode = "de",
                CountryName = "Deutschland"
            });

            _context.SaveChanges();
        }
    }
}
