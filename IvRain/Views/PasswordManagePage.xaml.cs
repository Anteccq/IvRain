using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using Windows.Foundation;
using Windows.Foundation.Collections;
using IvRain.Models;
using IvRain.Models.Parameter;
using IvRain.Models.Services;
using IvRain.ViewModels;
using Visibility = Microsoft.UI.Xaml.Visibility;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PasswordManagePage : Page
{
    public PasswordManagePage()
    {
        this.InitializeComponent();
        WindowUtil.ResizeMainWindow(1100, 700);
        SiteListView.SelectionChanged += ToggleSwitchToVisible;
    }

    private void ToggleSwitchToVisible(object _, SelectionChangedEventArgs __)
    {
        PasswordView.Visibility = Visibility.Visible;
        SiteListView.SelectionChanged -= ToggleSwitchToVisible;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is not PasswordManagePageParameter(var service, var blocks, var password))
            throw new InvalidOperationException("Need InitializeDataPageViewModel.");
        var viewModel = new PasswordManagePageViewModel(service, blocks, password);
        this.DataContext = viewModel;
        PasswordView.PasswordChanged += () => viewModel.StoreBlock?.Execute();
        base.OnNavigatedTo(e);
    }
}