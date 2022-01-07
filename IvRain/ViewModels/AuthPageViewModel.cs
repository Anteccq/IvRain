using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IvRain.Models.Storage;
using Reactive.Bindings;

namespace IvRain.ViewModels;

public class AuthPageViewModel
{
    private readonly IStorage _storage;
    public ReactiveProperty<string> InputPassword { get; } = new("");
    public ReactiveProperty<bool> IsAuthenticated { get; }
    public ReactiveCommand ChallengeAuthentication { get; }
    public object Block { get; private set; }

    public AuthPageViewModel(IStorage storage)
    {
        IsAuthenticated = new ReactiveProperty<bool>(false);
        ChallengeAuthentication = new ReactiveCommand();
        ChallengeAuthentication.Subscribe(_ => IsAuthenticated.Value = true);
        _storage = storage;
;    }

    public async ValueTask<bool> IsFirstBoot()
        => !(await _storage.IsExistDataAsync(CancellationToken.None));
}