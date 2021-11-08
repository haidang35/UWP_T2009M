using MusicPlayerDemo.Constance;
using MusicPlayerDemo.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Services
{
    class AuthService
    {
        public static JObject AuthData;
        public async Task<bool> Register(Account account)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                    var dataJson = JsonConvert.SerializeObject(account);
                    var dataConvert = new StringContent(dataJson, Encoding.UTF8, ApiConstance.ApiDataType);
                    var response = await client.PostAsync(ApiConstance.ApiAuth, dataConvert);
                    var result = await response.Content.ReadAsStringAsync();
                    return true;
                }
            }catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Login(object loginData)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                    var dataJson = JsonConvert.SerializeObject(loginData);
                    var dataConvert = new StringContent(dataJson, Encoding.UTF8, ApiConstance.ApiDataType);
                    var request = await client.PostAsync(ApiConstance.ApiAuthLogin, dataConvert);
                    var result = await request.Content.ReadAsStringAsync();
                    AuthData = JObject.Parse(result);
                    return true;
                }
            }catch(Exception e)
            {
                return false;
            }
        }
        public async Task<Account> GetMyInfo(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var req = await client.GetAsync(ApiConstance.ApiAuth);
                var result = await req.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Account>(result);
            }
            return null;
        }

        public async Task<List<Account>> GetAccountList()
        {
            List<Account> accountList = new List<Account>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.GetAsync(ApiConstance.ApiGetAccountList);
                var result = await req.Content.ReadAsStringAsync();
                accountList = JsonConvert.DeserializeObject<List<Account>>(result);
            }
            return accountList;
        }

        public async Task<bool> CreateMySong(Song song)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var dataJson = JsonConvert.SerializeObject(song);
                var dataConvert = new StringContent(dataJson, Encoding.UTF8, ApiConstance.ApiDataType);
                var req = await client.PostAsync(ApiConstance.ApiMySong, dataConvert);
                return true;
            }
        }

        public async Task<List<Song>> GetMySong()
        {
            List<Song> mySongList = new List<Song>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.GetAsync(ApiConstance.ApiMySong);
                var res = await req.Content.ReadAsStringAsync();
                mySongList = JsonConvert.DeserializeObject<List<Song>>(res);
            }
            return mySongList;
        }

        public async Task<List<Song>> GetSongList()
        {
            List<Song> songList = new List<Song>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.GetAsync(ApiConstance.ApiGetSongList);
                var res = await req.Content.ReadAsStringAsync();
                songList = JsonConvert.DeserializeObject<List<Song>>(res);
            }
            return songList;
        }
        
        public async Task<bool> UpdateAccountInfo(int accountId, Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var dataJson = JsonConvert.SerializeObject(account);
                var dataConvert = new StringContent(dataJson, Encoding.UTF8, ApiConstance.ApiDataType);
                var req = await client.PutAsync($"{ApiConstance.ApiGetAccountList}/{accountId}", dataConvert);
                var res = await req.Content.ReadAsStringAsync();
                Debug.WriteLine(res);
                return true;
            }
        }

        public async Task<Account> GetAccountInfo(int accountId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.GetAsync($"{ApiConstance.ApiGetAccountList}/{accountId}");
                var res = await req.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Account>(res);
            }
        }

        public async Task<bool> DeleteAccount(int accountId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.DeleteAsync($"{ApiConstance.ApiGetAccountList}/{accountId}");
                return true;
            }
        }

        public async Task<bool> UpdateSongInfo(int songId, Song song)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var dataJson = JsonConvert.SerializeObject(song);
                var dataConvert = new StringContent(dataJson, Encoding.UTF8, ApiConstance.ApiDataType);
                var req = await client.PutAsync($"{ApiConstance.ApiGetSongList}/{songId}", dataConvert);
                return true;
            }
        }

        public async Task<bool> DeleteSong(int songId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiConstance.ApiOrgApp);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AuthData["access_token"]);
                var req = await client.DeleteAsync($"{ApiConstance.ApiGetSongList}/{songId}");
                return true;
            }
        }

    }
}
