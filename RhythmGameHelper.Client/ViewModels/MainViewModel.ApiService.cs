using RhythmGameHelper.Client.Services;
using RhythmGameHelper.Shared.DataQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhythmGameHelper.Client.ViewModels
{
    public partial class MainViewModel
    {
        private readonly IApiService _apiService;

        private async Task OnGameSelectionChanged()
        {
            Categories.Clear();
            Versions.Clear();
            SelectedCategory = null!;
            SelectedVersion = null!;

            //选中则重新获取分类
            if (SelectedGame != null)
            {
                var categories = await _apiService.SearchGameCategories(SelectedGame.Id);
                var versions = await _apiService.SearchGameVersions(SelectedGame.Id);

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
                foreach (var version in versions)
                {
                    Versions.Add(version);
                }
            }
        }

        private async Task PerformSearch()
        {
            IsNotLoading = false;

            try
            {
                SearchResults.Clear();

                var searchQuery = new SongQuery
                {
                    SongName = this.SearchText,
                    Artist = this.SearchArtist,
                    GameId = this.SelectedGame?.Id,
                    MinBPM = MinBPM, 
                    MaxBPM = MaxBPM,
                    CategoryName = this.SelectedCategory,
                    ShowNotOriginal = this.ShowNotOriginal,
                    ShowDeleted = this.ShowDeleted,
                    PageNumber = CurrentPage,
                    PageSize = _settingsService.UserSetting.PageSize,
                };

                var results = await _apiService.SearchSongAsync(searchQuery);

                foreach (var item in results.Songs)
                {
                    var songViewModel = new SongViewModel(item, _favoriteService);

                    SearchResults.Add(songViewModel);
                }

                if (results.TotalCount == 0)
                {
                    TotalPage = 1;
                }
                else TotalPage = results.TotalPages;
                TotalCount = results.TotalCount;

            }
            catch (Exception ex)
            {
                ErrorText = "Failed To Search Songs With Exception: " + ex.Message;
            }

            IsNotLoading = true;
        }
    }
}
