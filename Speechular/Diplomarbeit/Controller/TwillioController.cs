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
        private readonly TranslationService _translationService;

        public TwillioController(MyTwillioService service, TranslationService translationService)
        {
            _service = service;
            _translationService = translationService;
        }

        [HttpPost]
        [Route("MadTech/CreateRoom")]
        public CreateRoomDto createRoom(UserDto user,string mailTo)
        {
             return _service.CreateRoom(user,mailTo);
        }
        [HttpPost]
        [Route("MadTech/JoinRoom")]
        public string joinRoom(string RoomName,string username)
        {
            return _service.JoinRoom(RoomName, username);
        }

        [HttpPost]
        [Route("MadTech/JoinRoomToken")]
        public string joinRoomToken(string RoomName, string username,string AccesToken)
        {
            return _service.JoinRoomAccesToken(RoomName, username,AccesToken);
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
        public bool AddUser(Register register)
        {
            return _service.AddUser(register);
        }

        [HttpPost]
        [Route("MadTech/LogOut")]
        public void LogOutUser(UserDto user)
        {
            _service.LogOut(user);
        }

        [HttpPost]
        [Route("MadTech/DeleteRoom")]
        public void DeleteRoom(string RoomName)
        {
            _service.DeleteRoom(RoomName);
        }

        [HttpGet]
        [Route("MadTech/Translate")]
        public string TranslateText(string text, string A, string B)
        {
            return _translationService.TranslateText(text, A, B);
        }
    }
}
