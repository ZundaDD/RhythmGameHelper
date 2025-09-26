using CommunityToolkit.Mvvm.ComponentModel;
using RhythmGameHelper.Client.Services;
using RhythmGameHelper.Shared.DataTransfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RhythmGameHelper.Client.ViewModels
{
    public partial class SongViewModel : ObservableObject
    {
        private readonly IFavoriteService _favoriteService;
        public SongDto Song { get; }
        public int Id => Song.Id;
        public string SongName => Song.SongName;
        public string? OfficialArtist => Song.OfficialArtist;
        public float OfficialBPM => Song.OfficialBPM;

        [ObservableProperty] private bool _isFavorite;

        public ICommand ToggleFavoriteCommand { get; }

        public SongViewModel(SongDto dto, IFavoriteService favoritesService)
        {
            Song = dto;
            _favoriteService = favoritesService;

            _isFavorite = _favoriteService.IsFavorite(Song.Id);
            ToggleFavoriteCommand = new RelayCommand(async _ => await ToggleFavorite());
        }

        private async Task ToggleFavorite()
        {
            if (IsFavorite)
            {
                await _favoriteService.RemoveAsync(Id);
                IsFavorite = false;
            }
            else
            {
                await _favoriteService.AddAsync(Id);
                IsFavorite = true;
            }
        }
    }
}
