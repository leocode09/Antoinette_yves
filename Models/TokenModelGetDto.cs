using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class TokenModelGetDto
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public int Expires_in { get; set; }
    }
}
