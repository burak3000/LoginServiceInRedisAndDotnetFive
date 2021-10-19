using System;

namespace Login.Common
{
    public class UserBase:IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid UserGuid { get; set; }
    }
}