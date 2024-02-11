namespace Diplomarbeit.Dtos
{
    public class CreateRoomDto
    {
        public string JwtToken { get; set; }
        public string AccessToken { get; set; }

        public string RoomName { get; set; }
    }
}
