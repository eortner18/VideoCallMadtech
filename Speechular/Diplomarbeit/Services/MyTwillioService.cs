using Diplomarbeit.Dtos;
using MadTechLib;
using Microsoft.Identity.Client;
using NuGet.Common;
using System.Collections.Generic;
using System.Security.Principal;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;

namespace Diplomarbeit.Services
{
    public class MyTwillioService
    {

        private MadTechContext _context;

        public MyTwillioService(MadTechContext context)
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", "f235a3caa04aadf48c9b5c53ce6d56b2");
            _context = context;
        }

        public void CreateRoom(UserDto user,string RoomName)
        {
            TwilioClient.Init(user.TwilSid, user.AuthTkn);

            var room = RoomResource.Create(uniqueName: RoomName, emptyRoomTimeout: 60);


        }

        public List<TwilioRooms> GetRooms()
        {
            TwilioClient.Init("ACb384c79bf65f369fde65c47460944e8d", "5e42bb265e61d3786dfcb5f28c974333");

            //var room = RoomResource.Create(uniqueName: "MyRoom", emptyRoomTimeout: 60);
            Console.WriteLine(RoomResource.Read().Count());
            return RoomResource.Read().Select(x=>new TwilioRooms
            {
                ParticipantsMax = x.MaxParticipants.Value,
                Name = x.UniqueName,
                Participants = ParticipantResource.Read(status: ParticipantResource.StatusEnum.Connected,pathRoomSid: x.Sid).Count(),
            }).ToList();
        }

        public List<string> GetUsers()
        {
            return _context.Users.Select(x=>x.Username).ToList();
        }

        public List<LanguageDto>Languages()
        {
            return _context.LanguageCodes.Select(x => new LanguageDto
            {
                CountryName = x.CountryName,
                Id = x.Id,
            }).Distinct().ToList();
        }

        public void AddUser(Register register)
        {
            //Abfragen ob mail existiert
            //To-Do

            
            var account = AccountResource.Create(friendlyName: register.UserName);

            _context.Users.Add(new User
            {
                UserToken = account.AuthToken,
                Email = register.Mail,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Password = register.Password,
                PreferredLanguage = _context.LanguageCodes.Where(x => x.CountryName == register.CountryName).Select(x => x.CountryCode).FirstOrDefault(),
                Username = register.UserName,
            });
            _context.SaveChanges();
        }

        public UserDto GetUserLogin(Login login)
        {
            Console.WriteLine("login");
            if(_context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).Count() > 0)
            {
                var account = AccountResource.Read(friendlyName: login.UserName);

                UserDto user = _context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).Select(x => new UserDto
                {
                    AuthTkn = account.FirstOrDefault().AuthToken.ToString(),
                    UserName = login.UserName,
                    TwilSid = account.FirstOrDefault().Sid.ToString()
                }).FirstOrDefault();

                return user;
            }
            return null;
        }
    }
}
