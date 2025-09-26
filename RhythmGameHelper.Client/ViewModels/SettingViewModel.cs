using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RhythmGameHelper.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RhythmGameHelper.Client.ViewModels
{
    public class UserSetting
    {
        public int PageSize { get; set; } = 15;
    }

    public partial class SettingViewModel : ObservableObject
    {
        private readonly ISettingService _settingService;
        private readonly UserSetting _userSetting;

        [ObservableProperty] private int _pageSize;
        
        public SettingViewModel(ISettingService settingsService)
        {
            _settingService = settingsService;
            _userSetting = _settingService.UserSetting;

            PageSize = _userSetting.PageSize;
        }

        [RelayCommand]
        private void ApplyClose(Window window)
        {
            if (window == null) return;
             
            _userSetting.PageSize = this.PageSize;

            _settingService.SaveSettings();

            window.DialogResult = true;
        }
    }
}
