namespace Auth.Api.Models
{
    public class Response
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public AuthData AuthData { get; set;}
    }
}
