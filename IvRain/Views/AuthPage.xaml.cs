using Microsoft.UI.Xaml;
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
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using IvRain.Models.Parameter;
using IvRain.Models.Services;
using IvRain.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AuthPage : Page
{
    private Frame RootFrame => this.Parent as Frame;

    public AuthPage(AuthPageViewModel viewModel, InitializeDataPageViewModel initializeDataPageViewModel, IBlockService service)
    {
        this.InitializeComponent();
        this.DataContext = viewModel;
        this.PasswordBox.KeyDown += async (a, e) =>
        {
            if (e.Key != VirtualKey.Enter) return;
            await viewModel.ChallengeAuthenticationAsync();
        };
        AuthenticationStatusBar.Fill = new SolidColorBrush(Colors.DarkRed);
        viewModel.IsAuthenticated
            .FirstAsync(x => x)
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(_ => AuthenticationStatusBar.Fill = new SolidColorBrush(Colors.Green));

        viewModel!.IsAuthenticated.FirstAsync(x => x)
            .Delay(TimeSpan.FromMilliseconds(1000))
            .ObserveOn(SynchronizationContext.Current!)
            .Subscribe(_ =>
                RootFrame.Navigate(typeof(PasswordManagePage), new PasswordManagePageParameter(service, viewModel.Block, viewModel.InputPassword.Value), new DrillInNavigationTransitionInfo()));

        viewModel.ErrorMessage
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Subscribe(msg =>
            {
                FailedInfoBar.Message = msg;
                FailedInfoBar.IsOpen = true;
                WindowUtil.ResizeMainWindow(600, 150);
            });

        this.Loaded += async (_, _) =>
        {
            if (await viewModel.IsFirstBoot())
                RootFrame.Navigate(typeof(InitializeDataPage), initializeDataPageViewModel,
                    new EntranceNavigationTransitionInfo());
        };
    }
}