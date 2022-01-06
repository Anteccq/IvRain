using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using IvRain.Models;
using IvRain.ViewModels;
using Microsoft.UI;
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
            WindowUtil.ResizeMainWindow(600, 250);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is not InitializeDataPageViewModel viewModel)
                throw new InvalidOperationException("Need InitializeDataPageViewModel.");
            this.DataContext = viewModel;
            viewModel.RegistrationStatusProverProperty
                .FirstAsync(x => x == RegistrationStatus.Registered)
                .Subscribe(async x =>
                {
                    await Task.Delay(1000);
                    DispatcherQueue.TryEnqueue(() =>
                        (this.Parent as Frame)!.Navigate(typeof(PasswordManagePage),
                            null, new DrillInNavigationTransitionInfo()));
                });

            Observable.FromEventPattern<KeyEventHandler, KeyRoutedEventArgs>(
                h => h.Invoke,
                h => SecondPasswordBox.KeyDown += h,
                h => SecondPasswordBox.KeyDown -= h)
                .Subscribe(async _ => await viewModel.Register.ExecuteAsync());

            viewModel.Register
                .Subscribe(_ =>
                {
                    DispatcherQueue.TryEnqueue(() => RegistrationStatusBar.Fill = new SolidColorBrush(Colors.Orange));
                    return Task.CompletedTask;
                });

            viewModel.RegistrationStatusProverProperty.Where(x => x == RegistrationStatus.Registered).Subscribe(_ =>
                DispatcherQueue.TryEnqueue(() => RegistrationStatusBar.Fill = new SolidColorBrush(Colors.Green)));

            var firstPasswordBoxPointerEnter = UiElementPointerEnteredAsObservable(FirstPasswordBox);
            var firstPasswordBoxPointerExited = UiElementPointerExitedAsObservable(FirstPasswordBox);

            firstPasswordBoxPointerEnter
                .ObserveOn(SynchronizationContext.Current!)
                .Do(_ => FirstPasswordBox.PasswordRevealMode = PasswordRevealMode.Visible)
                .SkipUntil(firstPasswordBoxPointerExited)
                .Repeat()
                .SelectMany(_ => firstPasswordBoxPointerExited)
                .Subscribe(_ => FirstPasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden);

            var secondPasswordBoxPointerEnter = UiElementPointerEnteredAsObservable(SecondPasswordBox);
            var secondPasswordBoxPointerExited = UiElementPointerExitedAsObservable(SecondPasswordBox);

            secondPasswordBoxPointerEnter
                .ObserveOn(SynchronizationContext.Current!)
                .Do(_ => SecondPasswordBox.PasswordRevealMode = PasswordRevealMode.Visible)
                .SkipUntil(secondPasswordBoxPointerExited)
                .Repeat()
                .SelectMany(_ => secondPasswordBoxPointerExited)
                .Subscribe(_ => SecondPasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden);

            base.OnNavigatedTo(e);
        }

        private static IObservable<EventPattern<PointerRoutedEventArgs>> UiElementPointerEnteredAsObservable(UIElement element)
            => Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
                h => h.Invoke,
                h => element.PointerEntered += h,
                h => element.PointerEntered -= h);

        private static IObservable<EventPattern<PointerRoutedEventArgs>> UiElementPointerExitedAsObservable(UIElement element)
            => Observable.FromEventPattern<PointerEventHandler, PointerRoutedEventArgs>(
                h => h.Invoke,
                h => element.PointerExited += h,
                h => element.PointerExited -= h);
    }
}
