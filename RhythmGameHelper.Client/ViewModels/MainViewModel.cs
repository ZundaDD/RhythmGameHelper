using RhythmGameHelper.Client.Services;
using RhythmGameHelper.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using RhythmGameHelper.Shared.DataStructure;
using RhythmGameHelper.Shared.DataTransfer;
using RhythmGameHelper.Shared.DataQuery;

namespace RhythmGameHelper.Client.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ISettingService _settingsService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFavoriteService _favoriteService;
        public ObservableCollection<string> Versions { get; } = new();
        public ObservableCollection<string> Categories { get; } = new();
        public ObservableCollection<Game> AllGames { get; } = new();
        public ObservableCollection<SongViewModel> SearchResults { get; } = new();

        private Game? _selectedGame;
        public Game? SelectedGame
        {
            get => _selectedGame;
            set
            {
                if(value != _selectedGame)
                {
                    _selectedGame = value;

                    OnPropertyChanged();
                    _ = OnGameSelectionChanged();
                }
            }
        }

        [ObservableProperty] private string _searchArtist = string.Empty;

        [ObservableProperty] private int _minBPM = 0;

        [ObservableProperty] private int _maxBPM = 99999;

        [ObservableProperty] private int _totalCount;

        [ObservableProperty] private int _totalPage = 1;
        
        [ObservableProperty] private int _currentPage = 1;

        [ObservableProperty] private string _selectedCategory = null!;

        [ObservableProperty] private string _selectedVersion = null!;
        
        [ObservableProperty] private bool _showDeleted = true;

        [ObservableProperty] private bool _showNotOriginal = true;

        [ObservableProperty] private string _errorText = string.Empty;

        [ObservableProperty] private string _searchText = string.Empty;

        [ObservableProperty] private bool _isNotLoading = false;

        public MainViewModel(IServiceProvider serviceprovider,ISettingService settingService,IApiService apiService, IFavoriteService favoriteService)
        {
            _settingsService = settingService;
            _serviceProvider = serviceprovider;
            _apiService = apiService;
            _favoriteService = favoriteService;

            SearchSongCommand = new RelayCommand(async _ => await PerformSearchSong(), _ => IsNotLoading);
            ClearErrorCommand = new RelayCommand(_ => ClearError());
            NextPageCommand = new RelayCommand(async _ => await PerformNextPage(), _ => IsNotLoading && CurrentPage < TotalPage);
            PreviousPageCommand = new RelayCommand(async _ => await PerformPreviousPage(), _ => IsNotLoading && CurrentPage > 1);
            OpenSettingCommand = new RelayCommand(_ => PerformOpenSetting());

            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                var games = await _apiService.SearchAllGames();
                AllGames.Clear();
                foreach (var game in games)
                {
                    AllGames.Add(game);
                }

                await _favoriteService.LoadFavoritesAsync();
                
            }
            catch (Exception ex)
            {
                ErrorText = "Failed To Load Games List With Exception: " + ex.Message;
            }

            IsNotLoading = true;
        }
    }
}
