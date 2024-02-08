using Diplomarbeit.Dtos;
using MadTechLib;
using Microsoft.Identity.Client;
using NuGet.Common;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;
using Twilio.TwiML.Voice;
using static System.Net.WebRequestMethods;

namespace Diplomarbeit.Services
{
    public class MyTwillioService
    {

        private MadTechContext _context;
        private string TwilTok = "a5222c00ee889f1865ea401a3d18939e";

        public MyTwillioService(MadTechContext context)
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", TwilTok);
            _context = context;
        }

        public CreateRoomDto CreateRoom(UserDto user,string sendMailTo)
        {

            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", TwilTok);
            try
            {
                var roomExist = RoomResource.Fetch(user.UserName + " Room");


                if (roomExist != null)
                {
                    string myjwt = JoinRoom(user.UserName + " Room", user.UserName);
                    return new CreateRoomDto { JwtToken = myjwt, AccessToken = GetHashString(myjwt) + GetHashString(user.UserName), RoomName = user.UserName + " Room" };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }




            var room = RoomResource.Create(uniqueName: user.UserName+" Room", emptyRoomTimeout: 2,type:RoomResource.RoomTypeEnum.Go);

            var grant = new VideoGrant();
            grant.Room = room.UniqueName;

            var grants = new HashSet<IGrant> { grant };

            string jwt = new Twilio.Jwt.AccessToken.Token("ACcc68c5f3aed6ca9e4509cff2536c5977",
                         "SK04a4a93362e4b1551c96b56a33db7ef0",
                         "Dl9bhxqLz9XM7ArWLlA0Ci9w7EAks0Hj",
                         user.UserName,
                         grants: grants).ToJwt();

            _context.RoomDetail.Add(new RoomDetails
            {
                Name = user.UserName + " Room",
                AccessToken = GetHashString(jwt) + GetHashString(user.UserName)
            });

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("madtechpdfemail@gmail.com");
                mail.To.Add(user.mail);
                mail.Subject = "PDF von Madtech";
                mail.Body = "Link: " + "http://localhost:4200/video-room/" + user.UserName + " Room" + "/" + GetHashString(jwt) + GetHashString(user.UserName);

                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("madtechpdfemail@gmail.com", "baxmhgcasdxbcgbd");
                    smtp.EnableSsl = true;

                    smtp.Send(mail);

                }

                if(sendMailTo != null || sendMailTo != "")
                {
                    mail.To.Clear();
                    mail.To.Add(sendMailTo);
                    mail.Body = "Link for Video Room: http://localhost:4200/video-room/" + user.UserName + " Room" + "/" + GetHashString(jwt) + GetHashString(user.UserName);
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("madtechpdfemail@gmail.com", "baxmhgcasdxbcgbd");
                        smtp.EnableSsl = true;

                        smtp.Send(mail);

                    }
                }
                
            }

            return new CreateRoomDto { JwtToken = jwt, AccessToken = GetHashString(jwt) + GetHashString(user.UserName),RoomName = user.UserName + " Room" };
        }

        public string JoinRoomAccesToken(string roomName, string username, string AccessToken)
        {
            string myRoomName = _context.RoomDetail.Where(x => x.AccessToken == AccessToken).Select(x => x.Name).FirstOrDefault();
            if (myRoomName == roomName)
            {
                var room = RoomResource.Fetch(roomName);

                var grant = new VideoGrant();
                grant.Room = room.UniqueName;

                var grants = new HashSet<IGrant> { grant };

                return new Twilio.Jwt.AccessToken.Token("ACcc68c5f3aed6ca9e4509cff2536c5977",
                             "SK04a4a93362e4b1551c96b56a33db7ef0",
                             "Dl9bhxqLz9XM7ArWLlA0Ci9w7EAks0Hj",
                             username,
                             grants: grants).ToJwt();
            }

            return null;
            
        }

        public string JoinRoom(string roomName,string username)
        {
            var room = RoomResource.Fetch(roomName);

            var grant = new VideoGrant();
            grant.Room = room.UniqueName;

            var grants = new HashSet<IGrant> { grant };

            return new Twilio.Jwt.AccessToken.Token("ACcc68c5f3aed6ca9e4509cff2536c5977",
                         "SK04a4a93362e4b1551c96b56a33db7ef0",
                         "Dl9bhxqLz9XM7ArWLlA0Ci9w7EAks0Hj",
                         username,
                         grants: grants).ToJwt();
        }

        public List<TwilioRooms> GetRooms()
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", TwilTok);

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

        public bool AddUser(Register register)
        {
            if(_context.Users.Where(x=>x.Username == register.UserName).ToList().Count > 0)
            {
                return false;
            }
            else
            {
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

                return true;
            }
            
            
        }

        public UserDto GetUserLogin(Login login)
        {
            Console.WriteLine("login");
            if(_context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).Count() > 0)
            {
                TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", TwilTok);

                    UserDto user = _context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).Select(x => new UserDto
                    {
                        AuthTkn = "d3e145fe329b5e1630fc243ebeb557a0",
                        UserName = login.UserName,
                        TwilSid = "ACcc68c5f3aed6ca9e4509cff2536c5977",
                        mail = x.Email
                    }).FirstOrDefault();

                    _context.Users.Where(x => x.Username == login.UserName || x.Email == login.UserName).Where(x => x.Password == login.Password).FirstOrDefault().UserToken = "ACcc68c5f3aed6ca9e4509cff2536c5977";
                    _context.SaveChanges();

                    return user;
                
            }
            return null;
        }

        public void LogOut(UserDto user)
        {
            TwilioClient.Init("ACcc68c5f3aed6ca9e4509cff2536c5977", TwilTok);

            var account = AccountResource.Update(
            status: AccountResource.StatusEnum.Closed,
            pathSid:user.TwilSid
                );

            _context.Users.Where(x => x.Username == user.UserName).FirstOrDefault().UserToken = "";
            _context.SaveChanges();
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
