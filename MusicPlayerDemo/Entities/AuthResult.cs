using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Entities
{
    class AuthResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ExpireTime { get; set; }
        public int Status { get; set; }
    }

    
}
