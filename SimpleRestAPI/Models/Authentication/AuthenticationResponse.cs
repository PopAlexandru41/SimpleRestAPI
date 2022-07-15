namespace SimpleRestAPI.Models.Authentication
{
    public class AuthenticationResponse
    {
        public Guid IdUser { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }

        public AuthenticationResponse(User user,string token)
        {
            IdUser = user.Id;
            Name = user.Name;
            Token = token;
        }
    }
}
