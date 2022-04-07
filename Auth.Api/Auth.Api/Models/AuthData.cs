using System;

namespace Auth.Api.Models
{
    public class AuthData
    {
        public string JWT { get; set; }

        public string UserRole { get; set; }

        public string UserFullName { get; set; }

        public string MemberId { get; set; }

        public DateTime Expiration { get; set; }
    }
}
