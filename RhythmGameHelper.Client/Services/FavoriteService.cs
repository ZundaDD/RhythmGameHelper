using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RhythmGameHelper.Client.Services
{
    public interface IFavoriteService
    {
        bool IsFavorite(int songId);
        Task AddAsync(int songId);
        Task RemoveAsync(int songId);
        Task LoadFavoritesAsync();
    }

    public class FavoriteService : IFavoriteService
    {
        private readonly string _filePath;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private HashSet<int> _favoriteCache = new();

        public FavoriteService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "RhythmGameHelper");
            Directory.CreateDirectory(appFolder);
            _filePath = Path.Combine(appFolder, "favorites.json");
        }

        public async Task LoadFavoritesAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    _favoriteCache = new HashSet<int>();
                    return;
                }

                var json = await File.ReadAllTextAsync(_filePath);
                var ids = JsonSerializer.Deserialize<List<int>>(json);
                _favoriteCache = new HashSet<int>(ids ?? new List<int>());
            }
            catch (Exception)
            {
                _favoriteCache = new HashSet<int>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public bool IsFavorite(int songId) => _favoriteCache.Contains(songId);

        public async Task AddAsync(int songId)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_favoriteCache.Add(songId)) await SaveAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task RemoveAsync(int songId)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_favoriteCache.Remove(songId))await SaveAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task SaveAsync()
        {
            var json = JsonSerializer.Serialize(_favoriteCache.ToList());
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}
