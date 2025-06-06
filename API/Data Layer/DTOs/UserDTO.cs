namespace API.Data_Layer.DTOs
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Token { get; set; }

        //Adding photo url for the user's main photo to be displayed in the nav bar 
        public string? PhotoUrl { get; set; }
    }
}
