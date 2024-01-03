using Diplomarbeit.Dtos;
using Diplomarbeit.Services;
using MadTechLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diplomarbeit.Controller
{
    [ApiController]
    public class TwillioController : ControllerBase
    {
        private readonly MyTwillioService _service;

        public TwillioController(MyTwillioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("MadTech/CreateRoom")]
        public string createRoom(UserDto user,string RoomName)
        {
             return _service.CreateRoom(user, RoomName);
        }
        [HttpPost]
        [Route("MadTech/JoinRoom")]
        public string joinRoom(string RoomName)
        {
            return _service.JoinRoom(RoomName);
        }

        [HttpGet]
        [Route("MadTech/GetRooms")]
        public List<TwilioRooms> GetRooms()
        {
            return _service.GetRooms();
        }

        [HttpGet]
        [Route("MadTech/GetUsers")]
        public List<string> GetUsers()
        {
            return _service.GetUsers();
        }

        [HttpGet]
        [Route("MadTech/GetLanguages")]
        public List<LanguageDto> GetLanguages()
        {
            return _service.Languages();
        }

        [HttpPost]
        [Route("MadTech/GetUserLogin")]
        public UserDto GetUserLogin(Login login)
        {
            return _service.GetUserLogin(login);
        }

        [HttpPost]
        [Route("MadTech/AddUser")]
        public void AddUser(Register register)
        {
            _service.AddUser(register);
        }

        [HttpPost]
        [Route("MadTech/LogOut")]
        public void LogOutUser(UserDto user)
        {
            _service.LogOut(user);
        }
    }
}
