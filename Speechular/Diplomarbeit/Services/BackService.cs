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


            foreach (var item in _context.Users)
            {
                _context.Users.Remove(item);
            };
            foreach (var item in _context.LanguageCodes)
            {
                _context.LanguageCodes.Remove(item);
            };

            _context.Users.Add(new User
            {
                Email = "DaurerM190019@sus.htl-grieskirchen.at",
                Username = "Daurer",
                FirstName = "Michael",
                LastName = "Daurer",
                Password = "123",
                PreferredLanguage = "en-US",
                UserToken = "4a987abfde63ba9676662e26cc29da64",
            });

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
