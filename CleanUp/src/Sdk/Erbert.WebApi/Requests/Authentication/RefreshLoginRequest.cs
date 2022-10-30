using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.WebApi.Sdk.Requests.Authentication
{
    public class RefreshLoginRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
