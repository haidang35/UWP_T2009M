using MusicPlayerDemo.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Services
{
    class MusicService
    {
        private  string ApiOrgApp = "https://music-i-like.herokuapp.com";
        private  string ApiMusicPath = "/api/v1/songs";
        private  string ApiDataType = "application/json";

        public async Task<bool> Save(Song song)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiOrgApp);
                    var dataConvert = JsonConvert.SerializeObject(song);
                    var songData = new StringContent(dataConvert, Encoding.UTF8, ApiDataType);
                    var response = await client.PostAsync(ApiMusicPath, songData);
                    var result = await response.Content.ReadAsStringAsync();
                    return true;
                }
            }catch(Exception)
            {
                return false;
            }
        }

        public async Task<List<Song>> FindAll()
        {
            List<Song> songList = new List<Song>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiOrgApp);
                var response = await client.GetAsync(ApiMusicPath);
                var result = await response.Content.ReadAsStringAsync();
                songList = JsonConvert.DeserializeObject<List<Song>>(result);
            }
            return songList;
        }

        public async Task<Song> FindOne()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiOrgApp);
                var response = await client.GetAsync(ApiMusicPath);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Song>(result);
            }
        }

        public async Task<bool> Update(int id, Song song)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiOrgApp);
                    var dataConvert = JsonConvert.SerializeObject(song);
                    var songData = new StringContent(dataConvert, Encoding.UTF8 , ApiDataType);
                    var songUpdatedData = await client.PutAsync($"{ApiMusicPath}/{id}", songData);
                    return true;
                }
            }catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiOrgApp);
                var response = await client.DeleteAsync($"{ApiMusicPath}/{id}");
                var result = await response.Content.ReadAsStringAsync();
                return true;
            }
        }


    }
}
