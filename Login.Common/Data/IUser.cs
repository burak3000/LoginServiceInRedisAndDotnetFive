using System;

namespace Login.Common
{
    public interface IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid UserGuid { get; set; }
    }
}
