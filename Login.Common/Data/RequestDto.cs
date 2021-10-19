using System;

namespace Login.Common
{
    public class RequestDto
    {
        public Guid RequestGuid { get; set; }
        public IUser UserInfo { get; set; }
        public RequestState RequestState { get; set; }
        public RequestType RequestType { get; set; }
        public string AdditionalMessage { get; set; }

    }
}