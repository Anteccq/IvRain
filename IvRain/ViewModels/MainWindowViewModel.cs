using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using IvRain.Models.Storage;
using IvRain.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Reactive.Bindings;

namespace IvRain.ViewModels;

public class MainWindowViewModel
{
    private readonly IStorage _storage;

    public ReactiveProperty<string> Password = new("");
    public MainWindowViewModel(IStorage storage)
    {
        _storage = storage;
    }
}