using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using IvRain.Models.Cryptography;
using IvRain.Models.Services;
using IvRain.Models.Storage;
using IvRain.ViewModels;
using IvRain.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reactive.Bindings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace IvRain;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    public Window MainWindow { get; private set; }
    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);
        var provider = CreateDefaultBuilder((config, services) =>
        {
            services.AddSingleton<AuthPageViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<PasswordManagePage>();
            services.AddSingleton<AuthPage>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ICrypto, EyeCrypto>();
            services.AddSingleton<IBlockService, BlockService>();
            services.AddSingleton<IStorage, FileStorage>();
        });
        MainWindow = provider.GetService<MainWindow>();
        MainWindow!.Activate();
    }

    private static ServiceProvider CreateDefaultBuilder(Action<IConfiguration, IServiceCollection> configureService)
    {
        IConfiguration config = new ConfigurationBuilder().Build();
        IServiceCollection services = new ServiceCollection();
        configureService?.Invoke(config, services);
        return services.BuildServiceProvider();
    }
}