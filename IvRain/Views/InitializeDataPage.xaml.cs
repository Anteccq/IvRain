using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using IvRain.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InitializeDataPage : Page
    {
        public InitializeDataPage()
        {
            this.InitializeComponent();
            WindowUtil.ResizeMainWindow(600, 230);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not InitializeDataPageViewModel viewModel)
                throw new InvalidOperationException("Need InitializeDataPageViewModel.");
            this.DataContext = viewModel;
            viewModel.IsRegistered
                .FirstAsync(x => x)
                .ObserveOn(SynchronizationContext.Current!)
                .Subscribe(x => (this.Parent as Frame)!.Navigate(typeof(PasswordManagePage), null, new DrillInNavigationTransitionInfo()));
            base.OnNavigatedTo(e);
        }
    }
}
