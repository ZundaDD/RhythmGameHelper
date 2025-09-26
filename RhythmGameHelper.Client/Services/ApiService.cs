using Microsoft.Extensions.Configuration;
using RhythmGameHelper.Shared.DataQuery;
using RhythmGameHelper.Shared.DataStructure;
using RhythmGameHelper.Shared.DataTransfer;
using RhythmGameHelper.Client.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Client.Services
{
    public interface IApiService
    {
        Task<List<Game>> SearchAllGames();
        Task<List<string>> SearchGameCategories(int gameId);
        Task<List<string>> SearchGameVersions(int gameId);
        Task<PackedSong> SearchSongAsync(SongQuery query);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"] ?? string.Empty;
        }

        public async Task<List<Game>> SearchAllGames()
        {
            var url = new UrlBuilder(_apiBaseUrl);
            url.AddSubUrl("api").AddSubUrl("game");

            
            var results = await _httpClient.GetFromJsonAsync<List<Game>>(url.Url);
            return results ?? new();
        }

        public async Task<List<string>> SearchGameCategories(int gameId)
        {
            var url = new UrlBuilder(_apiBaseUrl);
            url.AddSubUrl("api").AddSubUrl("game").AddSubUrl(gameId.ToString()).AddSubUrl("categories");

            var results = await _httpClient.GetFromJsonAsync<List<string>>(url.Url);
            return results ?? new();
        }

        public async Task<List<string>> SearchGameVersions(int gameId)
        {
            var url = new UrlBuilder(_apiBaseUrl);
            url.AddSubUrl("api").AddSubUrl("game").AddSubUrl(gameId.ToString()).AddSubUrl("versions");

            var results = await _httpClient.GetFromJsonAsync<List<string>>(url.Url);
            return results ?? new();
        }

        public async Task<PackedSong> SearchSongAsync(SongQuery query)
        {
            var url = new UrlBuilder(_apiBaseUrl);
            url.AddSubUrl("api").AddSubUrl("song").AddSubUrl("search");
            
            var response = await _httpClient.PostAsJsonAsync(url.Url, query);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PackedSong>() ?? new();
        }
    }
}
