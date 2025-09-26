using Microsoft.Extensions.DependencyInjection;
using RhythmGameHelper.Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RhythmGameHelper.Client.ViewModels
{
    public partial class MainViewModel
    {
        public ICommand ClearErrorCommand { get; }
        public ICommand SearchSongCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand OpenSettingCommand { get; }

        private void PerformOpenSetting()
        {
            var settingsWindow = _serviceProvider.GetRequiredService<SettingWindow>();
            settingsWindow.Owner = Application.Current.MainWindow;
            settingsWindow.ShowDialog();
        }

        private async Task PerformSearchSong()
        {
            CurrentPage = 1;
            await PerformSearch();
        }

        private async Task PerformNextPage()
        {
            if (CurrentPage < TotalPage) CurrentPage++;
            await PerformSearch();
        }

        private async Task PerformPreviousPage()
        {
            if (CurrentPage > 1) CurrentPage--;
            await PerformSearch();
        }

        private void ClearError() => ErrorText = "";
    }
}
