using Diplomarbeit.Dtos;
using MadTechLib;
using Microsoft.Identity.Client;
using NuGet.Common;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Security.Principal;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using Twilio.TwiML.Voice;

namespace Diplomarbeit.Services
{
    public class MyTwillioService
    {

        private MadTechContext _context;

        public MyTwillioService(MadTechContext context)
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", "4a987abfde63ba9676662e26cc29da64");
            _context = context;
        }

        public string CreateRoom(UserDto user,string RoomName)
        {
            TwilioClient.Init(user.TwilSid, user.AuthTkn);

            var room = RoomResource.Create(uniqueName: RoomName, emptyRoomTimeout: 60,type:RoomResource.RoomTypeEnum.Go);

            var grant = new VideoGrant();
            grant.Room = room.UniqueName;

            var grants = new HashSet<IGrant> { grant };

            return new Twilio.Jwt.AccessToken.Token("ACcc68c5f3aed6ca9e4509cff2536c5977",
                         "SK04a4a93362e4b1551c96b56a33db7ef0",
                         "Dl9bhxqLz9XM7ArWLlA0Ci9w7EAks0Hj",
                         user.UserName,
                         grants: grants).ToJwt();
        }

        public string JoinRoom(string roomName)
        {
            var room = RoomResource.Fetch(roomName);

            var grant = new VideoGrant();
            grant.Room = room.UniqueName;

            var grants = new HashSet<IGrant> { grant };

            return new Twilio.Jwt.AccessToken.Token("ACcc68c5f3aed6ca9e4509cff2536c5977",
                         "SK04a4a93362e4b1551c96b56a33db7ef0",
                         "Dl9bhxqLz9XM7ArWLlA0Ci9w7EAks0Hj",
                         "Invited_User",
                         grants: grants).ToJwt();
        }

        public List<TwilioRooms> GetRooms()
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", "4a987abfde63ba9676662e26cc29da64");

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

            
            _context.Users.Add(new User
            {
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
                TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", "4a987abfde63ba9676662e26cc29da64");

                    UserDto user = _context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).Select(x => new UserDto
                    {
                        AuthTkn = "4a987abfde63ba9676662e26cc29da64",
                        UserName = login.UserName,
                        TwilSid = "ACcc68c5f3aed6ca9e4509cff2536c5977"
                    }).FirstOrDefault();

                    _context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).FirstOrDefault().UserToken = "ACcc68c5f3aed6ca9e4509cff2536c5977";
                    _context.SaveChanges();

                    return user;
                
            }
            return null;
        }

        public void LogOut(UserDto user)
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", "4a987abfde63ba9676662e26cc29da64");

            var account = AccountResource.Update(
            status: AccountResource.StatusEnum.Closed,
            pathSid:user.TwilSid
                );

            _context.Users.Where(x => x.Username == user.UserName).FirstOrDefault().UserToken = "";
            _context.SaveChanges();
        }
    }
}
