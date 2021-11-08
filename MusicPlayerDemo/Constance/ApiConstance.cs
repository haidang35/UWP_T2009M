using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Constance
{
    public class ApiConstance
    {
        public static string ApiOrgApp = "https://music-i-like.herokuapp.com";
        public static string ApiMusicPath = "/api/v1/songs";
        public static string ApiAuth = "/api/v1/accounts";
        public static string ApiAuthLogin = "/api/v1/accounts/authentication";
        public static string ApiDataType = "application/json";
        public static string ApiGetAccountList = "/admin/accounts";
        public static string ApiGetSongList = "/admin/songs";
        public static string ApiMySong = "/api/v1/songs/mine";
    }
}
